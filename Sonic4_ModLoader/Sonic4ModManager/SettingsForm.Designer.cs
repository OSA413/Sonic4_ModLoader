namespace Sonic4ModManager
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
            this.label5 = new System.Windows.Forms.Label();
            this.cb_recover_orig = new System.Windows.Forms.CheckBox();
            this.rb_delete = new System.Windows.Forms.RadioButton();
            this.rb_rename = new System.Windows.Forms.RadioButton();
            this.bInstall = new System.Windows.Forms.Button();
            this.label_Installation_status = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabAMBPatcher = new System.Windows.Forms.TabPage();
            this.list_SHAType = new System.Windows.Forms.ComboBox();
            this.cb_AMBPatcher_sha_check = new System.Windows.Forms.CheckBox();
            this.cb_AMBPatcher_generate_log = new System.Windows.Forms.CheckBox();
            this.cb_AMBPatcher_progress_bar = new System.Windows.Forms.CheckBox();
            this.tabCsbEditor = new System.Windows.Forms.TabPage();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.bRL_7z = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.link7z = new System.Windows.Forms.LinkLabel();
            this.bRL_SAT = new System.Windows.Forms.Button();
            this.bRL_S4ML = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.cb_CsbEditor_EnableThreading = new System.Windows.Forms.CheckBox();
            this.num_CsbEditor_MaxThreads = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.num_CsbEditor_BufferSize = new System.Windows.Forms.NumericUpDown();
            this.tabControl1.SuspendLayout();
            this.tabInstallation.SuspendLayout();
            this.tabAMBPatcher.SuspendLayout();
            this.tabCsbEditor.SuspendLayout();
            this.tabAbout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_CsbEditor_MaxThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_CsbEditor_BufferSize)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mod Manager and AMBPather by OSA413 under MIT License.";
            // 
            // linkSAT
            // 
            this.linkSAT.AutoSize = true;
            this.linkSAT.Location = new System.Drawing.Point(22, 83);
            this.linkSAT.Name = "linkSAT";
            this.linkSAT.Size = new System.Drawing.Size(282, 13);
            this.linkSAT.TabIndex = 3;
            this.linkSAT.TabStop = true;
            this.linkSAT.Text = "https://github.com/blueskythlikesclouds/SonicAudioTools";
            this.linkSAT.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
            // 
            // linkMain
            // 
            this.linkMain.AutoSize = true;
            this.linkMain.Location = new System.Drawing.Point(22, 19);
            this.linkMain.Name = "linkMain";
            this.linkMain.Size = new System.Drawing.Size(238, 13);
            this.linkMain.TabIndex = 1;
            this.linkMain.TabStop = true;
            this.linkMain.Text = "https://github.com/OSA413/Sonic4_ModLoader";
            this.linkMain.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabInstallation);
            this.tabControl1.Controls.Add(this.tabAMBPatcher);
            this.tabControl1.Controls.Add(this.tabCsbEditor);
            this.tabControl1.Controls.Add(this.tabAbout);
            this.tabControl1.ItemSize = new System.Drawing.Size(90, 18);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(366, 218);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // tabInstallation
            // 
            this.tabInstallation.Controls.Add(this.label5);
            this.tabInstallation.Controls.Add(this.cb_recover_orig);
            this.tabInstallation.Controls.Add(this.rb_delete);
            this.tabInstallation.Controls.Add(this.rb_rename);
            this.tabInstallation.Controls.Add(this.bInstall);
            this.tabInstallation.Controls.Add(this.label_Installation_status);
            this.tabInstallation.Controls.Add(this.label3);
            this.tabInstallation.Location = new System.Drawing.Point(4, 22);
            this.tabInstallation.Name = "tabInstallation";
            this.tabInstallation.Padding = new System.Windows.Forms.Padding(3);
            this.tabInstallation.Size = new System.Drawing.Size(358, 192);
            this.tabInstallation.TabIndex = 0;
            this.tabInstallation.Text = "Installation";
            this.tabInstallation.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(80, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Uninstallation options:";
            // 
            // cb_recover_orig
            // 
            this.cb_recover_orig.AutoSize = true;
            this.cb_recover_orig.Location = new System.Drawing.Point(64, 152);
            this.cb_recover_orig.Name = "cb_recover_orig";
            this.cb_recover_orig.Size = new System.Drawing.Size(231, 17);
            this.cb_recover_orig.TabIndex = 4;
            this.cb_recover_orig.Text = "Recover original game files (AMBs, CSBs...)";
            this.cb_recover_orig.UseVisualStyleBackColor = true;
            // 
            // rb_delete
            // 
            this.rb_delete.AutoSize = true;
            this.rb_delete.Location = new System.Drawing.Point(64, 129);
            this.rb_delete.Name = "rb_delete";
            this.rb_delete.Size = new System.Drawing.Size(150, 17);
            this.rb_delete.TabIndex = 3;
            this.rb_delete.TabStop = true;
            this.rb_delete.Text = "Delete all Mod Loader files";
            this.rb_delete.UseVisualStyleBackColor = true;
            // 
            // rb_rename
            // 
            this.rb_rename.AutoSize = true;
            this.rb_rename.Location = new System.Drawing.Point(64, 106);
            this.rb_rename.Name = "rb_rename";
            this.rb_rename.Size = new System.Drawing.Size(113, 17);
            this.rb_rename.TabIndex = 2;
            this.rb_rename.TabStop = true;
            this.rb_rename.Text = "Rename files back";
            this.rb_rename.UseVisualStyleBackColor = true;
            // 
            // bInstall
            // 
            this.bInstall.Location = new System.Drawing.Point(115, 55);
            this.bInstall.Name = "bInstall";
            this.bInstall.Size = new System.Drawing.Size(128, 32);
            this.bInstall.TabIndex = 1;
            this.bInstall.Text = "(Un)install";
            this.bInstall.UseVisualStyleBackColor = true;
            this.bInstall.Click += new System.EventHandler(this.bInstall_Click);
            // 
            // label_Installation_status
            // 
            this.label_Installation_status.AutoSize = true;
            this.label_Installation_status.Location = new System.Drawing.Point(103, 30);
            this.label_Installation_status.Name = "label_Installation_status";
            this.label_Installation_status.Size = new System.Drawing.Size(148, 13);
            this.label_Installation_status.TabIndex = 1;
            this.label_Installation_status.Text = "//Installation status goes here";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Installation status:";
            // 
            // tabAMBPatcher
            // 
            this.tabAMBPatcher.Controls.Add(this.list_SHAType);
            this.tabAMBPatcher.Controls.Add(this.cb_AMBPatcher_sha_check);
            this.tabAMBPatcher.Controls.Add(this.cb_AMBPatcher_generate_log);
            this.tabAMBPatcher.Controls.Add(this.cb_AMBPatcher_progress_bar);
            this.tabAMBPatcher.Location = new System.Drawing.Point(4, 22);
            this.tabAMBPatcher.Name = "tabAMBPatcher";
            this.tabAMBPatcher.Padding = new System.Windows.Forms.Padding(3);
            this.tabAMBPatcher.Size = new System.Drawing.Size(358, 192);
            this.tabAMBPatcher.TabIndex = 1;
            this.tabAMBPatcher.Text = "AMBPatcher";
            this.tabAMBPatcher.UseVisualStyleBackColor = true;
            // 
            // list_SHAType
            // 
            this.list_SHAType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.list_SHAType.Enabled = false;
            this.list_SHAType.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.list_SHAType.Items.AddRange(new object[] {
            "1",
            "256",
            "384",
            "512"});
            this.list_SHAType.Location = new System.Drawing.Point(150, 77);
            this.list_SHAType.Name = "list_SHAType";
            this.list_SHAType.Size = new System.Drawing.Size(47, 21);
            this.list_SHAType.TabIndex = 4;
            // 
            // cb_AMBPatcher_sha_check
            // 
            this.cb_AMBPatcher_sha_check.AutoSize = true;
            this.cb_AMBPatcher_sha_check.Location = new System.Drawing.Point(6, 54);
            this.cb_AMBPatcher_sha_check.Name = "cb_AMBPatcher_sha_check";
            this.cb_AMBPatcher_sha_check.Size = new System.Drawing.Size(191, 17);
            this.cb_AMBPatcher_sha_check.TabIndex = 3;
            this.cb_AMBPatcher_sha_check.Text = "Check SHA of files (recommended)";
            this.cb_AMBPatcher_sha_check.UseVisualStyleBackColor = true;
            this.cb_AMBPatcher_sha_check.CheckedChanged += new System.EventHandler(this.cb_AMBPatcher_sha_check_CheckedChanged);
            // 
            // cb_AMBPatcher_generate_log
            // 
            this.cb_AMBPatcher_generate_log.AutoSize = true;
            this.cb_AMBPatcher_generate_log.Location = new System.Drawing.Point(6, 31);
            this.cb_AMBPatcher_generate_log.Name = "cb_AMBPatcher_generate_log";
            this.cb_AMBPatcher_generate_log.Size = new System.Drawing.Size(144, 17);
            this.cb_AMBPatcher_generate_log.TabIndex = 2;
            this.cb_AMBPatcher_generate_log.Text = "Generate Simple Log File";
            this.cb_AMBPatcher_generate_log.UseVisualStyleBackColor = true;
            // 
            // cb_AMBPatcher_progress_bar
            // 
            this.cb_AMBPatcher_progress_bar.AutoSize = true;
            this.cb_AMBPatcher_progress_bar.Location = new System.Drawing.Point(6, 8);
            this.cb_AMBPatcher_progress_bar.Name = "cb_AMBPatcher_progress_bar";
            this.cb_AMBPatcher_progress_bar.Size = new System.Drawing.Size(86, 17);
            this.cb_AMBPatcher_progress_bar.TabIndex = 1;
            this.cb_AMBPatcher_progress_bar.Text = "Progress Bar";
            this.cb_AMBPatcher_progress_bar.UseVisualStyleBackColor = true;
            // 
            // tabCsbEditor
            // 
            this.tabCsbEditor.Controls.Add(this.label7);
            this.tabCsbEditor.Controls.Add(this.num_CsbEditor_BufferSize);
            this.tabCsbEditor.Controls.Add(this.label6);
            this.tabCsbEditor.Controls.Add(this.num_CsbEditor_MaxThreads);
            this.tabCsbEditor.Controls.Add(this.cb_CsbEditor_EnableThreading);
            this.tabCsbEditor.Location = new System.Drawing.Point(4, 22);
            this.tabCsbEditor.Name = "tabCsbEditor";
            this.tabCsbEditor.Size = new System.Drawing.Size(358, 192);
            this.tabCsbEditor.TabIndex = 3;
            this.tabCsbEditor.Text = "CsbEditor";
            this.tabCsbEditor.UseVisualStyleBackColor = true;
            // 
            // tabAbout
            // 
            this.tabAbout.Controls.Add(this.bRL_7z);
            this.tabAbout.Controls.Add(this.label4);
            this.tabAbout.Controls.Add(this.link7z);
            this.tabAbout.Controls.Add(this.bRL_SAT);
            this.tabAbout.Controls.Add(this.bRL_S4ML);
            this.tabAbout.Controls.Add(this.label2);
            this.tabAbout.Controls.Add(this.linkMain);
            this.tabAbout.Controls.Add(this.label1);
            this.tabAbout.Controls.Add(this.linkSAT);
            this.tabAbout.Location = new System.Drawing.Point(4, 22);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabAbout.Size = new System.Drawing.Size(358, 192);
            this.tabAbout.TabIndex = 2;
            this.tabAbout.Text = "About";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // bRL_7z
            // 
            this.bRL_7z.Location = new System.Drawing.Point(261, 163);
            this.bRL_7z.Name = "bRL_7z";
            this.bRL_7z.Size = new System.Drawing.Size(91, 23);
            this.bRL_7z.TabIndex = 7;
            this.bRL_7z.Text = "Read License";
            this.bRL_7z.UseVisualStyleBackColor = true;
            this.bRL_7z.Click += new System.EventHandler(this.ReadLicense_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(205, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "7-Zip Copyright (C) 1999-2018 Igor Pavlov";
            // 
            // link7z
            // 
            this.link7z.AutoSize = true;
            this.link7z.Location = new System.Drawing.Point(22, 147);
            this.link7z.Name = "link7z";
            this.link7z.Size = new System.Drawing.Size(115, 13);
            this.link7z.TabIndex = 6;
            this.link7z.TabStop = true;
            this.link7z.Text = "https://www.7-zip.org/";
            this.link7z.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
            // 
            // bRL_SAT
            // 
            this.bRL_SAT.Location = new System.Drawing.Point(261, 99);
            this.bRL_SAT.Name = "bRL_SAT";
            this.bRL_SAT.Size = new System.Drawing.Size(91, 23);
            this.bRL_SAT.TabIndex = 4;
            this.bRL_SAT.Text = "Read License";
            this.bRL_SAT.UseVisualStyleBackColor = true;
            this.bRL_SAT.Click += new System.EventHandler(this.ReadLicense_Click);
            // 
            // bRL_S4ML
            // 
            this.bRL_S4ML.Location = new System.Drawing.Point(261, 35);
            this.bRL_S4ML.Name = "bRL_S4ML";
            this.bRL_S4ML.Size = new System.Drawing.Size(91, 23);
            this.bRL_S4ML.TabIndex = 2;
            this.bRL_S4ML.Text = "Read License";
            this.bRL_S4ML.UseVisualStyleBackColor = true;
            this.bRL_S4ML.Click += new System.EventHandler(this.ReadLicense_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(303, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "CsbEditor (from SonicAudioTools) by Skyth under MIT License.";
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(12, 236);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(180, 38);
            this.bOK.TabIndex = 98;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(198, 236);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(180, 38);
            this.bCancel.TabIndex = 99;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // cb_CsbEditor_EnableThreading
            // 
            this.cb_CsbEditor_EnableThreading.AutoSize = true;
            this.cb_CsbEditor_EnableThreading.Location = new System.Drawing.Point(6, 31);
            this.cb_CsbEditor_EnableThreading.Name = "cb_CsbEditor_EnableThreading";
            this.cb_CsbEditor_EnableThreading.Size = new System.Drawing.Size(110, 17);
            this.cb_CsbEditor_EnableThreading.TabIndex = 0;
            this.cb_CsbEditor_EnableThreading.Text = "Enable Threading";
            this.cb_CsbEditor_EnableThreading.UseVisualStyleBackColor = true;
            this.cb_CsbEditor_EnableThreading.CheckedChanged += new System.EventHandler(this.cb_CsbEditor_EnableThreading_CheckedChanged);
            // 
            // num_CsbEditor_MaxThreads
            // 
            this.num_CsbEditor_MaxThreads.Enabled = false;
            this.num_CsbEditor_MaxThreads.Location = new System.Drawing.Point(101, 52);
            this.num_CsbEditor_MaxThreads.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.num_CsbEditor_MaxThreads.Name = "num_CsbEditor_MaxThreads";
            this.num_CsbEditor_MaxThreads.Size = new System.Drawing.Size(84, 20);
            this.num_CsbEditor_MaxThreads.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Max Threads";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Buffer Size";
            // 
            // num_CsbEditor_BufferSize
            // 
            this.num_CsbEditor_BufferSize.Location = new System.Drawing.Point(101, 6);
            this.num_CsbEditor_BufferSize.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.num_CsbEditor_BufferSize.Name = "num_CsbEditor_BufferSize";
            this.num_CsbEditor_BufferSize.Size = new System.Drawing.Size(84, 20);
            this.num_CsbEditor_BufferSize.TabIndex = 3;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 286);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.tabControl1.ResumeLayout(false);
            this.tabInstallation.ResumeLayout(false);
            this.tabInstallation.PerformLayout();
            this.tabAMBPatcher.ResumeLayout(false);
            this.tabAMBPatcher.PerformLayout();
            this.tabCsbEditor.ResumeLayout(false);
            this.tabCsbEditor.PerformLayout();
            this.tabAbout.ResumeLayout(false);
            this.tabAbout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_CsbEditor_MaxThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_CsbEditor_BufferSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkSAT;
        private System.Windows.Forms.LinkLabel linkMain;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInstallation;
        private System.Windows.Forms.TabPage tabAMBPatcher;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.Button bRL_SAT;
        private System.Windows.Forms.Button bRL_S4ML;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cb_AMBPatcher_generate_log;
        private System.Windows.Forms.CheckBox cb_AMBPatcher_progress_bar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bInstall;
        private System.Windows.Forms.Label label_Installation_status;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cb_recover_orig;
        private System.Windows.Forms.RadioButton rb_delete;
        private System.Windows.Forms.RadioButton rb_rename;
        private System.Windows.Forms.CheckBox cb_AMBPatcher_sha_check;
        private System.Windows.Forms.ComboBox list_SHAType;
        private System.Windows.Forms.Button bRL_7z;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel link7z;
        private System.Windows.Forms.TabPage tabCsbEditor;
        private System.Windows.Forms.CheckBox cb_CsbEditor_EnableThreading;
        private System.Windows.Forms.NumericUpDown num_CsbEditor_MaxThreads;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown num_CsbEditor_BufferSize;
    }
}