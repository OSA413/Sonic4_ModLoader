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
            tcMain = new System.Windows.Forms.TabControl();
            tabInstaller = new System.Windows.Forms.TabPage();
            tcInstallation = new System.Windows.Forms.TabControl();
            tabCurrent = new System.Windows.Forms.TabPage();
            lGameName = new System.Windows.Forms.Label();
            lInstallGame = new System.Windows.Forms.Label();
            lInstallAdmin = new System.Windows.Forms.Label();
            bUninstall = new System.Windows.Forms.Button();
            bInstall = new System.Windows.Forms.Button();
            lInstallationStatus = new System.Windows.Forms.Label();
            lInstallStatus = new System.Windows.Forms.Label();
            tabOverall = new System.Windows.Forms.TabPage();
            lIOEp2Path = new System.Windows.Forms.Label();
            lIOEp2Stat = new System.Windows.Forms.Label();
            lIOEp1Path = new System.Windows.Forms.Label();
            bIOEp2Visit = new System.Windows.Forms.Button();
            bIOEp2Uninstall = new System.Windows.Forms.Button();
            bIOEp1Visit = new System.Windows.Forms.Button();
            bIOEp1Uninstall = new System.Windows.Forms.Button();
            lIOEp2Deco = new System.Windows.Forms.Label();
            lIOEp1Stat = new System.Windows.Forms.Label();
            lIOEp1Deco = new System.Windows.Forms.Label();
            tabModInst = new System.Windows.Forms.TabPage();
            lType = new System.Windows.Forms.Label();
            lDownloadType = new System.Windows.Forms.Label();
            lDownloadID = new System.Windows.Forms.Label();
            lModID = new System.Windows.Forms.Label();
            chSaveDownloadedArchives = new System.Windows.Forms.CheckBox();
            cbExitLaunchManager = new System.Windows.Forms.CheckBox();
            bModPath = new System.Windows.Forms.Button();
            tbModURL = new System.Windows.Forms.TextBox();
            lDownloadLink = new System.Windows.Forms.Label();
            lDownloadTrying = new System.Windows.Forms.Label();
            bModInstall = new System.Windows.Forms.Button();
            tabAbout = new System.Windows.Forms.TabPage();
            label8 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            label5 = new System.Windows.Forms.Label();
            link7z = new System.Windows.Forms.LinkLabel();
            linkMain = new System.Windows.Forms.LinkLabel();
            label6 = new System.Windows.Forms.Label();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            statusBar = new System.Windows.Forms.ToolStripStatusLabel();
            progressBar = new System.Windows.Forms.ProgressBar();
            logo = new System.Windows.Forms.PictureBox();
            tcMain.SuspendLayout();
            tabInstaller.SuspendLayout();
            tcInstallation.SuspendLayout();
            tabCurrent.SuspendLayout();
            tabOverall.SuspendLayout();
            tabModInst.SuspendLayout();
            tabAbout.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
            SuspendLayout();
            // 
            // tcMain
            // 
            tcMain.Controls.Add(tabInstaller);
            tcMain.Controls.Add(tabModInst);
            tcMain.Controls.Add(tabAbout);
            tcMain.Location = new System.Drawing.Point(16, 115);
            tcMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tcMain.Name = "tcMain";
            tcMain.Padding = new System.Drawing.Point(32, 3);
            tcMain.SelectedIndex = 0;
            tcMain.Size = new System.Drawing.Size(501, 308);
            tcMain.TabIndex = 1;
            // 
            // tabInstaller
            // 
            tabInstaller.Controls.Add(tcInstallation);
            tabInstaller.Location = new System.Drawing.Point(4, 29);
            tabInstaller.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabInstaller.Name = "tabInstaller";
            tabInstaller.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabInstaller.Size = new System.Drawing.Size(493, 275);
            tabInstaller.TabIndex = 0;
            tabInstaller.Text = "Installer";
            tabInstaller.UseVisualStyleBackColor = true;
            // 
            // tcInstallation
            // 
            tcInstallation.Controls.Add(tabCurrent);
            tcInstallation.Controls.Add(tabOverall);
            tcInstallation.Location = new System.Drawing.Point(-7, 0);
            tcInstallation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tcInstallation.Name = "tcInstallation";
            tcInstallation.Padding = new System.Drawing.Point(32, 3);
            tcInstallation.SelectedIndex = 0;
            tcInstallation.Size = new System.Drawing.Size(504, 280);
            tcInstallation.TabIndex = 15;
            // 
            // tabCurrent
            // 
            tabCurrent.Controls.Add(lGameName);
            tabCurrent.Controls.Add(lInstallGame);
            tabCurrent.Controls.Add(lInstallAdmin);
            tabCurrent.Controls.Add(bUninstall);
            tabCurrent.Controls.Add(bInstall);
            tabCurrent.Controls.Add(lInstallationStatus);
            tabCurrent.Controls.Add(lInstallStatus);
            tabCurrent.Location = new System.Drawing.Point(4, 29);
            tabCurrent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabCurrent.Name = "tabCurrent";
            tabCurrent.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabCurrent.Size = new System.Drawing.Size(496, 247);
            tabCurrent.TabIndex = 0;
            tabCurrent.Text = "Current installation";
            tabCurrent.UseVisualStyleBackColor = true;
            // 
            // lGameName
            // 
            lGameName.AutoSize = true;
            lGameName.Location = new System.Drawing.Point(147, 12);
            lGameName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lGameName.Name = "lGameName";
            lGameName.Size = new System.Drawing.Size(170, 20);
            lGameName.TabIndex = 21;
            lGameName.Text = "//Game name goes here";
            // 
            // lInstallGame
            // 
            lInstallGame.AutoSize = true;
            lInstallGame.Location = new System.Drawing.Point(84, 12);
            lInstallGame.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lInstallGame.Name = "lInstallGame";
            lInstallGame.Size = new System.Drawing.Size(55, 20);
            lInstallGame.TabIndex = 20;
            lInstallGame.Text = "Game: ";
            // 
            // lInstallAdmin
            // 
            lInstallAdmin.Location = new System.Drawing.Point(82, 68);
            lInstallAdmin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lInstallAdmin.Name = "lInstallAdmin";
            lInstallAdmin.Size = new System.Drawing.Size(341, 35);
            lInstallAdmin.TabIndex = 19;
            lInstallAdmin.Text = "Installation requires administrator privileges.";
            lInstallAdmin.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bUninstall
            // 
            bUninstall.Image = Properties.Resources.root_shield;
            bUninstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            bUninstall.Location = new System.Drawing.Point(82, 166);
            bUninstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bUninstall.Name = "bUninstall";
            bUninstall.Size = new System.Drawing.Size(341, 49);
            bUninstall.TabIndex = 18;
            bUninstall.Text = "Uninstall";
            bUninstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            bUninstall.UseVisualStyleBackColor = true;
            bUninstall.Click += bUninstall_Click;
            // 
            // bInstall
            // 
            bInstall.Image = Properties.Resources.root_shield;
            bInstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            bInstall.Location = new System.Drawing.Point(82, 108);
            bInstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bInstall.Name = "bInstall";
            bInstall.Size = new System.Drawing.Size(341, 49);
            bInstall.TabIndex = 17;
            bInstall.Text = "Install/Fix";
            bInstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            bInstall.UseVisualStyleBackColor = true;
            bInstall.Click += bInstall_Click;
            // 
            // lInstallationStatus
            // 
            lInstallationStatus.AutoSize = true;
            lInstallationStatus.Location = new System.Drawing.Point(217, 32);
            lInstallationStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lInstallationStatus.Name = "lInstallationStatus";
            lInstallationStatus.Size = new System.Drawing.Size(205, 20);
            lInstallationStatus.TabIndex = 16;
            lInstallationStatus.Text = "//Installation status goes here";
            // 
            // lInstallStatus
            // 
            lInstallStatus.AutoSize = true;
            lInstallStatus.Location = new System.Drawing.Point(84, 32);
            lInstallStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lInstallStatus.Name = "lInstallStatus";
            lInstallStatus.Size = new System.Drawing.Size(131, 20);
            lInstallStatus.TabIndex = 15;
            lInstallStatus.Text = "Installation status: ";
            // 
            // tabOverall
            // 
            tabOverall.Controls.Add(lIOEp2Path);
            tabOverall.Controls.Add(lIOEp2Stat);
            tabOverall.Controls.Add(lIOEp1Path);
            tabOverall.Controls.Add(bIOEp2Visit);
            tabOverall.Controls.Add(bIOEp2Uninstall);
            tabOverall.Controls.Add(bIOEp1Visit);
            tabOverall.Controls.Add(bIOEp1Uninstall);
            tabOverall.Controls.Add(lIOEp2Deco);
            tabOverall.Controls.Add(lIOEp1Stat);
            tabOverall.Controls.Add(lIOEp1Deco);
            tabOverall.Location = new System.Drawing.Point(4, 29);
            tabOverall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabOverall.Name = "tabOverall";
            tabOverall.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabOverall.Size = new System.Drawing.Size(496, 247);
            tabOverall.TabIndex = 1;
            tabOverall.Text = "Overall";
            tabOverall.UseVisualStyleBackColor = true;
            // 
            // lIOEp2Path
            // 
            lIOEp2Path.Location = new System.Drawing.Point(11, 172);
            lIOEp2Path.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lIOEp2Path.Name = "lIOEp2Path";
            lIOEp2Path.Size = new System.Drawing.Size(496, 40);
            lIOEp2Path.TabIndex = 18;
            lIOEp2Path.Text = "Path:";
            // 
            // lIOEp2Stat
            // 
            lIOEp2Stat.Location = new System.Drawing.Point(145, 135);
            lIOEp2Stat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lIOEp2Stat.Name = "lIOEp2Stat";
            lIOEp2Stat.Size = new System.Drawing.Size(97, 20);
            lIOEp2Stat.TabIndex = 17;
            lIOEp2Stat.Text = "Not installed";
            lIOEp2Stat.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lIOEp1Path
            // 
            lIOEp1Path.Location = new System.Drawing.Point(11, 65);
            lIOEp1Path.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lIOEp1Path.Name = "lIOEp1Path";
            lIOEp1Path.Size = new System.Drawing.Size(496, 40);
            lIOEp1Path.TabIndex = 16;
            lIOEp1Path.Text = "Path:";
            // 
            // bIOEp2Visit
            // 
            bIOEp2Visit.Location = new System.Drawing.Point(376, 121);
            bIOEp2Visit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bIOEp2Visit.Name = "bIOEp2Visit";
            bIOEp2Visit.Size = new System.Drawing.Size(112, 49);
            bIOEp2Visit.TabIndex = 15;
            bIOEp2Visit.Text = "Open location";
            bIOEp2Visit.UseVisualStyleBackColor = true;
            bIOEp2Visit.Click += bIOEp2Visit_Click;
            // 
            // bIOEp2Uninstall
            // 
            bIOEp2Uninstall.Image = Properties.Resources.root_shield;
            bIOEp2Uninstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            bIOEp2Uninstall.Location = new System.Drawing.Point(256, 121);
            bIOEp2Uninstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bIOEp2Uninstall.Name = "bIOEp2Uninstall";
            bIOEp2Uninstall.Size = new System.Drawing.Size(112, 49);
            bIOEp2Uninstall.TabIndex = 14;
            bIOEp2Uninstall.Text = "Uninstall";
            bIOEp2Uninstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            bIOEp2Uninstall.UseVisualStyleBackColor = true;
            bIOEp2Uninstall.Click += bIOEp2Uninstall_Click;
            // 
            // bIOEp1Visit
            // 
            bIOEp1Visit.Location = new System.Drawing.Point(376, 12);
            bIOEp1Visit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bIOEp1Visit.Name = "bIOEp1Visit";
            bIOEp1Visit.Size = new System.Drawing.Size(112, 49);
            bIOEp1Visit.TabIndex = 13;
            bIOEp1Visit.Text = "Open location";
            bIOEp1Visit.UseVisualStyleBackColor = true;
            bIOEp1Visit.Click += bIOEp1Visit_Click;
            // 
            // bIOEp1Uninstall
            // 
            bIOEp1Uninstall.Image = Properties.Resources.root_shield;
            bIOEp1Uninstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            bIOEp1Uninstall.Location = new System.Drawing.Point(256, 12);
            bIOEp1Uninstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bIOEp1Uninstall.Name = "bIOEp1Uninstall";
            bIOEp1Uninstall.Size = new System.Drawing.Size(112, 49);
            bIOEp1Uninstall.TabIndex = 12;
            bIOEp1Uninstall.Text = "Uninstall";
            bIOEp1Uninstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            bIOEp1Uninstall.UseVisualStyleBackColor = true;
            bIOEp1Uninstall.Click += bIOEp1Uninstall_Click;
            // 
            // lIOEp2Deco
            // 
            lIOEp2Deco.AutoSize = true;
            lIOEp2Deco.Location = new System.Drawing.Point(8, 135);
            lIOEp2Deco.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lIOEp2Deco.Name = "lIOEp2Deco";
            lIOEp2Deco.Size = new System.Drawing.Size(129, 20);
            lIOEp2Deco.TabIndex = 10;
            lIOEp2Deco.Text = "Sonic 4: Episode 2";
            // 
            // lIOEp1Stat
            // 
            lIOEp1Stat.Location = new System.Drawing.Point(148, 28);
            lIOEp1Stat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lIOEp1Stat.Name = "lIOEp1Stat";
            lIOEp1Stat.Size = new System.Drawing.Size(97, 20);
            lIOEp1Stat.TabIndex = 9;
            lIOEp1Stat.Text = "Not installed";
            lIOEp1Stat.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lIOEp1Deco
            // 
            lIOEp1Deco.AutoSize = true;
            lIOEp1Deco.Location = new System.Drawing.Point(11, 28);
            lIOEp1Deco.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lIOEp1Deco.Name = "lIOEp1Deco";
            lIOEp1Deco.Size = new System.Drawing.Size(129, 20);
            lIOEp1Deco.TabIndex = 8;
            lIOEp1Deco.Text = "Sonic 4: Episode 1";
            // 
            // tabModInst
            // 
            tabModInst.Controls.Add(lType);
            tabModInst.Controls.Add(lDownloadType);
            tabModInst.Controls.Add(lDownloadID);
            tabModInst.Controls.Add(lModID);
            tabModInst.Controls.Add(chSaveDownloadedArchives);
            tabModInst.Controls.Add(cbExitLaunchManager);
            tabModInst.Controls.Add(bModPath);
            tabModInst.Controls.Add(tbModURL);
            tabModInst.Controls.Add(lDownloadLink);
            tabModInst.Controls.Add(lDownloadTrying);
            tabModInst.Controls.Add(bModInstall);
            tabModInst.Location = new System.Drawing.Point(4, 29);
            tabModInst.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabModInst.Name = "tabModInst";
            tabModInst.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabModInst.Size = new System.Drawing.Size(493, 275);
            tabModInst.TabIndex = 1;
            tabModInst.Text = "Install mod";
            tabModInst.UseVisualStyleBackColor = true;
            // 
            // lType
            // 
            lType.AutoSize = true;
            lType.Location = new System.Drawing.Point(257, 74);
            lType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lType.Name = "lType";
            lType.Size = new System.Drawing.Size(30, 20);
            lType.TabIndex = 18;
            lType.Text = "???";
            // 
            // lDownloadType
            // 
            lDownloadType.AutoSize = true;
            lDownloadType.Location = new System.Drawing.Point(239, 54);
            lDownloadType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lDownloadType.Name = "lDownloadType";
            lDownloadType.Size = new System.Drawing.Size(76, 20);
            lDownloadType.TabIndex = 17;
            lDownloadType.Text = "Mod type:";
            // 
            // lDownloadID
            // 
            lDownloadID.AutoSize = true;
            lDownloadID.Location = new System.Drawing.Point(344, 54);
            lDownloadID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lDownloadID.Name = "lDownloadID";
            lDownloadID.Size = new System.Drawing.Size(62, 20);
            lDownloadID.TabIndex = 13;
            lDownloadID.Text = "Mod ID:";
            // 
            // lModID
            // 
            lModID.AutoSize = true;
            lModID.Location = new System.Drawing.Point(363, 74);
            lModID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lModID.Name = "lModID";
            lModID.Size = new System.Drawing.Size(30, 20);
            lModID.TabIndex = 12;
            lModID.Text = "???";
            // 
            // chSaveDownloadedArchives
            // 
            chSaveDownloadedArchives.AutoSize = true;
            chSaveDownloadedArchives.Location = new System.Drawing.Point(12, 177);
            chSaveDownloadedArchives.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chSaveDownloadedArchives.Name = "chSaveDownloadedArchives";
            chSaveDownloadedArchives.Size = new System.Drawing.Size(207, 24);
            chSaveDownloadedArchives.TabIndex = 22;
            chSaveDownloadedArchives.Text = "Save downloaded archives";
            chSaveDownloadedArchives.UseVisualStyleBackColor = true;
            chSaveDownloadedArchives.CheckedChanged += fake_SettingsSave;
            // 
            // cbExitLaunchManager
            // 
            cbExitLaunchManager.AutoSize = true;
            cbExitLaunchManager.Location = new System.Drawing.Point(12, 212);
            cbExitLaunchManager.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cbExitLaunchManager.Name = "cbExitLaunchManager";
            cbExitLaunchManager.Size = new System.Drawing.Size(254, 44);
            cbExitLaunchManager.TabIndex = 21;
            cbExitLaunchManager.Text = "Launch Mod Manager (if PC mod)\r\nand Exit after installation";
            cbExitLaunchManager.UseVisualStyleBackColor = true;
            cbExitLaunchManager.CheckedChanged += fake_SettingsSave;
            // 
            // bModPath
            // 
            bModPath.Location = new System.Drawing.Point(440, 98);
            bModPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bModPath.Name = "bModPath";
            bModPath.Size = new System.Drawing.Size(43, 35);
            bModPath.TabIndex = 20;
            bModPath.Text = "...";
            bModPath.UseVisualStyleBackColor = true;
            bModPath.Click += bModPath_Click;
            // 
            // tbModURL
            // 
            tbModURL.Location = new System.Drawing.Point(12, 98);
            tbModURL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tbModURL.MaxLength = 512;
            tbModURL.Name = "tbModURL";
            tbModURL.Size = new System.Drawing.Size(420, 27);
            tbModURL.TabIndex = 19;
            tbModURL.TextChanged += tbModURL_TextChanged;
            // 
            // lDownloadLink
            // 
            lDownloadLink.AutoSize = true;
            lDownloadLink.Location = new System.Drawing.Point(8, 74);
            lDownloadLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lDownloadLink.Name = "lDownloadLink";
            lDownloadLink.Size = new System.Drawing.Size(118, 20);
            lDownloadLink.TabIndex = 15;
            lDownloadLink.Text = "Path to the mod:";
            // 
            // lDownloadTrying
            // 
            lDownloadTrying.AutoSize = true;
            lDownloadTrying.Location = new System.Drawing.Point(8, 20);
            lDownloadTrying.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lDownloadTrying.Name = "lDownloadTrying";
            lDownloadTrying.Size = new System.Drawing.Size(193, 20);
            lDownloadTrying.TabIndex = 14;
            lDownloadTrying.Text = "Enter a path or URL to mod.";
            // 
            // bModInstall
            // 
            bModInstall.Enabled = false;
            bModInstall.Location = new System.Drawing.Point(312, 192);
            bModInstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bModInstall.Name = "bModInstall";
            bModInstall.Size = new System.Drawing.Size(171, 49);
            bModInstall.TabIndex = 11;
            bModInstall.Text = "Install";
            bModInstall.UseVisualStyleBackColor = true;
            bModInstall.Click += bModInstall_Click;
            // 
            // tabAbout
            // 
            tabAbout.Controls.Add(label8);
            tabAbout.Controls.Add(label7);
            tabAbout.Controls.Add(linkLabel1);
            tabAbout.Controls.Add(label5);
            tabAbout.Controls.Add(link7z);
            tabAbout.Controls.Add(linkMain);
            tabAbout.Controls.Add(label6);
            tabAbout.Location = new System.Drawing.Point(4, 29);
            tabAbout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabAbout.Name = "tabAbout";
            tabAbout.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabAbout.Size = new System.Drawing.Size(493, 275);
            tabAbout.TabIndex = 3;
            tabAbout.Text = "About";
            tabAbout.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            label8.Location = new System.Drawing.Point(76, 132);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(349, 46);
            label8.TabIndex = 13;
            label8.Text = "Read licenses in \"Mod Loader - licenses\" folder.";
            label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label7.Location = new System.Drawing.Point(-4, 188);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(509, 40);
            label7.TabIndex = 11;
            label7.Text = "Special thanks to the Tango Desktop Project developers for their icons dedicated to the Public Domain";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new System.Drawing.Point(151, 228);
            linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(198, 20);
            linkLabel1.TabIndex = 12;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "http://tango.freedesktop.org";
            linkLabel1.LinkClicked += linkLabel_LinkClicked;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(143, 75);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(214, 20);
            label5.TabIndex = 9;
            label5.Text = "7-Zip Copyright (C) Igor Pavlov";
            // 
            // link7z
            // 
            link7z.AutoSize = true;
            link7z.Location = new System.Drawing.Point(171, 95);
            link7z.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            link7z.Name = "link7z";
            link7z.Size = new System.Drawing.Size(159, 20);
            link7z.TabIndex = 10;
            link7z.TabStop = true;
            link7z.Text = "https://www.7-zip.org/";
            link7z.LinkClicked += linkLabel_LinkClicked;
            // 
            // linkMain
            // 
            linkMain.AutoSize = true;
            linkMain.Location = new System.Drawing.Point(88, 42);
            linkMain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkMain.Name = "linkMain";
            linkMain.Size = new System.Drawing.Size(324, 20);
            linkMain.TabIndex = 8;
            linkMain.TabStop = true;
            linkMain.Text = "https://github.com/OSA413/Sonic4_ModLoader";
            linkMain.LinkClicked += linkLabel_LinkClicked;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(56, 22);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(389, 20);
            label6.TabIndex = 7;
            label6.Text = "One-Click Mod Installer by OSA413 under the MIT License";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { statusBar });
            statusStrip1.Location = new System.Drawing.Point(0, 463);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            statusStrip1.Size = new System.Drawing.Size(530, 26);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // statusBar
            // 
            statusBar.Name = "statusBar";
            statusBar.Size = new System.Drawing.Size(129, 20);
            statusBar.Text = "This is a status bar";
            // 
            // progressBar
            // 
            progressBar.Location = new System.Drawing.Point(16, 433);
            progressBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            progressBar.MarqueeAnimationSpeed = 25;
            progressBar.Maximum = 1000;
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(501, 25);
            progressBar.Step = 1;
            progressBar.TabIndex = 10;
            // 
            // logo
            // 
            logo.Image = Properties.Resources.ocmi_logo;
            logo.Location = new System.Drawing.Point(16, 14);
            logo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            logo.Name = "logo";
            logo.Size = new System.Drawing.Size(501, 91);
            logo.TabIndex = 0;
            logo.TabStop = false;
            // 
            // UltimateWinForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(530, 489);
            Controls.Add(statusStrip1);
            Controls.Add(tcMain);
            Controls.Add(logo);
            Controls.Add(progressBar);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "UltimateWinForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "One-Click Mod Installer";
            tcMain.ResumeLayout(false);
            tabInstaller.ResumeLayout(false);
            tcInstallation.ResumeLayout(false);
            tabCurrent.ResumeLayout(false);
            tabCurrent.PerformLayout();
            tabOverall.ResumeLayout(false);
            tabOverall.PerformLayout();
            tabModInst.ResumeLayout(false);
            tabModInst.PerformLayout();
            tabAbout.ResumeLayout(false);
            tabAbout.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)logo).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tabInstaller;
        private System.Windows.Forms.TabPage tabModInst;
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
        private System.Windows.Forms.Button bModPath;
        private System.Windows.Forms.TextBox tbModURL;
        private System.Windows.Forms.CheckBox chSaveDownloadedArchives;
        private System.Windows.Forms.CheckBox cbExitLaunchManager;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel link7z;
        private System.Windows.Forms.LinkLabel linkMain;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label8;
    }
}