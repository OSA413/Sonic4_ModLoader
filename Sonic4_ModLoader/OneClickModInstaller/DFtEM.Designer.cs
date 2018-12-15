namespace OneClickModInstaller
{
    partial class DFtEM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DFtEM));
            this.label1 = new System.Windows.Forms.Label();
            this.bOpen = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Downloaded mod has been installed.\r\nDon\'t forget to enable it in Mod Manager.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bOpen
            // 
            this.bOpen.Location = new System.Drawing.Point(12, 137);
            this.bOpen.Name = "bOpen";
            this.bOpen.Size = new System.Drawing.Size(128, 52);
            this.bOpen.TabIndex = 3;
            this.bOpen.Text = "Open Mod Manager and Exit";
            this.bOpen.UseVisualStyleBackColor = true;
            this.bOpen.Click += new System.EventHandler(this.bOpen_Click);
            // 
            // bExit
            // 
            this.bExit.Location = new System.Drawing.Point(164, 137);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(128, 52);
            this.bExit.TabIndex = 4;
            this.bExit.Text = "Exit";
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // DFtEM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 201);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bOpen);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DFtEM";
            this.Text = "Mod Installed";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bOpen;
        private System.Windows.Forms.Button bExit;
    }
}