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

        public FormPlay(int level)
        {
            InitializeComponent();
            _level = level;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
        
        private void FormPlay_Load(object sender, EventArgs e)
        {
            //Manager is initialized here because the ClientRectangle isn't updated in the constructor
            Game.Initialize(this, new RGB(), _level);
        }

        private void FormPlay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) new FormPause(this).Show();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            base.OnPaint(e);

            foreach (var item in Game.GameplayElements) item.Draw(e.Graphics);
            foreach (var item in Game.MapElements) item.Draw(e.Graphics);
            foreach (var item in Game.UIElements) item.Draw(e.Graphics);
        }
    }
}
