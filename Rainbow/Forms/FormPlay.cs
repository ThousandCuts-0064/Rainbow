using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rainbow
{
    public partial class FormPlay : Form
    {
        public FormPlay(IColorModel colorModel, GameModifiers gameModifiers, int level)
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint, 
                true);
            UpdateStyles();

            Height = Screen.PrimaryScreen.Bounds.Height;
            Width = Screen.PrimaryScreen.Bounds.Width;
            Game.Initialize(this, colorModel, gameModifiers, level);
        }

        private void FormPlay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) new FormPause(this).Show();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            base.OnPaint(e);
            foreach (var item in Game.MapElements) item.Draw(g);
            foreach (var item in Game.GameplayElements) item.Draw(g);
            foreach (var item in Game.UIElements) item.Draw(g);
        }
    }
}
