using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Rainbow
{
    public class Tile : GameplayElement
    {
        private static readonly StringFormat _stringFormat = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        private static readonly SolidBrush _brushText = new SolidBrush(Color.Gray);
        private static readonly Color _colorBlend = Color.Gray;
        private static readonly Color _colorBorderNormal = Color.Black;
        private static readonly Color _colorBorderNoClick = Color.White;
        private readonly SolidBrush _solidBrush;
        private readonly Font _font;
        private readonly Pen _pen = new Pen(_colorBorderNormal, Game.Unit);
        private readonly GameModifiers _gameModifiers;
        private readonly IColorModel _colorModel;
        private readonly Color _colorDistortion;
        private readonly string _text;
        private readonly int _column;
        private float _fadeRatio;
        private int _lives;
        public ColorCode ColorCode { get; }
        public int TimesClicked { get; private set; }
        public bool IsNoClick { get; }
        public Color Color => _solidBrush.Color;
        public int Lives
        {
            get => _lives;
            private set
            {
                _lives = value;
                switch (value)
                {
                    case 0:
                        Dispose();
                        break;

                    case 1:
                        _pen.DashStyle = DashStyle.Solid;
                        break;

                    case 2:
                        _pen.DashStyle = DashStyle.Dash;
                        break;

                    case 3:
                        _pen.DashStyle = DashStyle.Dot;
                        break;
                }
            }
        }

        public Tile(IColorModel colorModel, ColorCode colorCode, GameModifiers gameModifiers, int column, int lives = 1, bool isNoClick = false)
        {
            _colorModel = colorModel;
            ColorCode = colorCode;
            _gameModifiers = gameModifiers;
            _column = column;
            Lives = lives;
            IsNoClick = isNoClick;
            _solidBrush = new SolidBrush(colorModel.CodeToColor(colorCode));
            Location = Game.Channels[column].PointSpawn;

            _fadeRatio = 0;
            _colorDistortion = Color.FromArgb(
                Game.Random.Next(64),
                Game.Random.Next(64),
                Game.Random.Next(64));

            if (gameModifiers.HasFlag(GameModifiers.HintButtons))
            {
                _text += new ColorColumn(colorCode, column).ToInput();
                _font = new Font(
                    FontFamily.GenericMonospace, // Chars are same width and calculations are easy
                    Math.Min(Game.TileHeight * 0.5f, (Game.TileWidth - Game.Unit) / _text.Length));
            }
        }

        public override PointF GetCenter() =>
            new PointF(
                Location.X + Game.TileWidth * 0.5f,
                Location.Y + Game.TileHeight * 0.5f);

        public override void Draw(Graphics graphics)
        {
            Color colorBase = _solidBrush.Color;

            if (_gameModifiers.HasFlag(GameModifiers.ColorDistortion))
            {
                _solidBrush.Color = _colorModel.Combine(
                    _solidBrush.Color,
                    _colorDistortion);
            }

            if (_gameModifiers.HasFlag(GameModifiers.FadingColors))
            {
                _solidBrush.Color = _solidBrush.Color.Blend(_colorBlend, _fadeRatio);
                _fadeRatio += _fadeRatio < 1 ? 0.00175f : 0;
                //_brushText.Color = _colorBlend;
            }

            if (_gameModifiers.HasFlag(GameModifiers.InvertedColors))
                _solidBrush.Color = _solidBrush.Color.Invert();

            _pen.Color = IsNoClick ? _colorBorderNoClick : _colorBorderNormal;
            //Hack: Testing tile boarder
            //_pen.Color = _colorBorderNoClick; 

            RectangleF rectangleF = new RectangleF(Location.X, Location.Y, Game.TileWidth, Game.TileHeight);
            rectangleF.Inflate(-Game.HalfUnit, -Game.HalfUnit);

            graphics.FillRectangle(_solidBrush, rectangleF);
            graphics.DrawRectangle(_pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
            graphics.DrawString(_text, _font, _brushText, rectangleF.X + rectangleF.Width * 0.5f, rectangleF.Y + rectangleF.Height * 0.5f, _stringFormat);

            _solidBrush.Color = colorBase;
        }

        /// <summary>
        /// Reduces tile's lives with 1.
        /// </summary>
        public void Click()
        {
            TimesClicked++;
            Lives--;
        }

        public override void Dispose()
        {
            base.Dispose();
            _solidBrush.Dispose();
            _pen.Dispose();
            _font?.Dispose();
        }

        protected override void Update() => Location = new PointF(Location.X,
            Location.Y + Game.TileUnitsPerSecond * Game.Unit * Game.DeltaTime);
    }
}
