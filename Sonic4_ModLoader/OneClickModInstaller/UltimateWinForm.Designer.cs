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
            this.chSaveDownloadedArchives = new System.Windows.Forms.CheckBox();
            this.cbExitLaunchManager = new System.Windows.Forms.CheckBox();
            this.bModPath = new System.Windows.Forms.Button();
            this.tbModURL = new System.Windows.Forms.TextBox();
            this.lType = new System.Windows.Forms.Label();
            this.lDownloadType = new System.Windows.Forms.Label();
            this.lDownloadLink = new System.Windows.Forms.Label();
            this.lDownloadTrying = new System.Windows.Forms.Label();
            this.lDownloadID = new System.Windows.Forms.Label();
            this.lModID = new System.Windows.Forms.Label();
            this.bModInstall = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.tcSettings = new System.Windows.Forms.TabControl();
            this.tPaths = new System.Windows.Forms.TabPage();
            this.bPathDownloadedArchives = new System.Windows.Forms.Button();
            this.tbDownloadedArchiveLocation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bPathCheatTables = new System.Windows.Forms.Button();
            this.cbUseLocal7zip = new System.Windows.Forms.CheckBox();
            this.bPath7z = new System.Windows.Forms.Button();
            this.tbPath7z = new System.Windows.Forms.TextBox();
            this.tbPathCheatTables = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tOther = new System.Windows.Forms.TabPage();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tcMain.SuspendLayout();
            this.tabInstaller.SuspendLayout();
            this.tcInstallation.SuspendLayout();
            this.tabCurrent.SuspendLayout();
            this.tabOverall.SuspendLayout();
            this.tabModInst.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tcSettings.SuspendLayout();
            this.tPaths.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tabInstaller);
            this.tcMain.Controls.Add(this.tabModInst);
            this.tcMain.Controls.Add(this.tabSettings);
            this.tcMain.Controls.Add(this.tabAbout);
            this.tcMain.Location = new System.Drawing.Point(12, 102);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(402, 200);
            this.tcMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tcMain.TabIndex = 1;
            // 
            // tabInstaller
            // 
            this.tabInstaller.Controls.Add(this.tcInstallation);
            this.tabInstaller.Location = new System.Drawing.Point(4, 22);
            this.tabInstaller.Name = "tabInstaller";
            this.tabInstaller.Padding = new System.Windows.Forms.Padding(3);
            this.tabInstaller.Size = new System.Drawing.Size(394, 174);
            this.tabInstaller.TabIndex = 0;
            this.tabInstaller.Text = "Installer";
            this.tabInstaller.UseVisualStyleBackColor = true;
            // 
            // tcInstallation
            // 
            this.tcInstallation.Controls.Add(this.tabCurrent);
            this.tcInstallation.Controls.Add(this.tabOverall);
            this.tcInstallation.Location = new System.Drawing.Point(0, 0);
            this.tcInstallation.Name = "tcInstallation";
            this.tcInstallation.SelectedIndex = 0;
            this.tcInstallation.Size = new System.Drawing.Size(394, 174);
            this.tcInstallation.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
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
            this.tabCurrent.Location = new System.Drawing.Point(4, 22);
            this.tabCurrent.Name = "tabCurrent";
            this.tabCurrent.Padding = new System.Windows.Forms.Padding(3);
            this.tabCurrent.Size = new System.Drawing.Size(386, 148);
            this.tabCurrent.TabIndex = 0;
            this.tabCurrent.Text = "Current installation";
            this.tabCurrent.UseVisualStyleBackColor = true;
            // 
            // lGameName
            // 
            this.lGameName.AutoSize = true;
            this.lGameName.Location = new System.Drawing.Point(110, 8);
            this.lGameName.Name = "lGameName";
            this.lGameName.Size = new System.Drawing.Size(124, 13);
            this.lGameName.TabIndex = 21;
            this.lGameName.Text = "//Game name goes here";
            // 
            // lInstallGame
            // 
            this.lInstallGame.AutoSize = true;
            this.lInstallGame.Location = new System.Drawing.Point(63, 8);
            this.lInstallGame.Name = "lInstallGame";
            this.lInstallGame.Size = new System.Drawing.Size(41, 13);
            this.lInstallGame.TabIndex = 20;
            this.lInstallGame.Text = "Game: ";
            // 
            // lInstallAdmin
            // 
            this.lInstallAdmin.Location = new System.Drawing.Point(63, 44);
            this.lInstallAdmin.Name = "lInstallAdmin";
            this.lInstallAdmin.Size = new System.Drawing.Size(256, 23);
            this.lInstallAdmin.TabIndex = 19;
            this.lInstallAdmin.Text = "Installation requires administrator privileges.";
            this.lInstallAdmin.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bUninstall
            // 
            this.bUninstall.Image = global::OneClickModInstaller.Properties.Resources.root_shield;
            this.bUninstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bUninstall.Location = new System.Drawing.Point(63, 108);
            this.bUninstall.Name = "bUninstall";
            this.bUninstall.Size = new System.Drawing.Size(256, 32);
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
            this.bInstall.Location = new System.Drawing.Point(63, 70);
            this.bInstall.Name = "bInstall";
            this.bInstall.Size = new System.Drawing.Size(256, 32);
            this.bInstall.TabIndex = 17;
            this.bInstall.Text = "Install/Fix";
            this.bInstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bInstall.UseVisualStyleBackColor = true;
            this.bInstall.Click += new System.EventHandler(this.bInstall_Click);
            // 
            // lInstallationStatus
            // 
            this.lInstallationStatus.AutoSize = true;
            this.lInstallationStatus.Location = new System.Drawing.Point(163, 21);
            this.lInstallationStatus.Name = "lInstallationStatus";
            this.lInstallationStatus.Size = new System.Drawing.Size(148, 13);
            this.lInstallationStatus.TabIndex = 16;
            this.lInstallationStatus.Text = "//Installation status goes here";
            // 
            // lInstallStatus
            // 
            this.lInstallStatus.AutoSize = true;
            this.lInstallStatus.Location = new System.Drawing.Point(63, 21);
            this.lInstallStatus.Name = "lInstallStatus";
            this.lInstallStatus.Size = new System.Drawing.Size(94, 13);
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
            this.tabOverall.Location = new System.Drawing.Point(4, 22);
            this.tabOverall.Name = "tabOverall";
            this.tabOverall.Padding = new System.Windows.Forms.Padding(3);
            this.tabOverall.Size = new System.Drawing.Size(386, 148);
            this.tabOverall.TabIndex = 1;
            this.tabOverall.Text = "Overall";
            this.tabOverall.UseVisualStyleBackColor = true;
            // 
            // lIOEp2Path
            // 
            this.lIOEp2Path.Location = new System.Drawing.Point(8, 112);
            this.lIOEp2Path.Name = "lIOEp2Path";
            this.lIOEp2Path.Size = new System.Drawing.Size(372, 26);
            this.lIOEp2Path.TabIndex = 18;
            this.lIOEp2Path.Text = "Path:";
            // 
            // lIOEp2Stat
            // 
            this.lIOEp2Stat.Location = new System.Drawing.Point(128, 88);
            this.lIOEp2Stat.Name = "lIOEp2Stat";
            this.lIOEp2Stat.Size = new System.Drawing.Size(65, 13);
            this.lIOEp2Stat.TabIndex = 17;
            this.lIOEp2Stat.Text = "Not installed";
            this.lIOEp2Stat.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lIOEp1Path
            // 
            this.lIOEp1Path.Location = new System.Drawing.Point(8, 42);
            this.lIOEp1Path.Name = "lIOEp1Path";
            this.lIOEp1Path.Size = new System.Drawing.Size(372, 26);
            this.lIOEp1Path.TabIndex = 16;
            this.lIOEp1Path.Text = "Path:";
            // 
            // bIOEp2Visit
            // 
            this.bIOEp2Visit.Location = new System.Drawing.Point(296, 78);
            this.bIOEp2Visit.Name = "bIOEp2Visit";
            this.bIOEp2Visit.Size = new System.Drawing.Size(84, 32);
            this.bIOEp2Visit.TabIndex = 15;
            this.bIOEp2Visit.Text = "Open location";
            this.bIOEp2Visit.UseVisualStyleBackColor = true;
            this.bIOEp2Visit.Click += new System.EventHandler(this.bIOEp2Visit_Click);
            // 
            // bIOEp2Uninstall
            // 
            this.bIOEp2Uninstall.Image = global::OneClickModInstaller.Properties.Resources.root_shield;
            this.bIOEp2Uninstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bIOEp2Uninstall.Location = new System.Drawing.Point(206, 78);
            this.bIOEp2Uninstall.Name = "bIOEp2Uninstall";
            this.bIOEp2Uninstall.Size = new System.Drawing.Size(84, 32);
            this.bIOEp2Uninstall.TabIndex = 14;
            this.bIOEp2Uninstall.Text = "Uninstall";
            this.bIOEp2Uninstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bIOEp2Uninstall.UseVisualStyleBackColor = true;
            this.bIOEp2Uninstall.Click += new System.EventHandler(this.bIOEp2Uninstall_Click);
            // 
            // bIOEp1Visit
            // 
            this.bIOEp1Visit.Location = new System.Drawing.Point(296, 8);
            this.bIOEp1Visit.Name = "bIOEp1Visit";
            this.bIOEp1Visit.Size = new System.Drawing.Size(84, 32);
            this.bIOEp1Visit.TabIndex = 13;
            this.bIOEp1Visit.Text = "Open location";
            this.bIOEp1Visit.UseVisualStyleBackColor = true;
            this.bIOEp1Visit.Click += new System.EventHandler(this.bIOEp1Visit_Click);
            // 
            // bIOEp1Uninstall
            // 
            this.bIOEp1Uninstall.Image = global::OneClickModInstaller.Properties.Resources.root_shield;
            this.bIOEp1Uninstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bIOEp1Uninstall.Location = new System.Drawing.Point(206, 8);
            this.bIOEp1Uninstall.Name = "bIOEp1Uninstall";
            this.bIOEp1Uninstall.Size = new System.Drawing.Size(84, 32);
            this.bIOEp1Uninstall.TabIndex = 12;
            this.bIOEp1Uninstall.Text = "Uninstall";
            this.bIOEp1Uninstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bIOEp1Uninstall.UseVisualStyleBackColor = true;
            this.bIOEp1Uninstall.Click += new System.EventHandler(this.bIOEp1Uninstall_Click);
            // 
            // lIOEp2Deco
            // 
            this.lIOEp2Deco.AutoSize = true;
            this.lIOEp2Deco.Location = new System.Drawing.Point(6, 88);
            this.lIOEp2Deco.Name = "lIOEp2Deco";
            this.lIOEp2Deco.Size = new System.Drawing.Size(96, 13);
            this.lIOEp2Deco.TabIndex = 10;
            this.lIOEp2Deco.Text = "Sonic 4: Episode 2";
            // 
            // lIOEp1Stat
            // 
            this.lIOEp1Stat.Location = new System.Drawing.Point(128, 18);
            this.lIOEp1Stat.Name = "lIOEp1Stat";
            this.lIOEp1Stat.Size = new System.Drawing.Size(65, 13);
            this.lIOEp1Stat.TabIndex = 9;
            this.lIOEp1Stat.Text = "Not installed";
            this.lIOEp1Stat.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lIOEp1Deco
            // 
            this.lIOEp1Deco.AutoSize = true;
            this.lIOEp1Deco.Location = new System.Drawing.Point(8, 18);
            this.lIOEp1Deco.Name = "lIOEp1Deco";
            this.lIOEp1Deco.Size = new System.Drawing.Size(96, 13);
            this.lIOEp1Deco.TabIndex = 8;
            this.lIOEp1Deco.Text = "Sonic 4: Episode 1";
            // 
            // tabModInst
            // 
            this.tabModInst.Controls.Add(this.chSaveDownloadedArchives);
            this.tabModInst.Controls.Add(this.cbExitLaunchManager);
            this.tabModInst.Controls.Add(this.bModPath);
            this.tabModInst.Controls.Add(this.tbModURL);
            this.tabModInst.Controls.Add(this.lType);
            this.tabModInst.Controls.Add(this.lDownloadType);
            this.tabModInst.Controls.Add(this.lDownloadLink);
            this.tabModInst.Controls.Add(this.lDownloadTrying);
            this.tabModInst.Controls.Add(this.lDownloadID);
            this.tabModInst.Controls.Add(this.lModID);
            this.tabModInst.Controls.Add(this.bModInstall);
            this.tabModInst.Location = new System.Drawing.Point(4, 22);
            this.tabModInst.Name = "tabModInst";
            this.tabModInst.Padding = new System.Windows.Forms.Padding(3);
            this.tabModInst.Size = new System.Drawing.Size(394, 174);
            this.tabModInst.TabIndex = 1;
            this.tabModInst.Text = "Install mod";
            this.tabModInst.UseVisualStyleBackColor = true;
            // 
            // chSaveDownloadedArchives
            // 
            this.chSaveDownloadedArchives.AutoSize = true;
            this.chSaveDownloadedArchives.Location = new System.Drawing.Point(9, 115);
            this.chSaveDownloadedArchives.Name = "chSaveDownloadedArchives";
            this.chSaveDownloadedArchives.Size = new System.Drawing.Size(155, 17);
            this.chSaveDownloadedArchives.TabIndex = 22;
            this.chSaveDownloadedArchives.Text = "Save downloaded archives";
            this.chSaveDownloadedArchives.UseVisualStyleBackColor = true;
            this.chSaveDownloadedArchives.CheckedChanged += new System.EventHandler(this.fake_SettingsSave);
            // 
            // cbExitLaunchManager
            // 
            this.cbExitLaunchManager.AutoSize = true;
            this.cbExitLaunchManager.Location = new System.Drawing.Point(9, 138);
            this.cbExitLaunchManager.Name = "cbExitLaunchManager";
            this.cbExitLaunchManager.Size = new System.Drawing.Size(185, 30);
            this.cbExitLaunchManager.TabIndex = 21;
            this.cbExitLaunchManager.Text = "Launch Mod Manager (if PC mod)\r\nand Exit after installation";
            this.cbExitLaunchManager.UseVisualStyleBackColor = true;
            this.cbExitLaunchManager.CheckedChanged += new System.EventHandler(this.fake_SettingsSave);
            // 
            // bModPath
            // 
            this.bModPath.Location = new System.Drawing.Point(356, 62);
            this.bModPath.Name = "bModPath";
            this.bModPath.Size = new System.Drawing.Size(32, 23);
            this.bModPath.TabIndex = 20;
            this.bModPath.Text = "...";
            this.bModPath.UseVisualStyleBackColor = true;
            this.bModPath.Click += new System.EventHandler(this.bModPath_Click);
            // 
            // tbModURL
            // 
            this.tbModURL.Location = new System.Drawing.Point(9, 64);
            this.tbModURL.Name = "tbModURL";
            this.tbModURL.Size = new System.Drawing.Size(341, 20);
            this.tbModURL.TabIndex = 19;
            this.tbModURL.TextChanged += new System.EventHandler(this.tbModURL_TextChanged);
            // 
            // lType
            // 
            this.lType.AutoSize = true;
            this.lType.Location = new System.Drawing.Point(245, 26);
            this.lType.Name = "lType";
            this.lType.Size = new System.Drawing.Size(25, 13);
            this.lType.TabIndex = 18;
            this.lType.Text = "???";
            // 
            // lDownloadType
            // 
            this.lDownloadType.AutoSize = true;
            this.lDownloadType.Location = new System.Drawing.Point(231, 13);
            this.lDownloadType.Name = "lDownloadType";
            this.lDownloadType.Size = new System.Drawing.Size(54, 13);
            this.lDownloadType.TabIndex = 17;
            this.lDownloadType.Text = "Mod type:";
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
            this.lDownloadID.Location = new System.Drawing.Point(310, 13);
            this.lDownloadID.Name = "lDownloadID";
            this.lDownloadID.Size = new System.Drawing.Size(45, 13);
            this.lDownloadID.TabIndex = 13;
            this.lDownloadID.Text = "Mod ID:";
            // 
            // lModID
            // 
            this.lModID.AutoSize = true;
            this.lModID.Location = new System.Drawing.Point(324, 26);
            this.lModID.Name = "lModID";
            this.lModID.Size = new System.Drawing.Size(25, 13);
            this.lModID.TabIndex = 12;
            this.lModID.Text = "???";
            // 
            // bModInstall
            // 
            this.bModInstall.Enabled = false;
            this.bModInstall.Location = new System.Drawing.Point(234, 125);
            this.bModInstall.Name = "bModInstall";
            this.bModInstall.Size = new System.Drawing.Size(128, 32);
            this.bModInstall.TabIndex = 11;
            this.bModInstall.Text = "Install";
            this.bModInstall.UseVisualStyleBackColor = true;
            this.bModInstall.Click += new System.EventHandler(this.bModInstall_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.tcSettings);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(394, 174);
            this.tabSettings.TabIndex = 2;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // tcSettings
            // 
            this.tcSettings.Controls.Add(this.tPaths);
            this.tcSettings.Controls.Add(this.tOther);
            this.tcSettings.Location = new System.Drawing.Point(0, 0);
            this.tcSettings.Name = "tcSettings";
            this.tcSettings.SelectedIndex = 0;
            this.tcSettings.Size = new System.Drawing.Size(394, 174);
            this.tcSettings.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tcSettings.TabIndex = 3;
            // 
            // tPaths
            // 
            this.tPaths.Controls.Add(this.bPathDownloadedArchives);
            this.tPaths.Controls.Add(this.tbDownloadedArchiveLocation);
            this.tPaths.Controls.Add(this.label2);
            this.tPaths.Controls.Add(this.bPathCheatTables);
            this.tPaths.Controls.Add(this.cbUseLocal7zip);
            this.tPaths.Controls.Add(this.bPath7z);
            this.tPaths.Controls.Add(this.tbPath7z);
            this.tPaths.Controls.Add(this.tbPathCheatTables);
            this.tPaths.Controls.Add(this.label1);
            this.tPaths.Location = new System.Drawing.Point(4, 22);
            this.tPaths.Name = "tPaths";
            this.tPaths.Padding = new System.Windows.Forms.Padding(3);
            this.tPaths.Size = new System.Drawing.Size(386, 148);
            this.tPaths.TabIndex = 0;
            this.tPaths.Text = "Paths";
            this.tPaths.UseVisualStyleBackColor = true;
            // 
            // bPathDownloadedArchives
            // 
            this.bPathDownloadedArchives.Location = new System.Drawing.Point(344, 105);
            this.bPathDownloadedArchives.Name = "bPathDownloadedArchives";
            this.bPathDownloadedArchives.Size = new System.Drawing.Size(32, 23);
            this.bPathDownloadedArchives.TabIndex = 11;
            this.bPathDownloadedArchives.Text = "...";
            this.bPathDownloadedArchives.UseVisualStyleBackColor = true;
            this.bPathDownloadedArchives.Click += new System.EventHandler(this.bPathDownloadedArchives_Click);
            // 
            // tbDownloadedArchiveLocation
            // 
            this.tbDownloadedArchiveLocation.AllowDrop = true;
            this.tbDownloadedArchiveLocation.Location = new System.Drawing.Point(6, 107);
            this.tbDownloadedArchiveLocation.Name = "tbDownloadedArchiveLocation";
            this.tbDownloadedArchiveLocation.Size = new System.Drawing.Size(332, 20);
            this.tbDownloadedArchiveLocation.TabIndex = 10;
            this.tbDownloadedArchiveLocation.TextChanged += new System.EventHandler(this.fake_SettingsSave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Downloaded archive save location";
            // 
            // bPathCheatTables
            // 
            this.bPathCheatTables.Location = new System.Drawing.Point(344, 66);
            this.bPathCheatTables.Name = "bPathCheatTables";
            this.bPathCheatTables.Size = new System.Drawing.Size(32, 23);
            this.bPathCheatTables.TabIndex = 8;
            this.bPathCheatTables.Text = "...";
            this.bPathCheatTables.UseVisualStyleBackColor = true;
            this.bPathCheatTables.Click += new System.EventHandler(this.bPathCheatTables_Click);
            // 
            // cbUseLocal7zip
            // 
            this.cbUseLocal7zip.AutoSize = true;
            this.cbUseLocal7zip.Location = new System.Drawing.Point(6, 6);
            this.cbUseLocal7zip.Name = "cbUseLocal7zip";
            this.cbUseLocal7zip.Size = new System.Drawing.Size(208, 17);
            this.cbUseLocal7zip.TabIndex = 0;
            this.cbUseLocal7zip.Text = "Use a copy of 7-Zip from this computer";
            this.cbUseLocal7zip.UseVisualStyleBackColor = true;
            this.cbUseLocal7zip.CheckedChanged += new System.EventHandler(this.fake_SettingsSave);
            // 
            // bPath7z
            // 
            this.bPath7z.Enabled = false;
            this.bPath7z.Location = new System.Drawing.Point(344, 27);
            this.bPath7z.Name = "bPath7z";
            this.bPath7z.Size = new System.Drawing.Size(32, 23);
            this.bPath7z.TabIndex = 7;
            this.bPath7z.Text = "...";
            this.bPath7z.UseVisualStyleBackColor = true;
            this.bPath7z.Click += new System.EventHandler(this.bPath7z_Click);
            // 
            // tbPath7z
            // 
            this.tbPath7z.AllowDrop = true;
            this.tbPath7z.Enabled = false;
            this.tbPath7z.Location = new System.Drawing.Point(6, 29);
            this.tbPath7z.Name = "tbPath7z";
            this.tbPath7z.Size = new System.Drawing.Size(332, 20);
            this.tbPath7z.TabIndex = 1;
            this.tbPath7z.TextChanged += new System.EventHandler(this.fake_SettingsSave);
            // 
            // tbPathCheatTables
            // 
            this.tbPathCheatTables.AllowDrop = true;
            this.tbPathCheatTables.Location = new System.Drawing.Point(6, 68);
            this.tbPathCheatTables.Name = "tbPathCheatTables";
            this.tbPathCheatTables.Size = new System.Drawing.Size(332, 20);
            this.tbPathCheatTables.TabIndex = 3;
            this.tbPathCheatTables.TextChanged += new System.EventHandler(this.fake_SettingsSave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cheat Tables";
            // 
            // tOther
            // 
            this.tOther.Location = new System.Drawing.Point(4, 22);
            this.tOther.Name = "tOther";
            this.tOther.Padding = new System.Windows.Forms.Padding(3);
            this.tOther.Size = new System.Drawing.Size(386, 148);
            this.tOther.TabIndex = 1;
            this.tOther.Text = "Other";
            this.tOther.UseVisualStyleBackColor = true;
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
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 325);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(426, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusBar
            // 
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(103, 17);
            this.statusBar.Text = "This is a status bar";
            this.statusBar.TextChanged += new System.EventHandler(statusBar_TextChanged);
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
            // pictureBox1
            // 
            this.pictureBox1.Image = global::OneClickModInstaller.Properties.Resources.ocmi_logo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(402, 84);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // UltimateWinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 347);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UltimateWinForm";
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
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
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
        private System.Windows.Forms.Button bPathCheatTables;
        private System.Windows.Forms.Button bPath7z;
        private System.Windows.Forms.TextBox tbPathCheatTables;
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
    }
}