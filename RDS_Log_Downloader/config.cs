using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using System;
using System.Windows.Forms;

namespace RDS_Log_Downloader
{
    public partial class config : Form
    {
        private string aws_credential_profile_name;
        public config(string aws_credential_profile_name)
        {
            this.aws_credential_profile_name = aws_credential_profile_name;

            InitializeComponent();

            //大きさ固定
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //フォームが最大化されないようにする
            this.MaximizeBox = false;
            //フォームが最小化されないようにする
            this.MinimizeBox = false;

            // 参考:http://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html
            // プロファイルを読み込む
            var chain = new CredentialProfileStoreChain();
            AWSCredentials awsCredentials;

            if (chain.TryGetAWSCredentials(aws_credential_profile_name, out awsCredentials))
            {
                //読めたのでセット
                var cretiential = awsCredentials.GetCredentials();
                Txt_AWS_AccessKey.Text = cretiential.AccessKey;
                Txt_AWS_SecretKey.Text = cretiential.SecretKey;
            }
            else
            {
                //読めなかったのでクリア
                Txt_AWS_AccessKey.Text = "";
                Txt_AWS_SecretKey.Text = "";
            }
        }
        /// <summary>
        /// 保存ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (!save_config())
            {
                MessageBox.Show("アクセスキー、シークレットキーをセットしてください。");
                return;
            }
            else
            {
                this.Close();
                return;
            }
        }
        /// <summary>
        /// プロファイルにキーを保存。
        /// </summary>
        /// <returns>True:成功 False:失敗</returns>
        private bool save_config()
        {
            if (string.IsNullOrEmpty(Txt_AWS_AccessKey.Text) || string.IsNullOrEmpty(Txt_AWS_SecretKey.Text))
            {
                return false;
            }
            else
            {
                //プロファイル設定に保存
                // 参考:http://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html

                var profile_options = new CredentialProfileOptions();
                profile_options.AccessKey = Txt_AWS_AccessKey.Text;
                profile_options.SecretKey = Txt_AWS_SecretKey.Text;

                var profile = new CredentialProfile(this.aws_credential_profile_name, profile_options);
                profile.Region = Amazon.RegionEndpoint.APNortheast1; //東京リージョン
                var netSDKFile = new NetSDKCredentialsFile();
                netSDKFile.RegisterProfile(profile);

                return true;
            }
        }
        /// <summary>
        /// バツボタンが押された時などフォームクローズ時のイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void config_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!save_config())
            {
                MessageBox.Show("アクセスキー、シークレットキーをセットしてください。");
                e.Cancel = true; //ウインドウを閉じない。
            }
        }
    }
}
