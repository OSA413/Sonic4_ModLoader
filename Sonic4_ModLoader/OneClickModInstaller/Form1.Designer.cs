namespace OneClickModInstaller
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.bDownload = new System.Windows.Forms.Button();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lModID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 144);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(280, 32);
            this.progressBar.TabIndex = 0;
            // 
            // bDownload
            // 
            this.bDownload.Location = new System.Drawing.Point(164, 106);
            this.bDownload.Name = "bDownload";
            this.bDownload.Size = new System.Drawing.Size(128, 32);
            this.bDownload.TabIndex = 1;
            this.bDownload.Text = "Download";
            this.bDownload.UseVisualStyleBackColor = true;
            this.bDownload.Click += new System.EventHandler(this.bDownload_Click);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusBar.Location = new System.Drawing.Point(0, 179);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(304, 22);
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // lModID
            // 
            this.lModID.AutoSize = true;
            this.lModID.Location = new System.Drawing.Point(28, 74);
            this.lModID.Name = "lModID";
            this.lModID.Size = new System.Drawing.Size(31, 13);
            this.lModID.TabIndex = 3;
            this.lModID.Text = "0000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Mod ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(259, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "You are trying to download a mod from GameBanana.\r\nAren\'t you?";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 201);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lModID);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.bDownload);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "1-Click Mod Installer";
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button bDownload;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label lModID;
    }
}

