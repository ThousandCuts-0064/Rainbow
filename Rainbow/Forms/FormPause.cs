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
            Manager.IsPaused = true;
        }

        private void FormPause_Load(object sender, EventArgs e) => CenterToParent();
        private void buttonResume_Click(object sender, EventArgs e) => _formDim.Close();
        private void buttonQuit_Click(object sender, EventArgs e) => Application.Exit();
        private void FormPause_FormClosed(object sender, FormClosedEventArgs e) => Manager.IsPaused = false;

        private void FormPause_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) _formDim.Close();
        }

    }
}
