namespace OneClickModInstaller
{
    partial class UltimateWinForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UltimateWinForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabInstallation = new System.Windows.Forms.TabPage();
            this.lGameName = new System.Windows.Forms.Label();
            this.lInstallGame = new System.Windows.Forms.Label();
            this.lInstallAdmin = new System.Windows.Forms.Label();
            this.bUninstall = new System.Windows.Forms.Button();
            this.bInstall = new System.Windows.Forms.Button();
            this.lInstallationStatus = new System.Windows.Forms.Label();
            this.lInstallStatus = new System.Windows.Forms.Label();
            this.tabDownload = new System.Windows.Forms.TabPage();
            this.lType = new System.Windows.Forms.Label();
            this.lDownloadType = new System.Windows.Forms.Label();
            this.lURL = new System.Windows.Forms.Label();
            this.lDownloadLink = new System.Windows.Forms.Label();
            this.lDownloadTrying = new System.Windows.Forms.Label();
            this.lDownloadID = new System.Windows.Forms.Label();
            this.lModID = new System.Windows.Forms.Label();
            this.bDownload = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabInstallation.SuspendLayout();
            this.tabDownload.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::OneClickModInstaller.Properties.Resources.ocmi_logo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(402, 84);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabInstallation);
            this.tabControl1.Controls.Add(this.tabDownload);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Controls.Add(this.tabAbout);
            this.tabControl1.Location = new System.Drawing.Point(12, 102);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(402, 200);
            this.tabControl1.TabIndex = 1;
            // 
            // tabInstallation
            // 
            this.tabInstallation.Controls.Add(this.lGameName);
            this.tabInstallation.Controls.Add(this.lInstallGame);
            this.tabInstallation.Controls.Add(this.lInstallAdmin);
            this.tabInstallation.Controls.Add(this.bUninstall);
            this.tabInstallation.Controls.Add(this.bInstall);
            this.tabInstallation.Controls.Add(this.lInstallationStatus);
            this.tabInstallation.Controls.Add(this.lInstallStatus);
            this.tabInstallation.Location = new System.Drawing.Point(4, 22);
            this.tabInstallation.Name = "tabInstallation";
            this.tabInstallation.Padding = new System.Windows.Forms.Padding(3);
            this.tabInstallation.Size = new System.Drawing.Size(394, 174);
            this.tabInstallation.TabIndex = 0;
            this.tabInstallation.Text = "Installation";
            this.tabInstallation.UseVisualStyleBackColor = true;
            // 
            // lGameName
            // 
            this.lGameName.AutoSize = true;
            this.lGameName.Location = new System.Drawing.Point(117, 23);
            this.lGameName.Name = "lGameName";
            this.lGameName.Size = new System.Drawing.Size(124, 13);
            this.lGameName.TabIndex = 14;
            this.lGameName.Text = "//Game name goes here";
            // 
            // lInstallGame
            // 
            this.lInstallGame.AutoSize = true;
            this.lInstallGame.Location = new System.Drawing.Point(70, 23);
            this.lInstallGame.Name = "lInstallGame";
            this.lInstallGame.Size = new System.Drawing.Size(41, 13);
            this.lInstallGame.TabIndex = 13;
            this.lInstallGame.Text = "Game: ";
            // 
            // lInstallAdmin
            // 
            this.lInstallAdmin.Location = new System.Drawing.Point(70, 59);
            this.lInstallAdmin.Name = "lInstallAdmin";
            this.lInstallAdmin.Size = new System.Drawing.Size(256, 23);
            this.lInstallAdmin.TabIndex = 12;
            this.lInstallAdmin.Text = "Installation requires administrator privileges.";
            this.lInstallAdmin.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bUninstall
            // 
            this.bUninstall.Image = global::OneClickModInstaller.Properties.Resources.root_shield;
            this.bUninstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bUninstall.Location = new System.Drawing.Point(70, 123);
            this.bUninstall.Name = "bUninstall";
            this.bUninstall.Size = new System.Drawing.Size(256, 32);
            this.bUninstall.TabIndex = 11;
            this.bUninstall.Text = "Uninstall";
            this.bUninstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bUninstall.UseVisualStyleBackColor = true;
            this.bUninstall.Click += new System.EventHandler(this.bUninstall_Click);
            // 
            // bInstall
            // 
            this.bInstall.Image = global::OneClickModInstaller.Properties.Resources.root_shield;
            this.bInstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bInstall.Location = new System.Drawing.Point(70, 85);
            this.bInstall.Name = "bInstall";
            this.bInstall.Size = new System.Drawing.Size(256, 32);
            this.bInstall.TabIndex = 10;
            this.bInstall.Text = "Install/Fix";
            this.bInstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bInstall.UseVisualStyleBackColor = true;
            this.bInstall.Click += new System.EventHandler(this.bInstall_Click);
            // 
            // lInstallationStatus
            // 
            this.lInstallationStatus.AutoSize = true;
            this.lInstallationStatus.Location = new System.Drawing.Point(170, 36);
            this.lInstallationStatus.Name = "lInstallationStatus";
            this.lInstallationStatus.Size = new System.Drawing.Size(148, 13);
            this.lInstallationStatus.TabIndex = 9;
            this.lInstallationStatus.Text = "//Installation status goes here";
            // 
            // lInstallStatus
            // 
            this.lInstallStatus.AutoSize = true;
            this.lInstallStatus.Location = new System.Drawing.Point(70, 36);
            this.lInstallStatus.Name = "lInstallStatus";
            this.lInstallStatus.Size = new System.Drawing.Size(94, 13);
            this.lInstallStatus.TabIndex = 8;
            this.lInstallStatus.Text = "Installation status: ";
            // 
            // tabDownload
            // 
            this.tabDownload.Controls.Add(this.lType);
            this.tabDownload.Controls.Add(this.lDownloadType);
            this.tabDownload.Controls.Add(this.lURL);
            this.tabDownload.Controls.Add(this.lDownloadLink);
            this.tabDownload.Controls.Add(this.lDownloadTrying);
            this.tabDownload.Controls.Add(this.lDownloadID);
            this.tabDownload.Controls.Add(this.lModID);
            this.tabDownload.Controls.Add(this.bDownload);
            this.tabDownload.Location = new System.Drawing.Point(4, 22);
            this.tabDownload.Name = "tabDownload";
            this.tabDownload.Padding = new System.Windows.Forms.Padding(3);
            this.tabDownload.Size = new System.Drawing.Size(394, 174);
            this.tabDownload.TabIndex = 1;
            this.tabDownload.Text = "Download";
            this.tabDownload.UseVisualStyleBackColor = true;
            // 
            // lType
            // 
            this.lType.AutoSize = true;
            this.lType.Location = new System.Drawing.Point(70, 122);
            this.lType.Name = "lType";
            this.lType.Size = new System.Drawing.Size(25, 13);
            this.lType.TabIndex = 18;
            this.lType.Text = "???";
            // 
            // lDownloadType
            // 
            this.lDownloadType.AutoSize = true;
            this.lDownloadType.Location = new System.Drawing.Point(56, 109);
            this.lDownloadType.Name = "lDownloadType";
            this.lDownloadType.Size = new System.Drawing.Size(54, 13);
            this.lDownloadType.TabIndex = 17;
            this.lDownloadType.Text = "Mod type:";
            // 
            // lURL
            // 
            this.lURL.AutoEllipsis = true;
            this.lURL.Location = new System.Drawing.Point(24, 61);
            this.lURL.MaximumSize = new System.Drawing.Size(364, 26);
            this.lURL.Name = "lURL";
            this.lURL.Size = new System.Drawing.Size(364, 26);
            this.lURL.TabIndex = 16;
            this.lURL.Text = "Unknown";
            // 
            // lDownloadLink
            // 
            this.lDownloadLink.AutoSize = true;
            this.lDownloadLink.Location = new System.Drawing.Point(6, 48);
            this.lDownloadLink.Name = "lDownloadLink";
            this.lDownloadLink.Size = new System.Drawing.Size(77, 13);
            this.lDownloadLink.TabIndex = 15;
            this.lDownloadLink.Text = "Download link:";
            // 
            // lDownloadTrying
            // 
            this.lDownloadTrying.AutoSize = true;
            this.lDownloadTrying.Location = new System.Drawing.Point(6, 13);
            this.lDownloadTrying.Name = "lDownloadTrying";
            this.lDownloadTrying.Size = new System.Drawing.Size(104, 26);
            this.lDownloadTrying.TabIndex = 14;
            this.lDownloadTrying.Text = "You are trying to {0}.\r\nAren\'t you?";
            // 
            // lDownloadID
            // 
            this.lDownloadID.AutoSize = true;
            this.lDownloadID.Location = new System.Drawing.Point(135, 109);
            this.lDownloadID.Name = "lDownloadID";
            this.lDownloadID.Size = new System.Drawing.Size(45, 13);
            this.lDownloadID.TabIndex = 13;
            this.lDownloadID.Text = "Mod ID:";
            // 
            // lModID
            // 
            this.lModID.AutoSize = true;
            this.lModID.Location = new System.Drawing.Point(149, 122);
            this.lModID.Name = "lModID";
            this.lModID.Size = new System.Drawing.Size(25, 13);
            this.lModID.TabIndex = 12;
            this.lModID.Text = "???";
            // 
            // bDownload
            // 
            this.bDownload.Location = new System.Drawing.Point(211, 109);
            this.bDownload.Name = "bDownload";
            this.bDownload.Size = new System.Drawing.Size(128, 32);
            this.bDownload.TabIndex = 11;
            this.bDownload.Text = "Download";
            this.bDownload.UseVisualStyleBackColor = true;
            this.bDownload.Click += new System.EventHandler(this.bDownload_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(394, 174);
            this.tabSettings.TabIndex = 2;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // tabAbout
            // 
            this.tabAbout.Location = new System.Drawing.Point(4, 22);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabAbout.Size = new System.Drawing.Size(394, 174);
            this.tabAbout.TabIndex = 3;
            this.tabAbout.Text = "About";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusBar.Location = new System.Drawing.Point(0, 325);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(426, 22);
            this.statusBar.TabIndex = 3;
            this.statusBar.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(103, 17);
            this.toolStripStatusLabel1.Text = "This is a status bar";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 306);
            this.progressBar.MarqueeAnimationSpeed = 0;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(402, 16);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 10;
            // 
            // UltimateWinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 347);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UltimateWinForm";
            this.Text = "One-Click Mod Installer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabInstallation.ResumeLayout(false);
            this.tabInstallation.PerformLayout();
            this.tabDownload.ResumeLayout(false);
            this.tabDownload.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInstallation;
        private System.Windows.Forms.TabPage tabDownload;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.Label lGameName;
        private System.Windows.Forms.Label lInstallGame;
        private System.Windows.Forms.Label lInstallAdmin;
        private System.Windows.Forms.Button bUninstall;
        private System.Windows.Forms.Button bInstall;
        private System.Windows.Forms.Label lInstallationStatus;
        private System.Windows.Forms.Label lInstallStatus;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label lType;
        private System.Windows.Forms.Label lDownloadType;
        public System.Windows.Forms.Label lURL;
        private System.Windows.Forms.Label lDownloadLink;
        private System.Windows.Forms.Label lDownloadTrying;
        private System.Windows.Forms.Label lDownloadID;
        internal System.Windows.Forms.Label lModID;
        private System.Windows.Forms.Button bDownload;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}