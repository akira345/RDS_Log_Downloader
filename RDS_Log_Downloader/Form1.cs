using System;
using System.IO;
using System.Windows.Forms;

namespace RDS_Log_Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

        private void button2_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Txt_instance_name.Text))
            {
                MessageBox.Show("RDSインスタンス名を入れてください。", "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("取得開始します。");
            try
            {
                Aws_Util rds = new Aws_Util();
                rds.db_instance_identifier = Txt_instance_name.Text;
                rds.log_base_path = log_base_path;
                rds.get_logs();
            }
            catch (Amazon.RDS.Model.DBInstanceNotFoundException ex)
            {
                MessageBox.Show("RDSインスタンスが存在しません：" + Environment.NewLine + ex.Message, "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("取得完了しました。");
        }
        /// <summary>
        /// 指定したディレクトリに書き込みができるかチェック
        /// </summary>
        /// <param name="write_path"></param>
        private void chk_write(string write_path)
        {
            try
            {
                if (Directory.Exists(write_path))
                {
                    string name = Path.GetRandomFileName();
                    File.WriteAllText(write_path + "\\" + name, "test");
                    File.Delete(write_path + "\\" + name);
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
                return;
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("ディレクトリが存在しません。" + Environment.NewLine + ex.Message, "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

        }


    }
}
