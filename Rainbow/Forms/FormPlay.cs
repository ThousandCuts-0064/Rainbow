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
        private readonly int _level;

        public FormPlay(int level)
        {
            InitializeComponent();
            _level = level;
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint, 
                true);
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
            var g = e.Graphics;

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            base.OnPaint(e);
            foreach (var item in Game.GameplayElements) item.Draw(g);
            foreach (var item in Game.MapElements) item.Draw(g);
            foreach (var item in Game.UIElements) item.Draw(g);
        }
    }
}
