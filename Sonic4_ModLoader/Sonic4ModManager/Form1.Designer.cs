namespace Sonic4ModManager
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listMods = new System.Windows.Forms.ListView();
            this.clName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clAuthors = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bSave = new System.Windows.Forms.Button();
            this.bPriorityUp = new System.Windows.Forms.Button();
            this.bPriorityDown = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.bSaveAndPlay = new System.Windows.Forms.Button();
            this.bRefresh = new System.Windows.Forms.Button();
            this.bAbout = new System.Windows.Forms.Button();
            this.bPriorityFirst = new System.Windows.Forms.Button();
            this.bPriorityLast = new System.Windows.Forms.Button();
            this.bRandom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listMods
            // 
            this.listMods.CheckBoxes = true;
            this.listMods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clName,
            this.clAuthors,
            this.clVersion});
            this.listMods.Location = new System.Drawing.Point(12, 12);
            this.listMods.Name = "listMods";
            this.listMods.Size = new System.Drawing.Size(512, 200);
            this.listMods.TabIndex = 1;
            this.listMods.UseCompatibleStateImageBehavior = false;
            this.listMods.View = System.Windows.Forms.View.Details;
            // 
            // clName
            // 
            this.clName.Text = "Name";
            this.clName.Width = 216;
            // 
            // clAuthors
            // 
            this.clAuthors.Text = "Author(s)";
            this.clAuthors.Width = 205;
            // 
            // clVersion
            // 
            this.clVersion.Text = "Version";
            this.clVersion.Width = 70;
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(12, 218);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(100, 40);
            this.bSave.TabIndex = 6;
            this.bSave.Text = "Save";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bPriorityUp
            // 
            this.bPriorityUp.Location = new System.Drawing.Point(530, 54);
            this.bPriorityUp.Name = "bPriorityUp";
            this.bPriorityUp.Size = new System.Drawing.Size(32, 58);
            this.bPriorityUp.TabIndex = 3;
            this.bPriorityUp.Text = "/\\";
            this.bPriorityUp.UseVisualStyleBackColor = true;
            this.bPriorityUp.Click += new System.EventHandler(this.bPriorityUp_Click);
            // 
            // bPriorityDown
            // 
            this.bPriorityDown.Location = new System.Drawing.Point(530, 112);
            this.bPriorityDown.Name = "bPriorityDown";
            this.bPriorityDown.Size = new System.Drawing.Size(32, 58);
            this.bPriorityDown.TabIndex = 4;
            this.bPriorityDown.Text = "\\/";
            this.bPriorityDown.UseVisualStyleBackColor = true;
            this.bPriorityDown.Click += new System.EventHandler(this.bPriorityDown_Click);
            // 
            // bExit
            // 
            this.bExit.Location = new System.Drawing.Point(462, 264);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(100, 40);
            this.bExit.TabIndex = 10;
            this.bExit.Text = "Exit";
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bSaveAndPlay
            // 
            this.bSaveAndPlay.Location = new System.Drawing.Point(12, 264);
            this.bSaveAndPlay.Name = "bSaveAndPlay";
            this.bSaveAndPlay.Size = new System.Drawing.Size(100, 40);
            this.bSaveAndPlay.TabIndex = 7;
            this.bSaveAndPlay.Text = "Save and Play";
            this.bSaveAndPlay.UseVisualStyleBackColor = true;
            this.bSaveAndPlay.Click += new System.EventHandler(this.bSaveAndPlay_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.Location = new System.Drawing.Point(237, 218);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(100, 40);
            this.bRefresh.TabIndex = 8;
            this.bRefresh.Text = "Refresh";
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bAbout
            // 
            this.bAbout.Location = new System.Drawing.Point(462, 218);
            this.bAbout.Name = "bAbout";
            this.bAbout.Size = new System.Drawing.Size(100, 40);
            this.bAbout.TabIndex = 9;
            this.bAbout.Text = "About";
            this.bAbout.UseVisualStyleBackColor = true;
            this.bAbout.Click += new System.EventHandler(this.bAbout_Click);
            // 
            // bPriorityFirst
            // 
            this.bPriorityFirst.Location = new System.Drawing.Point(530, 12);
            this.bPriorityFirst.Name = "bPriorityFirst";
            this.bPriorityFirst.Size = new System.Drawing.Size(32, 36);
            this.bPriorityFirst.TabIndex = 2;
            this.bPriorityFirst.Text = "/\\\r\n/\\";
            this.bPriorityFirst.UseVisualStyleBackColor = true;
            this.bPriorityFirst.Click += new System.EventHandler(this.bPriorityFirst_Click);
            // 
            // bPriorityLast
            // 
            this.bPriorityLast.Location = new System.Drawing.Point(530, 176);
            this.bPriorityLast.Name = "bPriorityLast";
            this.bPriorityLast.Size = new System.Drawing.Size(32, 36);
            this.bPriorityLast.TabIndex = 5;
            this.bPriorityLast.Text = "\\/\r\n\\/";
            this.bPriorityLast.UseVisualStyleBackColor = true;
            this.bPriorityLast.Click += new System.EventHandler(this.bPriorityLast_Click);
            // 
            // bRandom
            // 
            this.bRandom.Location = new System.Drawing.Point(237, 264);
            this.bRandom.Name = "bRandom";
            this.bRandom.Size = new System.Drawing.Size(100, 40);
            this.bRandom.TabIndex = 11;
            this.bRandom.Text = "Random";
            this.bRandom.UseVisualStyleBackColor = true;
            this.bRandom.Click += new System.EventHandler(this.bRandom_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 316);
            this.Controls.Add(this.bRandom);
            this.Controls.Add(this.bPriorityLast);
            this.Controls.Add(this.bPriorityFirst);
            this.Controls.Add(this.bAbout);
            this.Controls.Add(this.bRefresh);
            this.Controls.Add(this.bSaveAndPlay);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bPriorityDown);
            this.Controls.Add(this.bPriorityUp);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.listMods);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Sonic 4 Mod Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listMods;
        private System.Windows.Forms.ColumnHeader clName;
        private System.Windows.Forms.ColumnHeader clAuthors;
        private System.Windows.Forms.ColumnHeader clVersion;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bPriorityUp;
        private System.Windows.Forms.Button bPriorityDown;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.Button bSaveAndPlay;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.Button bAbout;
        private System.Windows.Forms.Button bPriorityFirst;
        private System.Windows.Forms.Button bPriorityLast;
        private System.Windows.Forms.Button bRandom;
    }
}

