﻿using System;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    public partial class Suspicious : Form
    {
        public Suspicious(string[] args)
        {
            InitializeComponent();
            foreach (string file in args)
            {
                listView1.Items.Add(file.Substring(13));
            }
            listView1.Columns[0].Width = -2;
        }

        private void bContinue_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
