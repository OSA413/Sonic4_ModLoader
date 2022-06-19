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
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tabInstaller = new System.Windows.Forms.TabPage();
            this.tcInstallation = new System.Windows.Forms.TabControl();
            this.tabCurrent = new System.Windows.Forms.TabPage();
            this.lGameName = new System.Windows.Forms.Label();
            this.lInstallGame = new System.Windows.Forms.Label();
            this.lInstallAdmin = new System.Windows.Forms.Label();
            this.bUninstall = new System.Windows.Forms.Button();
            this.bInstall = new System.Windows.Forms.Button();
            this.lInstallationStatus = new System.Windows.Forms.Label();
            this.lInstallStatus = new System.Windows.Forms.Label();
            this.tabOverall = new System.Windows.Forms.TabPage();
            this.lIOEp2Path = new System.Windows.Forms.Label();
            this.lIOEp2Stat = new System.Windows.Forms.Label();
            this.lIOEp1Path = new System.Windows.Forms.Label();
            this.bIOEp2Visit = new System.Windows.Forms.Button();
            this.bIOEp2Uninstall = new System.Windows.Forms.Button();
            this.bIOEp1Visit = new System.Windows.Forms.Button();
            this.bIOEp1Uninstall = new System.Windows.Forms.Button();
            this.lIOEp2Deco = new System.Windows.Forms.Label();
            this.lIOEp1Stat = new System.Windows.Forms.Label();
            this.lIOEp1Deco = new System.Windows.Forms.Label();
            this.tabModInst = new System.Windows.Forms.TabPage();
            this.lType = new System.Windows.Forms.Label();
            this.lDownloadType = new System.Windows.Forms.Label();
            this.lDownloadID = new System.Windows.Forms.Label();
            this.lModID = new System.Windows.Forms.Label();
            this.chSaveDownloadedArchives = new System.Windows.Forms.CheckBox();
            this.cbExitLaunchManager = new System.Windows.Forms.CheckBox();
            this.bModPath = new System.Windows.Forms.Button();
            this.tbModURL = new System.Windows.Forms.TextBox();
            this.lDownloadLink = new System.Windows.Forms.Label();
            this.lDownloadTrying = new System.Windows.Forms.Label();
            this.bModInstall = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.tcSettings = new System.Windows.Forms.TabControl();
            this.tPaths = new System.Windows.Forms.TabPage();
            this.bPathDownloadedArchives = new System.Windows.Forms.Button();
            this.tbDownloadedArchiveLocation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbUseLocal7zip = new System.Windows.Forms.CheckBox();
            this.bPath7z = new System.Windows.Forms.Button();
            this.tbPath7z = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tOther = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.link7z = new System.Windows.Forms.LinkLabel();
            this.linkMain = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.logo = new System.Windows.Forms.PictureBox();
            this.tcMain.SuspendLayout();
            this.tabInstaller.SuspendLayout();
            this.tcInstallation.SuspendLayout();
            this.tabCurrent.SuspendLayout();
            this.tabOverall.SuspendLayout();
            this.tabModInst.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tcSettings.SuspendLayout();
            this.tPaths.SuspendLayout();
            this.tOther.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tabInstaller);
            this.tcMain.Controls.Add(this.tabModInst);
            this.tcMain.Controls.Add(this.tabSettings);
            this.tcMain.Controls.Add(this.tabAbout);
            this.tcMain.Location = new System.Drawing.Point(16, 115);
            this.tcMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tcMain.Name = "tcMain";
            this.tcMain.Padding = new System.Drawing.Point(32, 3);
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(501, 308);
            this.tcMain.TabIndex = 1;
            // 
            // tabInstaller
            // 
            this.tabInstaller.Controls.Add(this.tcInstallation);
            this.tabInstaller.Location = new System.Drawing.Point(4, 29);
            this.tabInstaller.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabInstaller.Name = "tabInstaller";
            this.tabInstaller.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabInstaller.Size = new System.Drawing.Size(493, 275);
            this.tabInstaller.TabIndex = 0;
            this.tabInstaller.Text = "Installer";
            this.tabInstaller.UseVisualStyleBackColor = true;
            // 
            // tcInstallation
            // 
            this.tcInstallation.Controls.Add(this.tabCurrent);
            this.tcInstallation.Controls.Add(this.tabOverall);
            this.tcInstallation.Location = new System.Drawing.Point(-7, 0);
            this.tcInstallation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tcInstallation.Name = "tcInstallation";
            this.tcInstallation.Padding = new System.Drawing.Point(32, 3);
            this.tcInstallation.SelectedIndex = 0;
            this.tcInstallation.Size = new System.Drawing.Size(504, 280);
            this.tcInstallation.TabIndex = 15;
            // 
            // tabCurrent
            // 
            this.tabCurrent.Controls.Add(this.lGameName);
            this.tabCurrent.Controls.Add(this.lInstallGame);
            this.tabCurrent.Controls.Add(this.lInstallAdmin);
            this.tabCurrent.Controls.Add(this.bUninstall);
            this.tabCurrent.Controls.Add(this.bInstall);
            this.tabCurrent.Controls.Add(this.lInstallationStatus);
            this.tabCurrent.Controls.Add(this.lInstallStatus);
            this.tabCurrent.Location = new System.Drawing.Point(4, 29);
            this.tabCurrent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabCurrent.Name = "tabCurrent";
            this.tabCurrent.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabCurrent.Size = new System.Drawing.Size(496, 247);
            this.tabCurrent.TabIndex = 0;
            this.tabCurrent.Text = "Current installation";
            this.tabCurrent.UseVisualStyleBackColor = true;
            // 
            // lGameName
            // 
            this.lGameName.AutoSize = true;
            this.lGameName.Location = new System.Drawing.Point(147, 12);
            this.lGameName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lGameName.Name = "lGameName";
            this.lGameName.Size = new System.Drawing.Size(170, 20);
            this.lGameName.TabIndex = 21;
            this.lGameName.Text = "//Game name goes here";
            // 
            // lInstallGame
            // 
            this.lInstallGame.AutoSize = true;
            this.lInstallGame.Location = new System.Drawing.Point(84, 12);
            this.lInstallGame.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lInstallGame.Name = "lInstallGame";
            this.lInstallGame.Size = new System.Drawing.Size(55, 20);
            this.lInstallGame.TabIndex = 20;
            this.lInstallGame.Text = "Game: ";
            // 
            // lInstallAdmin
            // 
            this.lInstallAdmin.Location = new System.Drawing.Point(82, 68);
            this.lInstallAdmin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lInstallAdmin.Name = "lInstallAdmin";
            this.lInstallAdmin.Size = new System.Drawing.Size(341, 35);
            this.lInstallAdmin.TabIndex = 19;
            this.lInstallAdmin.Text = "Installation requires administrator privileges.";
            this.lInstallAdmin.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bUninstall
            // 
            this.bUninstall.Image = global::OneClickModInstaller.Properties.Resources.root_shield;
            this.bUninstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bUninstall.Location = new System.Drawing.Point(82, 166);
            this.bUninstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bUninstall.Name = "bUninstall";
            this.bUninstall.Size = new System.Drawing.Size(341, 49);
            this.bUninstall.TabIndex = 18;
            this.bUninstall.Text = "Uninstall";
            this.bUninstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bUninstall.UseVisualStyleBackColor = true;
            this.bUninstall.Click += new System.EventHandler(this.bUninstall_Click);
            // 
            // bInstall
            // 
            this.bInstall.Image = global::OneClickModInstaller.Properties.Resources.root_shield;
            this.bInstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bInstall.Location = new System.Drawing.Point(82, 108);
            this.bInstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bInstall.Name = "bInstall";
            this.bInstall.Size = new System.Drawing.Size(341, 49);
            this.bInstall.TabIndex = 17;
            this.bInstall.Text = "Install/Fix";
            this.bInstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bInstall.UseVisualStyleBackColor = true;
            this.bInstall.Click += new System.EventHandler(this.bInstall_Click);
            // 
            // lInstallationStatus
            // 
            this.lInstallationStatus.AutoSize = true;
            this.lInstallationStatus.Location = new System.Drawing.Point(217, 32);
            this.lInstallationStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lInstallationStatus.Name = "lInstallationStatus";
            this.lInstallationStatus.Size = new System.Drawing.Size(205, 20);
            this.lInstallationStatus.TabIndex = 16;
            this.lInstallationStatus.Text = "//Installation status goes here";
            // 
            // lInstallStatus
            // 
            this.lInstallStatus.AutoSize = true;
            this.lInstallStatus.Location = new System.Drawing.Point(84, 32);
            this.lInstallStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lInstallStatus.Name = "lInstallStatus";
            this.lInstallStatus.Size = new System.Drawing.Size(131, 20);
            this.lInstallStatus.TabIndex = 15;
            this.lInstallStatus.Text = "Installation status: ";
            // 
            // tabOverall
            // 
            this.tabOverall.Controls.Add(this.lIOEp2Path);
            this.tabOverall.Controls.Add(this.lIOEp2Stat);
            this.tabOverall.Controls.Add(this.lIOEp1Path);
            this.tabOverall.Controls.Add(this.bIOEp2Visit);
            this.tabOverall.Controls.Add(this.bIOEp2Uninstall);
            this.tabOverall.Controls.Add(this.bIOEp1Visit);
            this.tabOverall.Controls.Add(this.bIOEp1Uninstall);
            this.tabOverall.Controls.Add(this.lIOEp2Deco);
            this.tabOverall.Controls.Add(this.lIOEp1Stat);
            this.tabOverall.Controls.Add(this.lIOEp1Deco);
            this.tabOverall.Location = new System.Drawing.Point(4, 29);
            this.tabOverall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabOverall.Name = "tabOverall";
            this.tabOverall.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabOverall.Size = new System.Drawing.Size(496, 247);
            this.tabOverall.TabIndex = 1;
            this.tabOverall.Text = "Overall";
            this.tabOverall.UseVisualStyleBackColor = true;
            // 
            // lIOEp2Path
            // 
            this.lIOEp2Path.Location = new System.Drawing.Point(11, 172);
            this.lIOEp2Path.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIOEp2Path.Name = "lIOEp2Path";
            this.lIOEp2Path.Size = new System.Drawing.Size(496, 40);
            this.lIOEp2Path.TabIndex = 18;
            this.lIOEp2Path.Text = "Path:";
            // 
            // lIOEp2Stat
            // 
            this.lIOEp2Stat.Location = new System.Drawing.Point(145, 135);
            this.lIOEp2Stat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIOEp2Stat.Name = "lIOEp2Stat";
            this.lIOEp2Stat.Size = new System.Drawing.Size(97, 20);
            this.lIOEp2Stat.TabIndex = 17;
            this.lIOEp2Stat.Text = "Not installed";
            this.lIOEp2Stat.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lIOEp1Path
            // 
            this.lIOEp1Path.Location = new System.Drawing.Point(11, 65);
            this.lIOEp1Path.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIOEp1Path.Name = "lIOEp1Path";
            this.lIOEp1Path.Size = new System.Drawing.Size(496, 40);
            this.lIOEp1Path.TabIndex = 16;
            this.lIOEp1Path.Text = "Path:";
            // 
            // bIOEp2Visit
            // 
            this.bIOEp2Visit.Location = new System.Drawing.Point(376, 121);
            this.bIOEp2Visit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bIOEp2Visit.Name = "bIOEp2Visit";
            this.bIOEp2Visit.Size = new System.Drawing.Size(112, 49);
            this.bIOEp2Visit.TabIndex = 15;
            this.bIOEp2Visit.Text = "Open location";
            this.bIOEp2Visit.UseVisualStyleBackColor = true;
            this.bIOEp2Visit.Click += new System.EventHandler(this.bIOEp2Visit_Click);
            // 
            // bIOEp2Uninstall
            // 
            this.bIOEp2Uninstall.Image = global::OneClickModInstaller.Properties.Resources.root_shield;
            this.bIOEp2Uninstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bIOEp2Uninstall.Location = new System.Drawing.Point(256, 121);
            this.bIOEp2Uninstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bIOEp2Uninstall.Name = "bIOEp2Uninstall";
            this.bIOEp2Uninstall.Size = new System.Drawing.Size(112, 49);
            this.bIOEp2Uninstall.TabIndex = 14;
            this.bIOEp2Uninstall.Text = "Uninstall";
            this.bIOEp2Uninstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bIOEp2Uninstall.UseVisualStyleBackColor = true;
            this.bIOEp2Uninstall.Click += new System.EventHandler(this.bIOEp2Uninstall_Click);
            // 
            // bIOEp1Visit
            // 
            this.bIOEp1Visit.Location = new System.Drawing.Point(376, 12);
            this.bIOEp1Visit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bIOEp1Visit.Name = "bIOEp1Visit";
            this.bIOEp1Visit.Size = new System.Drawing.Size(112, 49);
            this.bIOEp1Visit.TabIndex = 13;
            this.bIOEp1Visit.Text = "Open location";
            this.bIOEp1Visit.UseVisualStyleBackColor = true;
            this.bIOEp1Visit.Click += new System.EventHandler(this.bIOEp1Visit_Click);
            // 
            // bIOEp1Uninstall
            // 
            this.bIOEp1Uninstall.Image = global::OneClickModInstaller.Properties.Resources.root_shield;
            this.bIOEp1Uninstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bIOEp1Uninstall.Location = new System.Drawing.Point(256, 12);
            this.bIOEp1Uninstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bIOEp1Uninstall.Name = "bIOEp1Uninstall";
            this.bIOEp1Uninstall.Size = new System.Drawing.Size(112, 49);
            this.bIOEp1Uninstall.TabIndex = 12;
            this.bIOEp1Uninstall.Text = "Uninstall";
            this.bIOEp1Uninstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bIOEp1Uninstall.UseVisualStyleBackColor = true;
            this.bIOEp1Uninstall.Click += new System.EventHandler(this.bIOEp1Uninstall_Click);
            // 
            // lIOEp2Deco
            // 
            this.lIOEp2Deco.AutoSize = true;
            this.lIOEp2Deco.Location = new System.Drawing.Point(8, 135);
            this.lIOEp2Deco.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIOEp2Deco.Name = "lIOEp2Deco";
            this.lIOEp2Deco.Size = new System.Drawing.Size(129, 20);
            this.lIOEp2Deco.TabIndex = 10;
            this.lIOEp2Deco.Text = "Sonic 4: Episode 2";
            // 
            // lIOEp1Stat
            // 
            this.lIOEp1Stat.Location = new System.Drawing.Point(148, 28);
            this.lIOEp1Stat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIOEp1Stat.Name = "lIOEp1Stat";
            this.lIOEp1Stat.Size = new System.Drawing.Size(97, 20);
            this.lIOEp1Stat.TabIndex = 9;
            this.lIOEp1Stat.Text = "Not installed";
            this.lIOEp1Stat.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lIOEp1Deco
            // 
            this.lIOEp1Deco.AutoSize = true;
            this.lIOEp1Deco.Location = new System.Drawing.Point(11, 28);
            this.lIOEp1Deco.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIOEp1Deco.Name = "lIOEp1Deco";
            this.lIOEp1Deco.Size = new System.Drawing.Size(129, 20);
            this.lIOEp1Deco.TabIndex = 8;
            this.lIOEp1Deco.Text = "Sonic 4: Episode 1";
            // 
            // tabModInst
            // 
            this.tabModInst.Controls.Add(this.lType);
            this.tabModInst.Controls.Add(this.lDownloadType);
            this.tabModInst.Controls.Add(this.lDownloadID);
            this.tabModInst.Controls.Add(this.lModID);
            this.tabModInst.Controls.Add(this.chSaveDownloadedArchives);
            this.tabModInst.Controls.Add(this.cbExitLaunchManager);
            this.tabModInst.Controls.Add(this.bModPath);
            this.tabModInst.Controls.Add(this.tbModURL);
            this.tabModInst.Controls.Add(this.lDownloadLink);
            this.tabModInst.Controls.Add(this.lDownloadTrying);
            this.tabModInst.Controls.Add(this.bModInstall);
            this.tabModInst.Location = new System.Drawing.Point(4, 29);
            this.tabModInst.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabModInst.Name = "tabModInst";
            this.tabModInst.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabModInst.Size = new System.Drawing.Size(493, 275);
            this.tabModInst.TabIndex = 1;
            this.tabModInst.Text = "Install mod";
            this.tabModInst.UseVisualStyleBackColor = true;
            // 
            // lType
            // 
            this.lType.AutoSize = true;
            this.lType.Location = new System.Drawing.Point(257, 74);
            this.lType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lType.Name = "lType";
            this.lType.Size = new System.Drawing.Size(30, 20);
            this.lType.TabIndex = 18;
            this.lType.Text = "???";
            // 
            // lDownloadType
            // 
            this.lDownloadType.AutoSize = true;
            this.lDownloadType.Location = new System.Drawing.Point(239, 54);
            this.lDownloadType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lDownloadType.Name = "lDownloadType";
            this.lDownloadType.Size = new System.Drawing.Size(76, 20);
            this.lDownloadType.TabIndex = 17;
            this.lDownloadType.Text = "Mod type:";
            // 
            // lDownloadID
            // 
            this.lDownloadID.AutoSize = true;
            this.lDownloadID.Location = new System.Drawing.Point(344, 54);
            this.lDownloadID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lDownloadID.Name = "lDownloadID";
            this.lDownloadID.Size = new System.Drawing.Size(62, 20);
            this.lDownloadID.TabIndex = 13;
            this.lDownloadID.Text = "Mod ID:";
            // 
            // lModID
            // 
            this.lModID.AutoSize = true;
            this.lModID.Location = new System.Drawing.Point(363, 74);
            this.lModID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lModID.Name = "lModID";
            this.lModID.Size = new System.Drawing.Size(30, 20);
            this.lModID.TabIndex = 12;
            this.lModID.Text = "???";
            // 
            // chSaveDownloadedArchives
            // 
            this.chSaveDownloadedArchives.AutoSize = true;
            this.chSaveDownloadedArchives.Location = new System.Drawing.Point(12, 177);
            this.chSaveDownloadedArchives.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chSaveDownloadedArchives.Name = "chSaveDownloadedArchives";
            this.chSaveDownloadedArchives.Size = new System.Drawing.Size(207, 24);
            this.chSaveDownloadedArchives.TabIndex = 22;
            this.chSaveDownloadedArchives.Text = "Save downloaded archives";
            this.chSaveDownloadedArchives.UseVisualStyleBackColor = true;
            this.chSaveDownloadedArchives.CheckedChanged += new System.EventHandler(this.fake_SettingsSave);
            // 
            // cbExitLaunchManager
            // 
            this.cbExitLaunchManager.AutoSize = true;
            this.cbExitLaunchManager.Location = new System.Drawing.Point(12, 212);
            this.cbExitLaunchManager.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbExitLaunchManager.Name = "cbExitLaunchManager";
            this.cbExitLaunchManager.Size = new System.Drawing.Size(254, 44);
            this.cbExitLaunchManager.TabIndex = 21;
            this.cbExitLaunchManager.Text = "Launch Mod Manager (if PC mod)\r\nand Exit after installation";
            this.cbExitLaunchManager.UseVisualStyleBackColor = true;
            this.cbExitLaunchManager.CheckedChanged += new System.EventHandler(this.fake_SettingsSave);
            // 
            // bModPath
            // 
            this.bModPath.Location = new System.Drawing.Point(440, 98);
            this.bModPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bModPath.Name = "bModPath";
            this.bModPath.Size = new System.Drawing.Size(43, 35);
            this.bModPath.TabIndex = 20;
            this.bModPath.Text = "...";
            this.bModPath.UseVisualStyleBackColor = true;
            this.bModPath.Click += new System.EventHandler(this.bModPath_Click);
            // 
            // tbModURL
            // 
            this.tbModURL.Location = new System.Drawing.Point(12, 98);
            this.tbModURL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbModURL.MaxLength = 512;
            this.tbModURL.Name = "tbModURL";
            this.tbModURL.Size = new System.Drawing.Size(420, 27);
            this.tbModURL.TabIndex = 19;
            this.tbModURL.TextChanged += new System.EventHandler(this.tbModURL_TextChanged);
            // 
            // lDownloadLink
            // 
            this.lDownloadLink.AutoSize = true;
            this.lDownloadLink.Location = new System.Drawing.Point(8, 74);
            this.lDownloadLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lDownloadLink.Name = "lDownloadLink";
            this.lDownloadLink.Size = new System.Drawing.Size(118, 20);
            this.lDownloadLink.TabIndex = 15;
            this.lDownloadLink.Text = "Path to the mod:";
            // 
            // lDownloadTrying
            // 
            this.lDownloadTrying.AutoSize = true;
            this.lDownloadTrying.Location = new System.Drawing.Point(8, 20);
            this.lDownloadTrying.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lDownloadTrying.Name = "lDownloadTrying";
            this.lDownloadTrying.Size = new System.Drawing.Size(193, 20);
            this.lDownloadTrying.TabIndex = 14;
            this.lDownloadTrying.Text = "Enter a path or URL to mod.";
            // 
            // bModInstall
            // 
            this.bModInstall.Enabled = false;
            this.bModInstall.Location = new System.Drawing.Point(312, 192);
            this.bModInstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bModInstall.Name = "bModInstall";
            this.bModInstall.Size = new System.Drawing.Size(171, 49);
            this.bModInstall.TabIndex = 11;
            this.bModInstall.Text = "Install";
            this.bModInstall.UseVisualStyleBackColor = true;
            this.bModInstall.Click += new System.EventHandler(this.bModInstall_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.tcSettings);
            this.tabSettings.Location = new System.Drawing.Point(4, 29);
            this.tabSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabSettings.Size = new System.Drawing.Size(493, 275);
            this.tabSettings.TabIndex = 2;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // tcSettings
            // 
            this.tcSettings.Controls.Add(this.tPaths);
            this.tcSettings.Controls.Add(this.tOther);
            this.tcSettings.ItemSize = new System.Drawing.Size(195, 24);
            this.tcSettings.Location = new System.Drawing.Point(0, 0);
            this.tcSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tcSettings.Name = "tcSettings";
            this.tcSettings.SelectedIndex = 0;
            this.tcSettings.Size = new System.Drawing.Size(493, 275);
            this.tcSettings.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tcSettings.TabIndex = 3;
            // 
            // tPaths
            // 
            this.tPaths.Controls.Add(this.bPathDownloadedArchives);
            this.tPaths.Controls.Add(this.tbDownloadedArchiveLocation);
            this.tPaths.Controls.Add(this.label2);
            this.tPaths.Controls.Add(this.cbUseLocal7zip);
            this.tPaths.Controls.Add(this.bPath7z);
            this.tPaths.Controls.Add(this.tbPath7z);
            this.tPaths.Controls.Add(this.label1);
            this.tPaths.Location = new System.Drawing.Point(4, 28);
            this.tPaths.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tPaths.Name = "tPaths";
            this.tPaths.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tPaths.Size = new System.Drawing.Size(485, 243);
            this.tPaths.TabIndex = 0;
            this.tPaths.Text = "Paths";
            this.tPaths.UseVisualStyleBackColor = true;
            // 
            // bPathDownloadedArchives
            // 
            this.bPathDownloadedArchives.Location = new System.Drawing.Point(434, 161);
            this.bPathDownloadedArchives.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bPathDownloadedArchives.Name = "bPathDownloadedArchives";
            this.bPathDownloadedArchives.Size = new System.Drawing.Size(43, 35);
            this.bPathDownloadedArchives.TabIndex = 11;
            this.bPathDownloadedArchives.Text = "...";
            this.bPathDownloadedArchives.UseVisualStyleBackColor = true;
            this.bPathDownloadedArchives.Click += new System.EventHandler(this.bPathDownloadedArchives_Click);
            // 
            // tbDownloadedArchiveLocation
            // 
            this.tbDownloadedArchiveLocation.AllowDrop = true;
            this.tbDownloadedArchiveLocation.Location = new System.Drawing.Point(8, 165);
            this.tbDownloadedArchiveLocation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbDownloadedArchiveLocation.MaxLength = 512;
            this.tbDownloadedArchiveLocation.Name = "tbDownloadedArchiveLocation";
            this.tbDownloadedArchiveLocation.Size = new System.Drawing.Size(418, 27);
            this.tbDownloadedArchiveLocation.TabIndex = 10;
            this.tbDownloadedArchiveLocation.TextChanged += new System.EventHandler(this.fake_SettingsSave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 140);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(237, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Downloaded archive save location";
            // 
            // cbUseLocal7zip
            // 
            this.cbUseLocal7zip.AutoSize = true;
            this.cbUseLocal7zip.Location = new System.Drawing.Point(8, 9);
            this.cbUseLocal7zip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbUseLocal7zip.Name = "cbUseLocal7zip";
            this.cbUseLocal7zip.Size = new System.Drawing.Size(292, 24);
            this.cbUseLocal7zip.TabIndex = 0;
            this.cbUseLocal7zip.Text = "Use a copy of 7-Zip from this computer";
            this.cbUseLocal7zip.UseVisualStyleBackColor = true;
            this.cbUseLocal7zip.CheckedChanged += new System.EventHandler(this.cbUseLocal7zip_CheckedChanged);
            // 
            // bPath7z
            // 
            this.bPath7z.Enabled = false;
            this.bPath7z.Location = new System.Drawing.Point(434, 41);
            this.bPath7z.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bPath7z.Name = "bPath7z";
            this.bPath7z.Size = new System.Drawing.Size(43, 35);
            this.bPath7z.TabIndex = 7;
            this.bPath7z.Text = "...";
            this.bPath7z.UseVisualStyleBackColor = true;
            this.bPath7z.Click += new System.EventHandler(this.bPath7z_Click);
            // 
            // tbPath7z
            // 
            this.tbPath7z.AllowDrop = true;
            this.tbPath7z.Enabled = false;
            this.tbPath7z.Location = new System.Drawing.Point(8, 45);
            this.tbPath7z.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbPath7z.MaxLength = 512;
            this.tbPath7z.Name = "tbPath7z";
            this.tbPath7z.Size = new System.Drawing.Size(418, 27);
            this.tbPath7z.TabIndex = 1;
            this.tbPath7z.TextChanged += new System.EventHandler(this.fake_SettingsSave);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 12;
            // 
            // tOther
            // 
            this.tOther.Controls.Add(this.label3);
            this.tOther.Controls.Add(this.label4);
            this.tOther.Location = new System.Drawing.Point(4, 28);
            this.tOther.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tOther.Name = "tOther";
            this.tOther.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tOther.Size = new System.Drawing.Size(485, 243);
            this.tOther.TabIndex = 1;
            this.tOther.Text = "Other";
            this.tOther.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(220, 152);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "i hope";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(161, 74);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(190, 63);
            this.label4.TabIndex = 0;
            this.label4.Text = "Soon™";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabAbout
            // 
            this.tabAbout.Controls.Add(this.label8);
            this.tabAbout.Controls.Add(this.label7);
            this.tabAbout.Controls.Add(this.linkLabel1);
            this.tabAbout.Controls.Add(this.label5);
            this.tabAbout.Controls.Add(this.link7z);
            this.tabAbout.Controls.Add(this.linkMain);
            this.tabAbout.Controls.Add(this.label6);
            this.tabAbout.Location = new System.Drawing.Point(4, 29);
            this.tabAbout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabAbout.Size = new System.Drawing.Size(493, 275);
            this.tabAbout.TabIndex = 3;
            this.tabAbout.Text = "About";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(76, 132);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(349, 46);
            this.label8.TabIndex = 13;
            this.label8.Text = "Use Mod Manager to read licenses or view them in \"Mod Loader - licenses\" folder.";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(-4, 188);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(509, 40);
            this.label7.TabIndex = 11;
            this.label7.Text = "Special thanks to the Tango Desktop Project developers for their icons dedicated " +
    "to the Public Domain";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(151, 228);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(198, 20);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://tango.freedesktop.org";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(143, 75);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(214, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "7-Zip Copyright (C) Igor Pavlov";
            // 
            // link7z
            // 
            this.link7z.AutoSize = true;
            this.link7z.Location = new System.Drawing.Point(171, 95);
            this.link7z.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.link7z.Name = "link7z";
            this.link7z.Size = new System.Drawing.Size(159, 20);
            this.link7z.TabIndex = 10;
            this.link7z.TabStop = true;
            this.link7z.Text = "https://www.7-zip.org/";
            this.link7z.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
            // 
            // linkMain
            // 
            this.linkMain.AutoSize = true;
            this.linkMain.Location = new System.Drawing.Point(88, 42);
            this.linkMain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkMain.Name = "linkMain";
            this.linkMain.Size = new System.Drawing.Size(324, 20);
            this.linkMain.TabIndex = 8;
            this.linkMain.TabStop = true;
            this.linkMain.Text = "https://github.com/OSA413/Sonic4_ModLoader";
            this.linkMain.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(56, 22);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(389, 20);
            this.label6.TabIndex = 7;
            this.label6.Text = "One-Click Mod Installer by OSA413 under the MIT License";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 463);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(530, 26);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusBar
            // 
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(129, 20);
            this.statusBar.Text = "This is a status bar";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(16, 433);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressBar.MarqueeAnimationSpeed = 25;
            this.progressBar.Maximum = 1000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(501, 25);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 10;
            // 
            // logo
            // 
            this.logo.Image = global::OneClickModInstaller.Properties.Resources.ocmi_logo;
            this.logo.Location = new System.Drawing.Point(16, 14);
            this.logo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(501, 91);
            this.logo.TabIndex = 0;
            this.logo.TabStop = false;
            // 
            // UltimateWinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 489);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.logo);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "UltimateWinForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "One-Click Mod Installer";
            this.tcMain.ResumeLayout(false);
            this.tabInstaller.ResumeLayout(false);
            this.tcInstallation.ResumeLayout(false);
            this.tabCurrent.ResumeLayout(false);
            this.tabCurrent.PerformLayout();
            this.tabOverall.ResumeLayout(false);
            this.tabOverall.PerformLayout();
            this.tabModInst.ResumeLayout(false);
            this.tabModInst.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tcSettings.ResumeLayout(false);
            this.tPaths.ResumeLayout(false);
            this.tPaths.PerformLayout();
            this.tOther.ResumeLayout(false);
            this.tOther.PerformLayout();
            this.tabAbout.ResumeLayout(false);
            this.tabAbout.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tabInstaller;
        private System.Windows.Forms.TabPage tabModInst;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusBar;
        private System.Windows.Forms.Label lType;
        private System.Windows.Forms.Label lDownloadType;
        private System.Windows.Forms.Label lDownloadLink;
        private System.Windows.Forms.Label lDownloadTrying;
        private System.Windows.Forms.Label lDownloadID;
        internal System.Windows.Forms.Label lModID;
        private System.Windows.Forms.Button bModInstall;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TabControl tcInstallation;
        private System.Windows.Forms.TabPage tabCurrent;
        private System.Windows.Forms.Label lGameName;
        private System.Windows.Forms.Label lInstallGame;
        private System.Windows.Forms.Label lInstallAdmin;
        private System.Windows.Forms.Button bUninstall;
        private System.Windows.Forms.Button bInstall;
        private System.Windows.Forms.Label lInstallationStatus;
        private System.Windows.Forms.Label lInstallStatus;
        private System.Windows.Forms.TabPage tabOverall;
        private System.Windows.Forms.Button bIOEp2Visit;
        private System.Windows.Forms.Button bIOEp2Uninstall;
        private System.Windows.Forms.Button bIOEp1Visit;
        private System.Windows.Forms.Button bIOEp1Uninstall;
        private System.Windows.Forms.Label lIOEp2Deco;
        private System.Windows.Forms.Label lIOEp1Stat;
        private System.Windows.Forms.Label lIOEp1Deco;
        private System.Windows.Forms.Label lIOEp1Path;
        private System.Windows.Forms.Label lIOEp2Stat;
        private System.Windows.Forms.Label lIOEp2Path;
        private System.Windows.Forms.TabControl tcSettings;
        private System.Windows.Forms.TabPage tPaths;
        private System.Windows.Forms.TabPage tOther;
        private System.Windows.Forms.Button bPath7z;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPath7z;
        private System.Windows.Forms.CheckBox cbUseLocal7zip;
        private System.Windows.Forms.Button bModPath;
        private System.Windows.Forms.TextBox tbModURL;
        private System.Windows.Forms.CheckBox chSaveDownloadedArchives;
        private System.Windows.Forms.CheckBox cbExitLaunchManager;
        private System.Windows.Forms.Button bPathDownloadedArchives;
        private System.Windows.Forms.TextBox tbDownloadedArchiveLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel link7z;
        private System.Windows.Forms.LinkLabel linkMain;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label8;
    }
}