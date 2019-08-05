namespace Sonic4ModManager
{
    partial class FirstLaunch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirstLaunch));
            this.bNo = new System.Windows.Forms.Button();
            this.bYes = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bIDUNNO = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bNo
            // 
            this.bNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.bNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.bNo.Location = new System.Drawing.Point(316, 103);
            this.bNo.Name = "bNo";
            this.bNo.Size = new System.Drawing.Size(64, 32);
            this.bNo.TabIndex = 1;
            this.bNo.Text = "No";
            this.bNo.UseVisualStyleBackColor = false;
            this.bNo.Click += new System.EventHandler(this.bNo_Click);
            // 
            // bYes
            // 
            this.bYes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(237)))));
            this.bYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.bYes.Location = new System.Drawing.Point(64, 103);
            this.bYes.Name = "bYes";
            this.bYes.Size = new System.Drawing.Size(64, 32);
            this.bYes.TabIndex = 2;
            this.bYes.Text = "Yes";
            this.bYes.UseVisualStyleBackColor = false;
            this.bYes.Click += new System.EventHandler(this.bYes_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(417, 91);
            this.label1.TabIndex = 3;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // bIDUNNO
            // 
            this.bIDUNNO.BackColor = System.Drawing.SystemColors.Control;
            this.bIDUNNO.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.bIDUNNO.Location = new System.Drawing.Point(192, 103);
            this.bIDUNNO.Name = "bIDUNNO";
            this.bIDUNNO.Size = new System.Drawing.Size(64, 32);
            this.bIDUNNO.TabIndex = 0;
            this.bIDUNNO.Text = "Ask later";
            this.bIDUNNO.UseVisualStyleBackColor = false;
            // 
            // FirstLaunch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 156);
            this.Controls.Add(this.bIDUNNO);
            this.Controls.Add(this.bNo);
            this.Controls.Add(this.bYes);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FirstLaunch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "First Launch Dialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bNo;
        private System.Windows.Forms.Button bYes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bIDUNNO;
    }
}