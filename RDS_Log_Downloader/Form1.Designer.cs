namespace RDS_Log_Downloader
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.Txt_log_path = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.Txt_instance_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_Regist_Key = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(441, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "ログ保存場所";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Txt_log_path
            // 
            this.Txt_log_path.Location = new System.Drawing.Point(40, 12);
            this.Txt_log_path.Name = "Txt_log_path";
            this.Txt_log_path.ReadOnly = true;
            this.Txt_log_path.Size = new System.Drawing.Size(395, 19);
            this.Txt_log_path.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(40, 112);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(476, 61);
            this.button2.TabIndex = 2;
            this.button2.Text = "取得";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Txt_instance_name
            // 
            this.Txt_instance_name.Location = new System.Drawing.Point(40, 62);
            this.Txt_instance_name.Name = "Txt_instance_name";
            this.Txt_instance_name.Size = new System.Drawing.Size(202, 19);
            this.Txt_instance_name.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "RDSインスタンス名";
            // 
            // Btn_Regist_Key
            // 
            this.Btn_Regist_Key.Location = new System.Drawing.Point(441, 58);
            this.Btn_Regist_Key.Name = "Btn_Regist_Key";
            this.Btn_Regist_Key.Size = new System.Drawing.Size(88, 23);
            this.Btn_Regist_Key.TabIndex = 6;
            this.Btn_Regist_Key.Text = "AWSキー登録";
            this.Btn_Regist_Key.UseVisualStyleBackColor = true;
            this.Btn_Regist_Key.Click += new System.EventHandler(this.Btn_Regist_Key_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 211);
            this.Controls.Add(this.Btn_Regist_Key);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Txt_instance_name);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Txt_log_path);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "RDSログダウンローダVer1.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Txt_log_path;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox Txt_instance_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_Regist_Key;
    }
}

