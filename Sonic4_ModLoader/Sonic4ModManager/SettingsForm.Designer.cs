﻿namespace Sonic4ModManager
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.linkSAT = new System.Windows.Forms.LinkLabel();
            this.linkMain = new System.Windows.Forms.LinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabInstallation = new System.Windows.Forms.TabPage();
            this.cbKeepConfigs = new System.Windows.Forms.CheckBox();
            this.cb_ForceUninstall = new System.Windows.Forms.CheckBox();
            this.cb_Uninstall_OCMI = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_recover_orig = new System.Windows.Forms.CheckBox();
            this.rb_delete = new System.Windows.Forms.RadioButton();
            this.rb_rename = new System.Windows.Forms.RadioButton();
            this.bInstall = new System.Windows.Forms.Button();
            this.label_Installation_status = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabProgram = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.num_CsbEditor_BufferSize = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.num_CsbEditor_MaxThreads = new System.Windows.Forms.NumericUpDown();
            this.cb_CsbEditor_EnableThreading = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_AMBPatcher_progress_bar = new System.Windows.Forms.CheckBox();
            this.bRecoverOriginalFiles = new System.Windows.Forms.Button();
            this.cb_AMBPatcher_sha_check = new System.Windows.Forms.CheckBox();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.bRL_SAT = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.bRL_7z = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.link7z = new System.Windows.Forms.LinkLabel();
            this.bRL_AML = new System.Windows.Forms.Button();
            this.bRL_S4ML = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabInstallation.SuspendLayout();
            this.tabProgram.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_CsbEditor_BufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_CsbEditor_MaxThreads)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(412, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mod Manager and AMBPather by OSA413 under MIT License.";
            // 
            // linkSAT
            // 
            this.linkSAT.AutoSize = true;
            this.linkSAT.Location = new System.Drawing.Point(29, 95);
            this.linkSAT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkSAT.Name = "linkSAT";
            this.linkSAT.Size = new System.Drawing.Size(338, 20);
            this.linkSAT.TabIndex = 3;
            this.linkSAT.TabStop = true;
            this.linkSAT.Text = "https://github.com/RadiantDerg/AliceModLoader";
            this.linkSAT.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
            // 
            // linkMain
            // 
            this.linkMain.AutoSize = true;
            this.linkMain.Location = new System.Drawing.Point(29, 29);
            this.linkMain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkMain.Name = "linkMain";
            this.linkMain.Size = new System.Drawing.Size(324, 20);
            this.linkMain.TabIndex = 1;
            this.linkMain.TabStop = true;
            this.linkMain.Text = "https://github.com/OSA413/Sonic4_ModLoader";
            this.linkMain.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabInstallation);
            this.tabControl1.Controls.Add(this.tabProgram);
            this.tabControl1.Controls.Add(this.tabAbout);
            this.tabControl1.ItemSize = new System.Drawing.Size(90, 25);
            this.tabControl1.Location = new System.Drawing.Point(16, 19);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(32, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(488, 335);
            this.tabControl1.TabIndex = 0;
            // 
            // tabInstallation
            // 
            this.tabInstallation.AutoScroll = true;
            this.tabInstallation.AutoScrollMargin = new System.Drawing.Size(0, 8);
            this.tabInstallation.Controls.Add(this.cbKeepConfigs);
            this.tabInstallation.Controls.Add(this.cb_ForceUninstall);
            this.tabInstallation.Controls.Add(this.cb_Uninstall_OCMI);
            this.tabInstallation.Controls.Add(this.label5);
            this.tabInstallation.Controls.Add(this.cb_recover_orig);
            this.tabInstallation.Controls.Add(this.rb_delete);
            this.tabInstallation.Controls.Add(this.rb_rename);
            this.tabInstallation.Controls.Add(this.bInstall);
            this.tabInstallation.Controls.Add(this.label_Installation_status);
            this.tabInstallation.Controls.Add(this.label3);
            this.tabInstallation.Location = new System.Drawing.Point(4, 29);
            this.tabInstallation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabInstallation.Name = "tabInstallation";
            this.tabInstallation.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabInstallation.Size = new System.Drawing.Size(480, 302);
            this.tabInstallation.TabIndex = 0;
            this.tabInstallation.Text = "Installation";
            this.tabInstallation.UseVisualStyleBackColor = true;
            // 
            // cbKeepConfigs
            // 
            this.cbKeepConfigs.AutoSize = true;
            this.cbKeepConfigs.Location = new System.Drawing.Point(296, 195);
            this.cbKeepConfigs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbKeepConfigs.Name = "cbKeepConfigs";
            this.cbKeepConfigs.Size = new System.Drawing.Size(117, 24);
            this.cbKeepConfigs.TabIndex = 10;
            this.cbKeepConfigs.Text = "Keep configs";
            this.cbKeepConfigs.UseVisualStyleBackColor = true;
            // 
            // cb_ForceUninstall
            // 
            this.cb_ForceUninstall.AutoSize = true;
            this.cb_ForceUninstall.Location = new System.Drawing.Point(181, 104);
            this.cb_ForceUninstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_ForceUninstall.Name = "cb_ForceUninstall";
            this.cb_ForceUninstall.Size = new System.Drawing.Size(126, 24);
            this.cb_ForceUninstall.TabIndex = 9;
            this.cb_ForceUninstall.Text = "Force uninstall";
            this.cb_ForceUninstall.UseVisualStyleBackColor = true;
            this.cb_ForceUninstall.CheckedChanged += new System.EventHandler(this.cb_ForceUninstall_CheckedChanged);
            // 
            // cb_Uninstall_OCMI
            // 
            this.cb_Uninstall_OCMI.AutoSize = true;
            this.cb_Uninstall_OCMI.Location = new System.Drawing.Point(112, 230);
            this.cb_Uninstall_OCMI.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_Uninstall_OCMI.Name = "cb_Uninstall_OCMI";
            this.cb_Uninstall_OCMI.Size = new System.Drawing.Size(322, 24);
            this.cb_Uninstall_OCMI.TabIndex = 7;
            this.cb_Uninstall_OCMI.Text = "Uninstall and delete One-Click Mod Installer";
            this.cb_Uninstall_OCMI.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(112, 135);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Uninstallation options:";
            // 
            // cb_recover_orig
            // 
            this.cb_recover_orig.AutoSize = true;
            this.cb_recover_orig.Location = new System.Drawing.Point(91, 266);
            this.cb_recover_orig.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_recover_orig.Name = "cb_recover_orig";
            this.cb_recover_orig.Size = new System.Drawing.Size(312, 24);
            this.cb_recover_orig.TabIndex = 4;
            this.cb_recover_orig.Text = "Recover original game files (AMBs, CSBs...)";
            this.cb_recover_orig.UseVisualStyleBackColor = true;
            // 
            // rb_delete
            // 
            this.rb_delete.AutoSize = true;
            this.rb_delete.Location = new System.Drawing.Point(91, 195);
            this.rb_delete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rb_delete.Name = "rb_delete";
            this.rb_delete.Size = new System.Drawing.Size(210, 24);
            this.rb_delete.TabIndex = 3;
            this.rb_delete.TabStop = true;
            this.rb_delete.Text = "Delete all Mod Loader files";
            this.rb_delete.UseVisualStyleBackColor = true;
            this.rb_delete.CheckedChanged += new System.EventHandler(this.rb_delete_CheckedChanged);
            // 
            // rb_rename
            // 
            this.rb_rename.AutoSize = true;
            this.rb_rename.Location = new System.Drawing.Point(91, 160);
            this.rb_rename.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rb_rename.Name = "rb_rename";
            this.rb_rename.Size = new System.Drawing.Size(150, 24);
            this.rb_rename.TabIndex = 2;
            this.rb_rename.TabStop = true;
            this.rb_rename.Text = "Rename files back";
            this.rb_rename.UseVisualStyleBackColor = true;
            // 
            // bInstall
            // 
            this.bInstall.Location = new System.Drawing.Point(159, 46);
            this.bInstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bInstall.Name = "bInstall";
            this.bInstall.Size = new System.Drawing.Size(171, 49);
            this.bInstall.TabIndex = 1;
            this.bInstall.Text = "(Un)install";
            this.bInstall.UseVisualStyleBackColor = true;
            this.bInstall.Click += new System.EventHandler(this.bInstall_Click);
            // 
            // label_Installation_status
            // 
            this.label_Installation_status.AutoSize = true;
            this.label_Installation_status.Location = new System.Drawing.Point(137, 12);
            this.label_Installation_status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Installation_status.Name = "label_Installation_status";
            this.label_Installation_status.Size = new System.Drawing.Size(205, 20);
            this.label_Installation_status.TabIndex = 1;
            this.label_Installation_status.Text = "//Installation status goes here";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 12);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Installation status:";
            // 
            // tabProgram
            // 
            this.tabProgram.Controls.Add(this.groupBox2);
            this.tabProgram.Controls.Add(this.groupBox1);
            this.tabProgram.Location = new System.Drawing.Point(4, 29);
            this.tabProgram.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabProgram.Name = "tabProgram";
            this.tabProgram.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabProgram.Size = new System.Drawing.Size(480, 302);
            this.tabProgram.TabIndex = 1;
            this.tabProgram.Text = "Programs";
            this.tabProgram.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.num_CsbEditor_BufferSize);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.num_CsbEditor_MaxThreads);
            this.groupBox2.Controls.Add(this.cb_CsbEditor_EnableThreading);
            this.groupBox2.Location = new System.Drawing.Point(243, 9);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(230, 285);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CSB Editor";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 76);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 20);
            this.label7.TabIndex = 9;
            this.label7.Text = "Buffer Size";
            // 
            // num_CsbEditor_BufferSize
            // 
            this.num_CsbEditor_BufferSize.Location = new System.Drawing.Point(134, 72);
            this.num_CsbEditor_BufferSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.num_CsbEditor_BufferSize.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.num_CsbEditor_BufferSize.Name = "num_CsbEditor_BufferSize";
            this.num_CsbEditor_BufferSize.Size = new System.Drawing.Size(89, 27);
            this.num_CsbEditor_BufferSize.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 149);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 20);
            this.label6.TabIndex = 7;
            this.label6.Text = "Max Threads";
            // 
            // num_CsbEditor_MaxThreads
            // 
            this.num_CsbEditor_MaxThreads.Enabled = false;
            this.num_CsbEditor_MaxThreads.Location = new System.Drawing.Point(134, 145);
            this.num_CsbEditor_MaxThreads.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.num_CsbEditor_MaxThreads.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.num_CsbEditor_MaxThreads.Name = "num_CsbEditor_MaxThreads";
            this.num_CsbEditor_MaxThreads.Size = new System.Drawing.Size(89, 27);
            this.num_CsbEditor_MaxThreads.TabIndex = 6;
            // 
            // cb_CsbEditor_EnableThreading
            // 
            this.cb_CsbEditor_EnableThreading.AutoSize = true;
            this.cb_CsbEditor_EnableThreading.Location = new System.Drawing.Point(7, 112);
            this.cb_CsbEditor_EnableThreading.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_CsbEditor_EnableThreading.Name = "cb_CsbEditor_EnableThreading";
            this.cb_CsbEditor_EnableThreading.Size = new System.Drawing.Size(147, 24);
            this.cb_CsbEditor_EnableThreading.TabIndex = 5;
            this.cb_CsbEditor_EnableThreading.Text = "Enable Threading";
            this.cb_CsbEditor_EnableThreading.UseVisualStyleBackColor = true;
            this.cb_CsbEditor_EnableThreading.CheckedChanged += new System.EventHandler(this.cb_CsbEditor_EnableThreading_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_AMBPatcher_progress_bar);
            this.groupBox1.Controls.Add(this.bRecoverOriginalFiles);
            this.groupBox1.Controls.Add(this.cb_AMBPatcher_sha_check);
            this.groupBox1.Location = new System.Drawing.Point(7, 9);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(230, 285);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AMB Patcher";
            // 
            // cb_AMBPatcher_progress_bar
            // 
            this.cb_AMBPatcher_progress_bar.AutoSize = true;
            this.cb_AMBPatcher_progress_bar.Location = new System.Drawing.Point(48, 76);
            this.cb_AMBPatcher_progress_bar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_AMBPatcher_progress_bar.Name = "cb_AMBPatcher_progress_bar";
            this.cb_AMBPatcher_progress_bar.Size = new System.Drawing.Size(113, 24);
            this.cb_AMBPatcher_progress_bar.TabIndex = 1;
            this.cb_AMBPatcher_progress_bar.Text = "Progress Bar";
            this.cb_AMBPatcher_progress_bar.UseVisualStyleBackColor = true;
            // 
            // bRecoverOriginalFiles
            // 
            this.bRecoverOriginalFiles.Location = new System.Drawing.Point(34, 228);
            this.bRecoverOriginalFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bRecoverOriginalFiles.Name = "bRecoverOriginalFiles";
            this.bRecoverOriginalFiles.Size = new System.Drawing.Size(171, 49);
            this.bRecoverOriginalFiles.TabIndex = 8;
            this.bRecoverOriginalFiles.Text = "Recover original files";
            this.bRecoverOriginalFiles.UseVisualStyleBackColor = true;
            this.bRecoverOriginalFiles.Click += new System.EventHandler(this.bRecoverOriginalFiles_Click);
            // 
            // cb_AMBPatcher_sha_check
            // 
            this.cb_AMBPatcher_sha_check.AutoSize = true;
            this.cb_AMBPatcher_sha_check.Location = new System.Drawing.Point(48, 112);
            this.cb_AMBPatcher_sha_check.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_AMBPatcher_sha_check.Name = "cb_AMBPatcher_sha_check";
            this.cb_AMBPatcher_sha_check.Size = new System.Drawing.Size(152, 44);
            this.cb_AMBPatcher_sha_check.TabIndex = 3;
            this.cb_AMBPatcher_sha_check.Text = "Check SHA of files\r\n(recommended)";
            this.cb_AMBPatcher_sha_check.UseVisualStyleBackColor = true;
            // 
            // tabAbout
            // 
            this.tabAbout.Controls.Add(this.bRL_SAT);
            this.tabAbout.Controls.Add(this.label8);
            this.tabAbout.Controls.Add(this.linkLabel1);
            this.tabAbout.Controls.Add(this.bRL_7z);
            this.tabAbout.Controls.Add(this.label4);
            this.tabAbout.Controls.Add(this.link7z);
            this.tabAbout.Controls.Add(this.bRL_AML);
            this.tabAbout.Controls.Add(this.bRL_S4ML);
            this.tabAbout.Controls.Add(this.label2);
            this.tabAbout.Controls.Add(this.linkMain);
            this.tabAbout.Controls.Add(this.label1);
            this.tabAbout.Controls.Add(this.linkSAT);
            this.tabAbout.Location = new System.Drawing.Point(4, 29);
            this.tabAbout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabAbout.Size = new System.Drawing.Size(480, 302);
            this.tabAbout.TabIndex = 2;
            this.tabAbout.Text = "About";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // bRL_SAT
            // 
            this.bRL_SAT.Location = new System.Drawing.Point(348, 205);
            this.bRL_SAT.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bRL_SAT.Name = "bRL_SAT";
            this.bRL_SAT.Size = new System.Drawing.Size(121, 35);
            this.bRL_SAT.TabIndex = 10;
            this.bRL_SAT.Text = "Read License";
            this.bRL_SAT.UseVisualStyleBackColor = true;
            this.bRL_SAT.Click += new System.EventHandler(this.ReadLicense_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 160);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(419, 20);
            this.label8.TabIndex = 8;
            this.label8.Text = "CsbEditor (from SonicAudioTools) by Skyth under MIT License.";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(29, 180);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(389, 20);
            this.linkLabel1.TabIndex = 9;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://github.com/blueskythlikesclouds/SonicAudioTools";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
            // 
            // bRL_7z
            // 
            this.bRL_7z.Location = new System.Drawing.Point(196, 256);
            this.bRL_7z.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bRL_7z.Name = "bRL_7z";
            this.bRL_7z.Size = new System.Drawing.Size(121, 35);
            this.bRL_7z.TabIndex = 7;
            this.bRL_7z.Text = "Read License";
            this.bRL_7z.UseVisualStyleBackColor = true;
            this.bRL_7z.Click += new System.EventHandler(this.ReadLicense_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 231);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(214, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "7-Zip Copyright (C) Igor Pavlov";
            // 
            // link7z
            // 
            this.link7z.AutoSize = true;
            this.link7z.Location = new System.Drawing.Point(29, 251);
            this.link7z.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.link7z.Name = "link7z";
            this.link7z.Size = new System.Drawing.Size(159, 20);
            this.link7z.TabIndex = 6;
            this.link7z.TabStop = true;
            this.link7z.Text = "https://www.7-zip.org/";
            this.link7z.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
            // 
            // bRL_AML
            // 
            this.bRL_AML.Location = new System.Drawing.Point(348, 120);
            this.bRL_AML.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bRL_AML.Name = "bRL_AML";
            this.bRL_AML.Size = new System.Drawing.Size(121, 35);
            this.bRL_AML.TabIndex = 4;
            this.bRL_AML.Text = "Read License";
            this.bRL_AML.UseVisualStyleBackColor = true;
            this.bRL_AML.Click += new System.EventHandler(this.ReadLicense_Click);
            // 
            // bRL_S4ML
            // 
            this.bRL_S4ML.Location = new System.Drawing.Point(348, 35);
            this.bRL_S4ML.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bRL_S4ML.Name = "bRL_S4ML";
            this.bRL_S4ML.Size = new System.Drawing.Size(121, 35);
            this.bRL_S4ML.TabIndex = 2;
            this.bRL_S4ML.Text = "Read License";
            this.bRL_S4ML.UseVisualStyleBackColor = true;
            this.bRL_S4ML.Click += new System.EventHandler(this.ReadLicense_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(386, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Alice Mod Loader by RadiantDerg under the MIT License.";
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(16, 362);
            this.bOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(240, 59);
            this.bOK.TabIndex = 98;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(264, 362);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(240, 59);
            this.bCancel.TabIndex = 99;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 440);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.tabControl1.ResumeLayout(false);
            this.tabInstallation.ResumeLayout(false);
            this.tabInstallation.PerformLayout();
            this.tabProgram.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_CsbEditor_BufferSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_CsbEditor_MaxThreads)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabAbout.ResumeLayout(false);
            this.tabAbout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkSAT;
        private System.Windows.Forms.LinkLabel linkMain;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInstallation;
        private System.Windows.Forms.TabPage tabProgram;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.Button bRL_AML;
        private System.Windows.Forms.Button bRL_S4ML;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cb_AMBPatcher_progress_bar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bInstall;
        private System.Windows.Forms.Label label_Installation_status;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cb_recover_orig;
        private System.Windows.Forms.RadioButton rb_delete;
        private System.Windows.Forms.RadioButton rb_rename;
        private System.Windows.Forms.CheckBox cb_AMBPatcher_sha_check;
        private System.Windows.Forms.Button bRL_7z;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel link7z;
        private System.Windows.Forms.Button bRecoverOriginalFiles;
        private System.Windows.Forms.CheckBox cb_Uninstall_OCMI;
        private System.Windows.Forms.CheckBox cb_ForceUninstall;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown num_CsbEditor_BufferSize;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown num_CsbEditor_MaxThreads;
        private System.Windows.Forms.CheckBox cb_CsbEditor_EnableThreading;
        private System.Windows.Forms.CheckBox cbKeepConfigs;
        private System.Windows.Forms.Button bRL_SAT;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}