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
    public partial class FormMain : Form
    {
        private static FormMain _instance;

        public FormMain()
        {
            InitializeComponent();
            _instance = this;
        }

        public static void Reload() => _instance.FormMain_Load(null, null);

        private void FormMain_Load(object sender, EventArgs e)
        {
            OwnedForms.FirstOrDefault(f => f.GetType() == typeof(FormLevelSelection))?.Close(); // Last LevelSelection may not be closed
            var form = new FormLevelSelection() { Owner = this };
            //Pause can be called from there
            form.KeyDown += FormMain_KeyDown;
            form.Show();
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) new FormPause(this).Show();
        }

        private void FormMain_Enter(object sender, EventArgs e)
        {
            if (Game.IsActive) Game.FocusFormPlay();
        }
    }
}
