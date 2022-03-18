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
    public partial class FormPlay : Form
    {
        private readonly int _level;
        public static FormPlay Get { get; private set; }

        public FormPlay(int level)
        {
            InitializeComponent();
            _level = level;
            Get = this;
        }

        private void FormPlay_Load(object sender, EventArgs e)
        {
            Manager.Initialize(_level, ClientRectangle);
        }

        private void FormPlay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) new FormPause(this).Show();
        }

        private void FormPlay_Paint(object sender, PaintEventArgs e)
        {
            foreach (var item in Manager.GameplayElements) item.Draw(e.Graphics);
            foreach (var item in Manager.MapElements) item.Draw(e.Graphics);
            foreach (var item in Manager.UIElements) item.Draw(e.Graphics);
        }
    }
}
