using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rainbow
{
    public partial class FormPlay : Form
    {
        
        private Settings _settings = FormSettings.SettingsDefault;
        private PixelOffsetMode _pixelOffsetMode;
        private SmoothingMode _smoothingMode;

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
            FormSettings.SettingsChanged += settings =>
            {
                _settings = settings;
                _pixelOffsetMode = _settings.PixelOffset ? PixelOffsetMode.Half : PixelOffsetMode.None;
                _smoothingMode = _settings.AntiAliasing? SmoothingMode.AntiAlias: SmoothingMode.None;
            };
        }

        private void FormPlay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) new FormPause(this).Show();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            
            graphics.SmoothingMode = _smoothingMode;
            graphics.PixelOffsetMode = _pixelOffsetMode;
            graphics.TextRenderingHint = _settings.TextRenderingHint;

            base.OnPaint(e);
            Game.Draw(graphics);
        }
    }
}