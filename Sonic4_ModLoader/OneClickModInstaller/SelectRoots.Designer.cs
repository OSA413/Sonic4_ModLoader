namespace OneClickModInstaller
{
    partial class SelectRoots
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectRoots));
            this.bContinue = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.ilIcons = new System.Windows.Forms.ImageList(this.components);
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bContinue
            // 
            this.bContinue.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.bContinue.Location = new System.Drawing.Point(111, 230);
            this.bContinue.Name = "bContinue";
            this.bContinue.Size = new System.Drawing.Size(128, 40);
            this.bContinue.TabIndex = 5;
            this.bContinue.Text = "Confirm";
            this.bContinue.UseVisualStyleBackColor = true;
            this.bContinue.Click += new System.EventHandler(this.bContinue_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(441, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Installer couldn\'t find any root directories to install. Please, select any or co" +
    "ntact mod creator.";
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(15, 32);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(469, 184);
            this.treeView1.TabIndex = 6;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            // 
            // ilIcons
            // 
            this.ilIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIcons.ImageStream")));
            this.ilIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilIcons.Images.SetKeyName(0, "application-x-executable");
            this.ilIcons.Images.SetKeyName(1, "audio-x-generic");
            this.ilIcons.Images.SetKeyName(2, "folder");
            this.ilIcons.Images.SetKeyName(3, "image-missing");
            this.ilIcons.Images.SetKeyName(4, "image-x-generic");
            this.ilIcons.Images.SetKeyName(5, "package-x-generic");
            this.ilIcons.Images.SetKeyName(6, "text-x-generic");
            this.ilIcons.Images.SetKeyName(7, "text-x-script");
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(272, 230);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(128, 40);
            this.bCancel.TabIndex = 7;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // SelectRoots
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 281);
            this.ControlBox = false;
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.bContinue);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectRoots";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SelectRoots";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bContinue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList ilIcons;
        private System.Windows.Forms.Button bCancel;
    }
}