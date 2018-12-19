namespace OneClickModInstaller
{
    partial class DownloadForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadForm));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.bDownload = new System.Windows.Forms.Button();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lModID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lURL = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lType = new System.Windows.Forms.Label();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 144);
            this.progressBar.MarqueeAnimationSpeed = 0;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(280, 32);
            this.progressBar.Step = 1;
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
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(151, 17);
            this.toolStripStatusLabel1.Text = "A wild {0} button appeared!";
            // 
            // lModID
            // 
            this.lModID.AutoSize = true;
            this.lModID.Location = new System.Drawing.Point(26, 110);
            this.lModID.Name = "lModID";
            this.lModID.Size = new System.Drawing.Size(25, 13);
            this.lModID.TabIndex = 3;
            this.lModID.Text = "???";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 97);
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
            this.label3.Size = new System.Drawing.Size(104, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "You are trying to {0}.\r\nAren\'t you?";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Download link:";
            // 
            // lURL
            // 
            this.lURL.AutoEllipsis = true;
            this.lURL.Location = new System.Drawing.Point(26, 58);
            this.lURL.MaximumSize = new System.Drawing.Size(266, 45);
            this.lURL.Name = "lURL";
            this.lURL.Size = new System.Drawing.Size(266, 45);
            this.lURL.TabIndex = 7;
            this.lURL.Text = "Unknown";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Mod type:";
            // 
            // lType
            // 
            this.lType.AutoSize = true;
            this.lType.Location = new System.Drawing.Point(26, 84);
            this.lType.Name = "lType";
            this.lType.Size = new System.Drawing.Size(25, 13);
            this.lType.TabIndex = 9;
            this.lType.Text = "???";
            // 
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 201);
            this.Controls.Add(this.lType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lURL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lModID);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.bDownload);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DownloadForm";
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
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lURL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lType;
    }
}

