namespace Sonic4ModManager
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.label1 = new System.Windows.Forms.Label();
            this.linkSAT = new System.Windows.Forms.LinkLabel();
            this.linkMain = new System.Windows.Forms.LinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabInstallation = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.bInstall = new System.Windows.Forms.Button();
            this.label_Installation_status = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabAMBPatcher = new System.Windows.Forms.TabPage();
            this.cb_AMBPatcher_generate_log = new System.Windows.Forms.CheckBox();
            this.cb_AMBPatcher_progress_bar = new System.Windows.Forms.CheckBox();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.bRL_SAT = new System.Windows.Forms.Button();
            this.bRL_S4ML = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.cb_AMBPatcher_sha_check = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabInstallation.SuspendLayout();
            this.tabAMBPatcher.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mod Manager and AMBPather by OSA413 under MIT License.";
            // 
            // linkSAT
            // 
            this.linkSAT.AutoSize = true;
            this.linkSAT.Location = new System.Drawing.Point(22, 97);
            this.linkSAT.Name = "linkSAT";
            this.linkSAT.Size = new System.Drawing.Size(282, 13);
            this.linkSAT.TabIndex = 3;
            this.linkSAT.TabStop = true;
            this.linkSAT.Text = "https://github.com/blueskythlikesclouds/SonicAudioTools";
            this.linkSAT.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkSAT_LinkClicked);
            // 
            // linkMain
            // 
            this.linkMain.AutoSize = true;
            this.linkMain.Location = new System.Drawing.Point(22, 28);
            this.linkMain.Name = "linkMain";
            this.linkMain.Size = new System.Drawing.Size(238, 13);
            this.linkMain.TabIndex = 1;
            this.linkMain.TabStop = true;
            this.linkMain.Text = "https://github.com/OSA413/Sonic4_ModLoader";
            this.linkMain.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkMain_LinkClicked);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabInstallation);
            this.tabControl1.Controls.Add(this.tabAMBPatcher);
            this.tabControl1.Controls.Add(this.tabAbout);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(366, 218);
            this.tabControl1.TabIndex = 0;
            // 
            // tabInstallation
            // 
            this.tabInstallation.Controls.Add(this.label5);
            this.tabInstallation.Controls.Add(this.checkBox1);
            this.tabInstallation.Controls.Add(this.radioButton2);
            this.tabInstallation.Controls.Add(this.radioButton1);
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
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(64, 152);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(231, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Recover original game files (AMBs, CSBs...)";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(64, 129);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(150, 17);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Delete all Mod Loader files";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(64, 106);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(113, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Rename files back";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // bInstall
            // 
            this.bInstall.Location = new System.Drawing.Point(115, 55);
            this.bInstall.Name = "bInstall";
            this.bInstall.Size = new System.Drawing.Size(128, 32);
            this.bInstall.TabIndex = 0;
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
            // cb_AMBPatcher_generate_log
            // 
            this.cb_AMBPatcher_generate_log.AutoSize = true;
            this.cb_AMBPatcher_generate_log.Location = new System.Drawing.Point(7, 29);
            this.cb_AMBPatcher_generate_log.Name = "cb_AMBPatcher_generate_log";
            this.cb_AMBPatcher_generate_log.Size = new System.Drawing.Size(144, 17);
            this.cb_AMBPatcher_generate_log.TabIndex = 1;
            this.cb_AMBPatcher_generate_log.Text = "Generate Simple Log File";
            this.cb_AMBPatcher_generate_log.UseVisualStyleBackColor = true;
            // 
            // cb_AMBPatcher_progress_bar
            // 
            this.cb_AMBPatcher_progress_bar.AutoSize = true;
            this.cb_AMBPatcher_progress_bar.Location = new System.Drawing.Point(6, 6);
            this.cb_AMBPatcher_progress_bar.Name = "cb_AMBPatcher_progress_bar";
            this.cb_AMBPatcher_progress_bar.Size = new System.Drawing.Size(86, 17);
            this.cb_AMBPatcher_progress_bar.TabIndex = 0;
            this.cb_AMBPatcher_progress_bar.Text = "Progress Bar";
            this.cb_AMBPatcher_progress_bar.UseVisualStyleBackColor = true;
            // 
            // tabAbout
            // 
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
            // bRL_SAT
            // 
            this.bRL_SAT.Location = new System.Drawing.Point(261, 113);
            this.bRL_SAT.Name = "bRL_SAT";
            this.bRL_SAT.Size = new System.Drawing.Size(91, 23);
            this.bRL_SAT.TabIndex = 4;
            this.bRL_SAT.Text = "Read License";
            this.bRL_SAT.UseVisualStyleBackColor = true;
            this.bRL_SAT.Click += new System.EventHandler(this.bRL_SAT_Click);
            // 
            // bRL_S4ML
            // 
            this.bRL_S4ML.Location = new System.Drawing.Point(261, 44);
            this.bRL_S4ML.Name = "bRL_S4ML";
            this.bRL_S4ML.Size = new System.Drawing.Size(91, 23);
            this.bRL_S4ML.TabIndex = 2;
            this.bRL_S4ML.Text = "Read License";
            this.bRL_S4ML.UseVisualStyleBackColor = true;
            this.bRL_S4ML.Click += new System.EventHandler(this.bRL_S4ML_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(303, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "CsbEditor (from SonicAudioTools) by Skyth under MIT License.";
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(12, 236);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(180, 38);
            this.bOK.TabIndex = 1;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(198, 236);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(180, 38);
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // cb_AMBPatcher_sha_check
            // 
            this.cb_AMBPatcher_sha_check.AutoSize = true;
            this.cb_AMBPatcher_sha_check.Location = new System.Drawing.Point(6, 52);
            this.cb_AMBPatcher_sha_check.Name = "cb_AMBPatcher_sha_check";
            this.cb_AMBPatcher_sha_check.Size = new System.Drawing.Size(187, 17);
            this.cb_AMBPatcher_sha_check.TabIndex = 2;
            this.cb_AMBPatcher_sha_check.Text = "Check files\' SHA1 (recommended)";
            this.cb_AMBPatcher_sha_check.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 286);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Settings";
            this.Text = "Settings";
            this.tabControl1.ResumeLayout(false);
            this.tabInstallation.ResumeLayout(false);
            this.tabInstallation.PerformLayout();
            this.tabAMBPatcher.ResumeLayout(false);
            this.tabAMBPatcher.PerformLayout();
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
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.CheckBox cb_AMBPatcher_sha_check;
    }
}