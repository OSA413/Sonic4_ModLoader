namespace OneClickModInstaller
{
    partial class Install
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
            this.label1 = new System.Windows.Forms.Label();
            this.lInstallationStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bInstall = new System.Windows.Forms.Button();
            this.bUninstall = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Installation status: ";
            // 
            // lInstallationStatus
            // 
            this.lInstallationStatus.AutoSize = true;
            this.lInstallationStatus.Location = new System.Drawing.Point(112, 40);
            this.lInstallationStatus.Name = "lInstallationStatus";
            this.lInstallationStatus.Size = new System.Drawing.Size(148, 13);
            this.lInstallationStatus.TabIndex = 1;
            this.lInstallationStatus.Text = "//Installation status goes here";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "1-Click Mod Installer";
            // 
            // bInstall
            // 
            this.bInstall.Location = new System.Drawing.Point(12, 99);
            this.bInstall.Name = "bInstall";
            this.bInstall.Size = new System.Drawing.Size(256, 32);
            this.bInstall.TabIndex = 3;
            this.bInstall.Text = "Install/Fix";
            this.bInstall.UseVisualStyleBackColor = true;
            this.bInstall.Click += new System.EventHandler(this.bInstall_Click);
            // 
            // bUninstall
            // 
            this.bUninstall.Location = new System.Drawing.Point(12, 137);
            this.bUninstall.Name = "bUninstall";
            this.bUninstall.Size = new System.Drawing.Size(256, 32);
            this.bUninstall.TabIndex = 4;
            this.bUninstall.Text = "Uninstall";
            this.bUninstall.UseVisualStyleBackColor = true;
            this.bUninstall.Click += new System.EventHandler(this.bUninstall_Click);
            // 
            // Install
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 181);
            this.Controls.Add(this.bUninstall);
            this.Controls.Add(this.bInstall);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lInstallationStatus);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Install";
            this.Text = "Install";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lInstallationStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bInstall;
        private System.Windows.Forms.Button bUninstall;
    }
}