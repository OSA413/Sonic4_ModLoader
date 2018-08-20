namespace OneClickModInstaller
{
    partial class WrongModStructureForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WrongModStructureForm));
            this.label1 = new System.Windows.Forms.Label();
            this.bOpen = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(280, 125);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // bOpen
            // 
            this.bOpen.Location = new System.Drawing.Point(12, 137);
            this.bOpen.Name = "bOpen";
            this.bOpen.Size = new System.Drawing.Size(128, 52);
            this.bOpen.TabIndex = 1;
            this.bOpen.Text = "Open extracted mod in Explorer and Exit";
            this.bOpen.UseVisualStyleBackColor = true;
            this.bOpen.Click += new System.EventHandler(this.bOpen_Click);
            // 
            // bExit
            // 
            this.bExit.Location = new System.Drawing.Point(164, 137);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(128, 52);
            this.bExit.TabIndex = 2;
            this.bExit.Text = "Exit";
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // WrongModStructureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 201);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bOpen);
            this.Controls.Add(this.label1);
            this.Name = "WrongModStructureForm";
            this.Text = "Wrong Mod Structure";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bOpen;
        private System.Windows.Forms.Button bExit;
    }
}