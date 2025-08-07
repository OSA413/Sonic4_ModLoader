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
            tabControl1 = new System.Windows.Forms.TabControl();
            tabInstallation = new System.Windows.Forms.TabPage();
            cbKeepConfigs = new System.Windows.Forms.CheckBox();
            cb_ForceUninstall = new System.Windows.Forms.CheckBox();
            cb_Uninstall_OCMI = new System.Windows.Forms.CheckBox();
            label5 = new System.Windows.Forms.Label();
            cb_recover_orig = new System.Windows.Forms.CheckBox();
            rb_delete = new System.Windows.Forms.RadioButton();
            rb_rename = new System.Windows.Forms.RadioButton();
            bInstall = new System.Windows.Forms.Button();
            label_Installation_status = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            tabProgram = new System.Windows.Forms.TabPage();
            groupBox2 = new System.Windows.Forms.GroupBox();
            label7 = new System.Windows.Forms.Label();
            num_CsbEditor_BufferSize = new System.Windows.Forms.NumericUpDown();
            label6 = new System.Windows.Forms.Label();
            num_CsbEditor_MaxThreads = new System.Windows.Forms.NumericUpDown();
            cb_CsbEditor_EnableThreading = new System.Windows.Forms.CheckBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            cb_AMBPatcher_progress_bar = new System.Windows.Forms.CheckBox();
            bRecoverOriginalFiles = new System.Windows.Forms.Button();
            cb_AMBPatcher_sha_check = new System.Windows.Forms.CheckBox();
            bOK = new System.Windows.Forms.Button();
            bCancel = new System.Windows.Forms.Button();
            tabControl1.SuspendLayout();
            tabInstallation.SuspendLayout();
            tabProgram.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_CsbEditor_BufferSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_CsbEditor_MaxThreads).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabInstallation);
            tabControl1.Controls.Add(tabProgram);
            tabControl1.ItemSize = new System.Drawing.Size(90, 25);
            tabControl1.Location = new System.Drawing.Point(16, 19);
            tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabControl1.Name = "tabControl1";
            tabControl1.Padding = new System.Drawing.Point(32, 3);
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(488, 335);
            tabControl1.TabIndex = 0;
            // 
            // tabInstallation
            // 
            tabInstallation.AutoScroll = true;
            tabInstallation.AutoScrollMargin = new System.Drawing.Size(0, 8);
            tabInstallation.Controls.Add(cbKeepConfigs);
            tabInstallation.Controls.Add(cb_ForceUninstall);
            tabInstallation.Controls.Add(cb_Uninstall_OCMI);
            tabInstallation.Controls.Add(label5);
            tabInstallation.Controls.Add(cb_recover_orig);
            tabInstallation.Controls.Add(rb_delete);
            tabInstallation.Controls.Add(rb_rename);
            tabInstallation.Controls.Add(bInstall);
            tabInstallation.Controls.Add(label_Installation_status);
            tabInstallation.Controls.Add(label3);
            tabInstallation.Location = new System.Drawing.Point(4, 29);
            tabInstallation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabInstallation.Name = "tabInstallation";
            tabInstallation.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabInstallation.Size = new System.Drawing.Size(480, 302);
            tabInstallation.TabIndex = 0;
            tabInstallation.Text = "Installation";
            tabInstallation.UseVisualStyleBackColor = true;
            // 
            // cbKeepConfigs
            // 
            cbKeepConfigs.AutoSize = true;
            cbKeepConfigs.Location = new System.Drawing.Point(296, 195);
            cbKeepConfigs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cbKeepConfigs.Name = "cbKeepConfigs";
            cbKeepConfigs.Size = new System.Drawing.Size(117, 24);
            cbKeepConfigs.TabIndex = 10;
            cbKeepConfigs.Text = "Keep configs";
            cbKeepConfigs.UseVisualStyleBackColor = true;
            // 
            // cb_ForceUninstall
            // 
            cb_ForceUninstall.AutoSize = true;
            cb_ForceUninstall.Location = new System.Drawing.Point(181, 104);
            cb_ForceUninstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cb_ForceUninstall.Name = "cb_ForceUninstall";
            cb_ForceUninstall.Size = new System.Drawing.Size(126, 24);
            cb_ForceUninstall.TabIndex = 9;
            cb_ForceUninstall.Text = "Force uninstall";
            cb_ForceUninstall.UseVisualStyleBackColor = true;
            cb_ForceUninstall.CheckedChanged += cb_ForceUninstall_CheckedChanged;
            // 
            // cb_Uninstall_OCMI
            // 
            cb_Uninstall_OCMI.AutoSize = true;
            cb_Uninstall_OCMI.Location = new System.Drawing.Point(112, 230);
            cb_Uninstall_OCMI.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cb_Uninstall_OCMI.Name = "cb_Uninstall_OCMI";
            cb_Uninstall_OCMI.Size = new System.Drawing.Size(322, 24);
            cb_Uninstall_OCMI.TabIndex = 7;
            cb_Uninstall_OCMI.Text = "Uninstall and delete One-Click Mod Installer";
            cb_Uninstall_OCMI.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(112, 135);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(157, 20);
            label5.TabIndex = 6;
            label5.Text = "Uninstallation options:";
            // 
            // cb_recover_orig
            // 
            cb_recover_orig.AutoSize = true;
            cb_recover_orig.Location = new System.Drawing.Point(91, 266);
            cb_recover_orig.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cb_recover_orig.Name = "cb_recover_orig";
            cb_recover_orig.Size = new System.Drawing.Size(312, 24);
            cb_recover_orig.TabIndex = 4;
            cb_recover_orig.Text = "Recover original game files (AMBs, CSBs...)";
            cb_recover_orig.UseVisualStyleBackColor = true;
            // 
            // rb_delete
            // 
            rb_delete.AutoSize = true;
            rb_delete.Location = new System.Drawing.Point(91, 195);
            rb_delete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            rb_delete.Name = "rb_delete";
            rb_delete.Size = new System.Drawing.Size(210, 24);
            rb_delete.TabIndex = 3;
            rb_delete.TabStop = true;
            rb_delete.Text = "Delete all Mod Loader files";
            rb_delete.UseVisualStyleBackColor = true;
            rb_delete.CheckedChanged += rb_delete_CheckedChanged;
            // 
            // rb_rename
            // 
            rb_rename.AutoSize = true;
            rb_rename.Location = new System.Drawing.Point(91, 160);
            rb_rename.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            rb_rename.Name = "rb_rename";
            rb_rename.Size = new System.Drawing.Size(150, 24);
            rb_rename.TabIndex = 2;
            rb_rename.TabStop = true;
            rb_rename.Text = "Rename files back";
            rb_rename.UseVisualStyleBackColor = true;
            // 
            // bInstall
            // 
            bInstall.Location = new System.Drawing.Point(159, 46);
            bInstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bInstall.Name = "bInstall";
            bInstall.Size = new System.Drawing.Size(171, 49);
            bInstall.TabIndex = 1;
            bInstall.Text = "(Un)install";
            bInstall.UseVisualStyleBackColor = true;
            bInstall.Click += bInstall_Click;
            // 
            // label_Installation_status
            // 
            label_Installation_status.AutoSize = true;
            label_Installation_status.Location = new System.Drawing.Point(137, 12);
            label_Installation_status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_Installation_status.Name = "label_Installation_status";
            label_Installation_status.Size = new System.Drawing.Size(205, 20);
            label_Installation_status.TabIndex = 1;
            label_Installation_status.Text = "//Installation status goes here";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(8, 12);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(127, 20);
            label3.TabIndex = 0;
            label3.Text = "Installation status:";
            // 
            // tabProgram
            // 
            tabProgram.Controls.Add(groupBox2);
            tabProgram.Controls.Add(groupBox1);
            tabProgram.Location = new System.Drawing.Point(4, 29);
            tabProgram.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabProgram.Name = "tabProgram";
            tabProgram.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabProgram.Size = new System.Drawing.Size(480, 302);
            tabProgram.TabIndex = 1;
            tabProgram.Text = "Programs";
            tabProgram.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(num_CsbEditor_BufferSize);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(num_CsbEditor_MaxThreads);
            groupBox2.Controls.Add(cb_CsbEditor_EnableThreading);
            groupBox2.Location = new System.Drawing.Point(243, 9);
            groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox2.Size = new System.Drawing.Size(230, 285);
            groupBox2.TabIndex = 10;
            groupBox2.TabStop = false;
            groupBox2.Text = "CSB Editor";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(7, 76);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(80, 20);
            label7.TabIndex = 9;
            label7.Text = "Buffer Size";
            // 
            // num_CsbEditor_BufferSize
            // 
            num_CsbEditor_BufferSize.Location = new System.Drawing.Point(134, 72);
            num_CsbEditor_BufferSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            num_CsbEditor_BufferSize.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            num_CsbEditor_BufferSize.Name = "num_CsbEditor_BufferSize";
            num_CsbEditor_BufferSize.Size = new System.Drawing.Size(89, 27);
            num_CsbEditor_BufferSize.TabIndex = 8;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(34, 149);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(93, 20);
            label6.TabIndex = 7;
            label6.Text = "Max Threads";
            // 
            // num_CsbEditor_MaxThreads
            // 
            num_CsbEditor_MaxThreads.Enabled = false;
            num_CsbEditor_MaxThreads.Location = new System.Drawing.Point(134, 145);
            num_CsbEditor_MaxThreads.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            num_CsbEditor_MaxThreads.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            num_CsbEditor_MaxThreads.Name = "num_CsbEditor_MaxThreads";
            num_CsbEditor_MaxThreads.Size = new System.Drawing.Size(89, 27);
            num_CsbEditor_MaxThreads.TabIndex = 6;
            // 
            // cb_CsbEditor_EnableThreading
            // 
            cb_CsbEditor_EnableThreading.AutoSize = true;
            cb_CsbEditor_EnableThreading.Location = new System.Drawing.Point(7, 112);
            cb_CsbEditor_EnableThreading.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cb_CsbEditor_EnableThreading.Name = "cb_CsbEditor_EnableThreading";
            cb_CsbEditor_EnableThreading.Size = new System.Drawing.Size(147, 24);
            cb_CsbEditor_EnableThreading.TabIndex = 5;
            cb_CsbEditor_EnableThreading.Text = "Enable Threading";
            cb_CsbEditor_EnableThreading.UseVisualStyleBackColor = true;
            cb_CsbEditor_EnableThreading.CheckedChanged += cb_CsbEditor_EnableThreading_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cb_AMBPatcher_progress_bar);
            groupBox1.Controls.Add(bRecoverOriginalFiles);
            groupBox1.Controls.Add(cb_AMBPatcher_sha_check);
            groupBox1.Location = new System.Drawing.Point(7, 9);
            groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox1.Size = new System.Drawing.Size(230, 285);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "AMB Patcher";
            // 
            // cb_AMBPatcher_progress_bar
            // 
            cb_AMBPatcher_progress_bar.AutoSize = true;
            cb_AMBPatcher_progress_bar.Location = new System.Drawing.Point(48, 76);
            cb_AMBPatcher_progress_bar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cb_AMBPatcher_progress_bar.Name = "cb_AMBPatcher_progress_bar";
            cb_AMBPatcher_progress_bar.Size = new System.Drawing.Size(113, 24);
            cb_AMBPatcher_progress_bar.TabIndex = 1;
            cb_AMBPatcher_progress_bar.Text = "Progress Bar";
            cb_AMBPatcher_progress_bar.UseVisualStyleBackColor = true;
            // 
            // bRecoverOriginalFiles
            // 
            bRecoverOriginalFiles.Location = new System.Drawing.Point(34, 228);
            bRecoverOriginalFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bRecoverOriginalFiles.Name = "bRecoverOriginalFiles";
            bRecoverOriginalFiles.Size = new System.Drawing.Size(171, 49);
            bRecoverOriginalFiles.TabIndex = 8;
            bRecoverOriginalFiles.Text = "Recover original files";
            bRecoverOriginalFiles.UseVisualStyleBackColor = true;
            bRecoverOriginalFiles.Click += bRecoverOriginalFiles_Click;
            // 
            // cb_AMBPatcher_sha_check
            // 
            cb_AMBPatcher_sha_check.AutoSize = true;
            cb_AMBPatcher_sha_check.Location = new System.Drawing.Point(48, 112);
            cb_AMBPatcher_sha_check.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cb_AMBPatcher_sha_check.Name = "cb_AMBPatcher_sha_check";
            cb_AMBPatcher_sha_check.Size = new System.Drawing.Size(152, 44);
            cb_AMBPatcher_sha_check.TabIndex = 3;
            cb_AMBPatcher_sha_check.Text = "Check SHA of files\r\n(recommended)";
            cb_AMBPatcher_sha_check.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            bOK.Location = new System.Drawing.Point(16, 362);
            bOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bOK.Name = "bOK";
            bOK.Size = new System.Drawing.Size(240, 59);
            bOK.TabIndex = 98;
            bOK.Text = "OK";
            bOK.UseVisualStyleBackColor = true;
            bOK.Click += bOK_Click;
            // 
            // bCancel
            // 
            bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            bCancel.Location = new System.Drawing.Point(264, 362);
            bCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bCancel.Name = "bCancel";
            bCancel.Size = new System.Drawing.Size(240, 59);
            bCancel.TabIndex = 99;
            bCancel.Text = "Cancel";
            bCancel.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(520, 440);
            Controls.Add(bCancel);
            Controls.Add(bOK);
            Controls.Add(tabControl1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "SettingsForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Settings";
            tabControl1.ResumeLayout(false);
            tabInstallation.ResumeLayout(false);
            tabInstallation.PerformLayout();
            tabProgram.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_CsbEditor_BufferSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_CsbEditor_MaxThreads).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInstallation;
        private System.Windows.Forms.TabPage tabProgram;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.CheckBox cb_AMBPatcher_progress_bar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bInstall;
        private System.Windows.Forms.Label label_Installation_status;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cb_recover_orig;
        private System.Windows.Forms.RadioButton rb_delete;
        private System.Windows.Forms.RadioButton rb_rename;
        private System.Windows.Forms.CheckBox cb_AMBPatcher_sha_check;
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
    }
}