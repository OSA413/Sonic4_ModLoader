namespace ManagerLauncher
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.bManager = new System.Windows.Forms.Button();
            this.bConf = new System.Windows.Forms.Button();
            this.bPlay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bManager
            // 
            this.bManager.Location = new System.Drawing.Point(12, 152);
            this.bManager.Name = "bManager";
            this.bManager.Size = new System.Drawing.Size(216, 64);
            this.bManager.TabIndex = 2;
            this.bManager.Text = "Launch Mod Manager";
            this.bManager.UseVisualStyleBackColor = true;
            this.bManager.Click += new System.EventHandler(this.bManagerClick);
            // 
            // bConf
            // 
            this.bConf.Location = new System.Drawing.Point(12, 82);
            this.bConf.Name = "bConf";
            this.bConf.Size = new System.Drawing.Size(216, 64);
            this.bConf.TabIndex = 1;
            this.bConf.Text = "Launch Configuration Tool";
            this.bConf.UseVisualStyleBackColor = true;
            this.bConf.Click += new System.EventHandler(this.bConfClick);
            // 
            // bPlay
            // 
            this.bPlay.Location = new System.Drawing.Point(12, 12);
            this.bPlay.Name = "bPlay";
            this.bPlay.Size = new System.Drawing.Size(216, 64);
            this.bPlay.TabIndex = 3;
            this.bPlay.Text = "Play";
            this.bPlay.UseVisualStyleBackColor = true;
            this.bPlay.Click += new System.EventHandler(this.bPlayClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 228);
            this.Controls.Add(this.bPlay);
            this.Controls.Add(this.bConf);
            this.Controls.Add(this.bManager);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manager Launcher";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button bManager;
        private System.Windows.Forms.Button bConf;
        private System.Windows.Forms.Button bPlay;
    }
}

