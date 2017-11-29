namespace RDS_Log_Downloader
{
    partial class config
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(config));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_Save = new System.Windows.Forms.Button();
            this.Txt_AWS_SecretKey = new System.Windows.Forms.TextBox();
            this.Txt_AWS_AccessKey = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "シークレットキー";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "アクセスキー";
            // 
            // Btn_Save
            // 
            this.Btn_Save.Location = new System.Drawing.Point(331, 85);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(75, 23);
            this.Btn_Save.TabIndex = 7;
            this.Btn_Save.Text = "保存";
            this.Btn_Save.UseVisualStyleBackColor = true;
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // Txt_AWS_SecretKey
            // 
            this.Txt_AWS_SecretKey.Location = new System.Drawing.Point(102, 51);
            this.Txt_AWS_SecretKey.Name = "Txt_AWS_SecretKey";
            this.Txt_AWS_SecretKey.Size = new System.Drawing.Size(315, 19);
            this.Txt_AWS_SecretKey.TabIndex = 6;
            // 
            // Txt_AWS_AccessKey
            // 
            this.Txt_AWS_AccessKey.Location = new System.Drawing.Point(102, 14);
            this.Txt_AWS_AccessKey.Name = "Txt_AWS_AccessKey";
            this.Txt_AWS_AccessKey.Size = new System.Drawing.Size(315, 19);
            this.Txt_AWS_AccessKey.TabIndex = 5;
            // 
            // config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 120);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Btn_Save);
            this.Controls.Add(this.Txt_AWS_SecretKey);
            this.Controls.Add(this.Txt_AWS_AccessKey);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "config";
            this.Text = "config";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.config_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_Save;
        private System.Windows.Forms.TextBox Txt_AWS_SecretKey;
        private System.Windows.Forms.TextBox Txt_AWS_AccessKey;
    }
}