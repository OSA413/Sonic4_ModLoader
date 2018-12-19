namespace OneClickModInstaller
{
    partial class Suspicious
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Suspicious));
            this.label1 = new System.Windows.Forms.Label();
            this.bContinue = new System.Windows.Forms.Button();
            this.bCancelAndExit = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.clFileNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(279, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Suspicious files have been found in the downloaded mod:\r\n";
            // 
            // bContinue
            // 
            this.bContinue.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.bContinue.Location = new System.Drawing.Point(64, 165);
            this.bContinue.Name = "bContinue";
            this.bContinue.Size = new System.Drawing.Size(128, 40);
            this.bContinue.TabIndex = 2;
            this.bContinue.Text = "Continue";
            this.bContinue.UseVisualStyleBackColor = true;
            this.bContinue.Click += new System.EventHandler(this.bContinue_Click);
            // 
            // bCancelAndExit
            // 
            this.bCancelAndExit.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.bCancelAndExit.Location = new System.Drawing.Point(320, 165);
            this.bCancelAndExit.Name = "bCancelAndExit";
            this.bCancelAndExit.Size = new System.Drawing.Size(128, 40);
            this.bCancelAndExit.TabIndex = 3;
            this.bCancelAndExit.Text = "Exit";
            this.bCancelAndExit.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clFileNames});
            this.listView1.Location = new System.Drawing.Point(15, 32);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(469, 124);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // clFileNames
            // 
            this.clFileNames.Text = "Files";
            this.clFileNames.Width = 440;
            // 
            // Suspicious
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 217);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.bCancelAndExit);
            this.Controls.Add(this.bContinue);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Suspicious";
            this.Text = "Suspicious Dialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bContinue;
        private System.Windows.Forms.Button bCancelAndExit;
        private System.Windows.Forms.ColumnHeader clFileNames;
        private System.Windows.Forms.ListView listView1;
    }
}