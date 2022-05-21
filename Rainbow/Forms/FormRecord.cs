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
    public partial class FormRecord : Form
    {
        private const string NONE = "|||||";
        private const string BACK = "Back";
        private const string QUIT = "Quit";
        private readonly Game.ReadOnlyRecord _record;
        private readonly int _imageWidth;
        private readonly int _imageHeight;
        private readonly bool _isGameOver;

        public FormRecord(Form owner, Game.ReadOnlyRecord record, bool isGameOver)
        {
            InitializeComponent();
            _imageWidth = labelLevelText.Size.Width / 2;
            _imageHeight = labelLevelText.Size.Height / 2;
            _record = record;
            _isGameOver = isGameOver;
            buttonEscape.Text = isGameOver ? QUIT : BACK;
            Owner = new FormDim() { Owner = owner };
            Owner.KeyDown += FormDeath_KeyDown;
            if (isGameOver) Owner.Show(); // Ownership chain must be preserved even if not shown
        }

        private void FormDeath_Load(object sender, EventArgs e)
        {
            CenterToParent();
            LoadRecord(_record);
        }

        private void ButtonEscape_Click(object sender, EventArgs e)
        {
            if (_isGameOver) Application.Exit();
            else
            {
                Owner.Owner.Show();
                Close();
            }
        }

        private void FormDeath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                if (_isGameOver)
                    new FormPause(this, false).Show();
                else
                {
                    Owner.Owner.Show();
                    Close();
                }
        }

        private void ButtonRestart_Click(object sender, EventArgs e)
        {
            Owner.Owner.Close(); // Owner from constructor
            FormMain.Reload();
        }

        private void FormDeath_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
                Owner.Close();
        }


        private void ButtonViewAllRecords_Click(object sender, EventArgs e)
        {
            var viewer = new FormRecordsViewer();
            viewer.Selected += LoadRecord;
            viewer.Deactivate += (sender1, e1) => viewer.Close();
            viewer.Show();
        }

        private void LoadRecord(Game.ReadOnlyRecord record)
        {
            panelGameModifiers.Controls.Clear();

            if (record == null)
            {
                SetLabel(labelTimeSpanValue, NONE);
                SetLabel(labelLevelValue, NONE);
                SetLabel(labelColorModelValue, NONE);
                SetLabel(labelTilesClickedValue, NONE);
                SetLabel(labelTilesPoppedValue, NONE);
                SetLabel(labelTilesPassedValue, NONE);
                return;
            }

            string timeStr = record.TimeSpan.ToString("G").TrimEnd(4);
            timeStr = timeStr.Insert(timeStr.LastIndexOf(':') + 1, "\n"); // String is too long for 1 line

            SetLabel(labelTimeSpanValue, timeStr);
            SetLabel(labelLevelValue, record.Level);
            SetLabel(labelColorModelValue, record.ColorModelName);
            SetLabel(labelTilesClickedValue, record.TilesClicked);
            SetLabel(labelTilesPoppedValue, record.TilesPopped);
            SetLabel(labelTilesPassedValue, record.TilesPassed);

            GameModifiers[] modifiers = ((int)record.GameModifiers).AddBitsToList(new List<int>()).Cast<GameModifiers>().ToArray();
            int count = modifiers.Length;
            if (count == 0) return;
            Point[] locations = new Point[count];
            float ratio = (float)panelGameModifiers.Height / panelGameModifiers.Width;
            int rowCount = (int)Math.Ceiling(Math.Sqrt(count * ratio));
            int columnCount = (int)Math.Sqrt(count / ratio);

            locations[0] = new Point(
                (int)Math.Round(panelGameModifiers.Width / 2f - columnCount * _imageWidth / 2f, MidpointRounding.AwayFromZero),
                (int)Math.Round(panelGameModifiers.Height / 2f - rowCount * _imageHeight / 2f, MidpointRounding.AwayFromZero));
            int inRectangleCount = (rowCount - 1) * columnCount;
            for (int i = 1; i < inRectangleCount; i++)
                locations[i] = locations[0].OffsetNew(_imageWidth * (i % columnCount), _imageHeight * (i / columnCount));

            int leftover = count - inRectangleCount;
            if (inRectangleCount != 0 && leftover != 0)
                locations[inRectangleCount] = new Point(
                    (int)Math.Round(panelGameModifiers.Width / 2f - leftover * _imageWidth / 2f, MidpointRounding.AwayFromZero),
                    locations[inRectangleCount - 1].Y + _imageHeight);
            for (int i = 1; i < leftover; i++)
                locations[inRectangleCount + i] = locations[inRectangleCount].OffsetNew(_imageWidth * i, 0);

            string[] names = new string[count];
            Font[] fonts = new Font[count];
            for (int i = 0; i < count; i++)
            {
                names[i] = modifiers[i].ToShortString();
                fonts[i] = new Font(
                    FontFamily.GenericMonospace,
                    Math.Min(_imageWidth * 0.9f / names[i].Length, _imageWidth * 0.5f));
            }

            var format = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var brush = new SolidBrush(Color.Black);

            for (int i = 0; i < count; i++)
            {
                Bitmap bitmap = new Bitmap(_imageWidth, _imageHeight);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                    graphics.DrawString(names[i], fonts[i], brush, new RectangleF(new PointF(), bitmap.Size), format);
                }
                panelGameModifiers.Controls.Add(new Panel()
                {
                    Location = locations[i],
                    Size = new Size(_imageWidth, _imageHeight),
                    BackgroundImage = bitmap,
                });
            }

            void SetLabel(Label label, object value) => label.Text = value.ToString();
        }
    }
}
