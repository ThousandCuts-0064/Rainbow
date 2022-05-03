using System;
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
        private readonly FormDim _formDim;

        public FormPause(Form owner)
        {
            InitializeComponent();
            _formDim = new FormDim();
            _formDim.KeyDown += FormPause_KeyDown;
            _formDim.Owner = owner;
            _formDim.Show();
            Owner = _formDim;
            if (Game.IsLoaded) Game.IsPaused = true;
        }

        private void FormPause_Load(object sender, EventArgs e) => CenterToParent();
        private void ButtonResume_Click(object sender, EventArgs e) => _formDim.Close();
        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            Hide();
            new FormSettings(this).Show();
        }
        private void ButtonQuit_Click(object sender, EventArgs e) => Application.Exit();
        private void FormPause_FormClosed(object sender, FormClosedEventArgs e)
        {
            Owner = null;
            _formDim.Close();
            if (!Game.IsLoaded) return;
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
