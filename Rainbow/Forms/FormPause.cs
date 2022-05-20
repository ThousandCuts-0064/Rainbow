﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rainbow
{
    public partial class FormPause : Form
    {
        public FormPause(Form owner)
        {
            InitializeComponent();
            Owner = new FormDim() { Owner = owner };
            Owner.KeyDown += FormPause_KeyDown;
            Owner.Show();
            if (Game.IsActive) Game.IsPaused = true;
        }

        private void FormPause_Load(object sender, EventArgs e) => CenterToParent();
        private void ButtonResume_Click(object sender, EventArgs e) => Close();
        private void ButtonQuit_Click(object sender, EventArgs e) => Application.Exit();

        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            Hide();
            new FormSettings(this).Show();
        }

        private void FormPause_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
                Owner.Close();

            if (!Game.IsActive) return;
            Game.IsPaused = false;
            Game.FocusFormPlay();
        }

        private void FormPause_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
