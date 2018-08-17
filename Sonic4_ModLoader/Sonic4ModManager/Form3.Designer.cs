namespace Sonic4ModManager
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.label1 = new System.Windows.Forms.Label();
            this.linkSAT = new System.Windows.Forms.LinkLabel();
            this.linkMain = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 52);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mod Manager and AMB Pather by OSA413 under MIT License.\r\n\r\n\r\nCSB Editor (from Son" +
    "icAudioTools) by Skyth under MIT License.";
            // 
            // linkSAT
            // 
            this.linkSAT.AutoSize = true;
            this.linkSAT.Location = new System.Drawing.Point(28, 61);
            this.linkSAT.Name = "linkSAT";
            this.linkSAT.Size = new System.Drawing.Size(282, 13);
            this.linkSAT.TabIndex = 2;
            this.linkSAT.TabStop = true;
            this.linkSAT.Text = "https://github.com/blueskythlikesclouds/SonicAudioTools";
            this.linkSAT.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkSAT_LinkClicked);
            // 
            // linkMain
            // 
            this.linkMain.AutoSize = true;
            this.linkMain.Location = new System.Drawing.Point(28, 22);
            this.linkMain.Name = "linkMain";
            this.linkMain.Size = new System.Drawing.Size(238, 13);
            this.linkMain.TabIndex = 3;
            this.linkMain.TabStop = true;
            this.linkMain.Text = "https://github.com/OSA413/Sonic4_ModLoader";
            this.linkMain.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkMain_LinkClicked);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 83);
            this.Controls.Add(this.linkMain);
            this.Controls.Add(this.linkSAT);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form3";
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkSAT;
        private System.Windows.Forms.LinkLabel linkMain;
    }
}