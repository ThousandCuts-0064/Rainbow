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
    public partial class FormDeath : Form
    {
        private const int imageWidth = 50;
        private const int imageHeight = 50;
        private readonly Game.ReadOnlyRecord _record;

        public FormDeath(Form owner, Game.ReadOnlyRecord record)
        {
            InitializeComponent();
            _record = record;
            Owner = new FormDim() { Owner = owner };
            Owner.KeyDown += FormDeath_KeyDown;
            Owner.Show();
        }

        private void FormDeath_Load(object sender, EventArgs e)
        {
            CenterToParent();

            SetLabel(labelLevelValue, _record.Level);
            SetLabel(labelColorModelValue, _record.ColorModelName);
            SetLabel(labelTimeSpanValue, _record.TimeSpan);
            SetLabel(labelTilesClickedValue, _record.TilesClicked);
            SetLabel(labelTilesPoppedValue, _record.TilesPopped);
            SetLabel(labelTilesPassedValue, _record.TilesPassed);

            int count = ((int)_record.GameModifiers).CountBits(); // Number of game modifiers
            if (count == 0) return;
            Point[] locations = new Point[count];
            float ratio = (float)panelGameModifiers.Height / panelGameModifiers.Width;
            int rowCount = (int)Math.Ceiling(Math.Sqrt(count * ratio));
            int columnCount = (int)Math.Sqrt(count / ratio); // Math.Floor

            locations[0] = new Point(
                (int)Math.Round(panelGameModifiers.Width / 2f - columnCount * imageWidth / 2f, MidpointRounding.AwayFromZero),
                (int)Math.Round(panelGameModifiers.Height / 2f - rowCount * imageHeight / 2f, MidpointRounding.AwayFromZero));
            int inRectangleCount = (rowCount - 1) * columnCount;
            for (int i = 1; i < inRectangleCount; i++)
                locations[i] = locations[0].OffsetNew(imageWidth * (i % columnCount), imageHeight * (i / columnCount));

            int leftover = count - inRectangleCount;
            if (inRectangleCount != 0)
                locations[inRectangleCount] = new Point(
                    (int)Math.Round(panelGameModifiers.Width / 2f - leftover * imageWidth / 2f, MidpointRounding.AwayFromZero),
                    locations[inRectangleCount - 1].Y + imageHeight);
            for (int i = 1; i < leftover; i++)
                locations[inRectangleCount + i] = locations[inRectangleCount].OffsetNew(imageWidth * i, 0);

            void SetLabel(Label label, object value) => label.Text = value.ToString();
        }

        private void ButtonQuit_Click(object sender, EventArgs e) => Application.Exit();

        private void FormDeath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) new FormPause(this).Show();
        }

        private void ButtonRestart_Click(object sender, EventArgs e)
        {
            Owner.Owner.Close(); // this -> FormDim -> FormPlay
            new FormLevelSelection() { Owner = ActiveForm }.Show();
            Close();
        }

        private void FormDeath_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
                Owner.Close();
        }
    }
}
