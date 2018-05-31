using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace RDS_Log_Downloader
{
    public partial class Form1 : Form
    {

        //アクセスキーを識別するAWSプロファイル名
        private const string AWS_PROFILE_NAME = "RDS_Log_Downloader_Profile";

        public Form1()
        {
            InitializeComponent();

            //大きさ固定
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //フォームが最大化されないようにする
            this.MaximizeBox = false;
            //フォームが最小化されないようにする
            this.MinimizeBox = false;

            Txt_instance_name.Text = "";
            Txt_log_path.Text = "";

            //設定を読み込む
            if (string.IsNullOrEmpty(Properties.Settings.Default.last_save_path))
            {
                //カレントディレクトリを初期値にする。
                string stCurrentDir = Environment.CurrentDirectory;
                log_base_path = stCurrentDir;
            }
            else
            {
                //設定を読む
                log_base_path = Properties.Settings.Default.last_save_path;
            }
            Txt_log_path.Text = log_base_path;

            //プロファイルから情報を読む
            var chain = new CredentialProfileStoreChain();
            AWSCredentials awsCredentials;
            if (chain.TryGetAWSCredentials(AWS_PROFILE_NAME, out awsCredentials))
            {
                //読み込みOK
            }
            else
            {
                MessageBox.Show("AWSへ接続に必要なアクセスキー、シークレットキーを設定してください。");
            }
            if (string.IsNullOrEmpty(Properties.Settings.Default.instance_id))
            {
                //インスタンスIDの初期値は空
                Txt_instance_name.Text = "";
            }
            else
            {
                //前回起動時の値を取得
                Txt_instance_name.Text = Properties.Settings.Default.instance_id;
            }

        }
        private string log_base_path;

        private void button1_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialogクラスのインスタンスを作成
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //上部に表示する説明テキストを指定する
            fbd.Description = "ログを保存するフォルダを指定してください。";
            //ルートフォルダを指定する
            //デフォルトでDesktop
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            //最初に選択するフォルダを指定する
            //RootFolder以下にあるフォルダである必要がある
            fbd.SelectedPath = log_base_path;
            //ユーザーが新しいフォルダを作成できるようにする
            //デフォルトでTrue
            fbd.ShowNewFolderButton = true;

            //ダイアログを表示する
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                //選択されたフォルダを表示する
                log_base_path = fbd.SelectedPath;
                Txt_log_path.Text = log_base_path;
                chk_write(log_base_path);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Txt_instance_name.Text))
            {
                MessageBox.Show("RDSインスタンス名を入れてください。", "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            //プロファイルから情報を読む
            var chain = new CredentialProfileStoreChain();
            AWSCredentials awsCredentials;
            if (chain.TryGetAWSCredentials(AWS_PROFILE_NAME, out awsCredentials))
            {
                //ログ出力先に書き込めるかチェック
                if (!chk_write(log_base_path))
                {
                    return;
                }

                //読み込みOK
                DialogResult ret = MessageBox.Show("取得開始します。", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                if (ret == DialogResult.Cancel)
                {
                    return;
                }
                button1.Enabled = false;
                button2.Enabled = false;
                Btn_Regist_Key.Enabled = false;

                try
                {
                    CredentialProfile profile;
                    chain.TryGetProfile(AWS_PROFILE_NAME, out profile);

                    Aws_Util rds = new Aws_Util(awsCredentials, profile.Region);
                    rds.db_instance_identifier = Txt_instance_name.Text;
                    rds.log_base_path = log_base_path;
                    //RDSログファイル名一覧取得
                    rds.setRdsLogAttr();

                    progressBar1.Value = 0;
                    // Progressクラスのインスタンスを生成
                    var p = new Progress<int>(ShowProgress);
                    await Task.Run(() => DownloadRdsLogFiles(rds,p, rds.rdslogattr.Count()));
                    // 処理結果の表示
                    progressBar1.Text = "ログデータ取得完了！！";
                    progressBar1.Value = 100;

                    //問題なければ設定を保存する。
                    Properties.Settings.Default.last_save_path = log_base_path;
                    Properties.Settings.Default.instance_id = Txt_instance_name.Text;

                    Properties.Settings.Default.Save();

                    MessageBox.Show("取得完了しました。");
                }
                catch (Amazon.RDS.Model.DBInstanceNotFoundException ex)
                {
                    MessageBox.Show("RDSインスタンスが存在しません：" + Environment.NewLine + ex.Message, "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                }
                catch (Amazon.RDS.AmazonRDSException ex)
                {
                    MessageBox.Show("RDSインスタンス名が不正です。：" + Environment.NewLine + ex.Message, "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                }
                finally
                {
                    button1.Enabled = Enabled;
                    button2.Enabled = Enabled;
                    Btn_Regist_Key.Enabled = Enabled;
                }
                return;
            }
            else
            {
                MessageBox.Show("AWSへ接続に必要なアクセスキー、シークレットキーを設定してください。");
                return;
            }
        }
        /// <summary>
        /// 指定したディレクトリに書き込みができるかチェック
        /// </summary>
        /// <param name="write_path"></param>
        private bool chk_write(string write_path)
        {
            try
            {
                if (Directory.Exists(write_path))
                {
                    string name = Path.GetRandomFileName();
                    File.WriteAllText(write_path + "\\" + name, "test");
                    File.Delete(write_path + "\\" + name);
                    Directory.CreateDirectory(write_path + "\\" + name);
                    Directory.Delete(write_path + "\\" + name);
                    //問題なければパスの設定を保存する。
                    Properties.Settings.Default.last_save_path = write_path;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    throw new DirectoryNotFoundException();
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("アクセス権がありません。" + Environment.NewLine + "Access to the directory " + write_path + " is not permitted", "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("ディレクトリが存在しません。" + Environment.NewLine + ex.Message, "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            return true;

        }
        /// <summary>
        /// AWSキー登録ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Regist_Key_Click(object sender, EventArgs e)
        {
            config config_form = new config(AWS_PROFILE_NAME);
            config_form.ShowDialog();
            config_form.Dispose();
        }
        /// <summary>
        /// RDSログファイルをダウンロードします。
        /// </summary>
        /// <param name="rds"></param>
        /// <param name="progress"></param>
        /// <param name="n"></param>
        private void DownloadRdsLogFiles(Aws_Util rds,IProgress<int> progress, int n)
        {
            int i=0;
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = 8;//同時8スレッド
            Parallel.ForEach(rds.rdslogattr,options, (current_rds_log) => {
                //進捗率カウントアップ
                Interlocked.Increment(ref i);
                //ディレクトリ作成
                string str_timestamp = current_rds_log.dt_timestamp.ToString("yyyyMMdd");
                string log_path = log_base_path + "\\" + str_timestamp + "\\";
                if (!Directory.Exists(log_path))
                {
                    Directory.CreateDirectory(log_path);
                }
                //ファイル名はslowquery/mysql-slowquery.logのような形式なので、ファイル名だけ抜き出す。
                string log_file_name = log_path + current_rds_log.download_log_file_name.Split('/')[1];
                Console.WriteLine(log_file_name);//for debug...

                //一旦ファイル削除(上書きする為)
                if (File.Exists(log_file_name))
                {
                    File.Delete(log_file_name);
                }
                //ファイル保存
                rds.get_log_data(current_rds_log.download_log_file_name, log_file_name, current_rds_log.dt_timestamp);

                int percentage = i * 100 / n; // 進捗率
                progress.Report(percentage);

            });
        }
        // 進捗を表示するメソッド（これはUIスレッドで呼び出される）
        private void ShowProgress(int percent)
        {
            progressBar1.Value = percent;
        }
    }
}
