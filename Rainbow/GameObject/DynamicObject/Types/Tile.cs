using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Rainbow
{
    public class Tile : DynamicObject
    {
        private static readonly Color _colorBlend = Color.Gray;
        private static readonly Color _colorBorderNormal = Color.Black;
        private static readonly Color _colorBorderNoClick = Color.White;
        private readonly IColorModel _colorModel;
        private readonly GameString _gameString;
        private readonly SolidBrush _brushFill;
        private readonly Pen _penBoarder = new Pen(_colorBorderNormal, Game.Unit);
        private RectangleF _rectangleFill;
        private readonly GameModifiers _gameModifiers;
        private readonly Color _colorDistortion;
        private readonly int _column;
        private float _fadeRatio;
        private int _lives;
        public ColorCode ColorCode { get; }
        public int TimesClicked { get; private set; }
        public bool IsNoClick { get; }
        public Color Color => _brushFill.Color;
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
                        _penBoarder.DashStyle = DashStyle.Solid;
                        break;

                    case 2:
                        _penBoarder.DashStyle = DashStyle.Dash;
                        break;

                    case 3:
                        _penBoarder.DashStyle = DashStyle.Dot;
                        break;
                }
            }
        }

        public Tile(IColorModel colorModel, ColorCode colorCode, GameModifiers gameModifiers, int column, 
            int lives = 1, bool isNoClick = false, Layer layer = Layer.Gameplay) : 
            base(layer)
        {
            _colorModel = colorModel;
            ColorCode = colorCode;
            _gameModifiers = gameModifiers;
            _column = column;
            Lives = lives;
            IsNoClick = isNoClick;
            _brushFill = new SolidBrush(colorModel.CodeToColor(colorCode));
            Location = Game.Channels[column].PointSpawn;
            _rectangleFill = new RectangleF(Location, new SizeF(Game.TileWidth, Game.TileHeight));
            _rectangleFill.Inflate(-Game.HalfUnit, -Game.HalfUnit);

            _fadeRatio = 0;
            _colorDistortion = Color.FromArgb(
                Game.Random.Next(64),
                Game.Random.Next(64),
                Game.Random.Next(64));

            if (gameModifiers.HasFlag(GameModifiers.HintButtons))
            {
                _gameString = new GameString(
                    new ColorColumn(colorCode, column).ToInput(),
                    _rectangleFill,
                    _colorBlend,
                    Layer.UI);
            }
        }

        public override PointF GetCenter() =>
            new PointF(
                Location.X + Game.TileWidth * 0.5f,
                Location.Y + Game.TileHeight * 0.5f);

        public override void Draw(Graphics graphics)
        {
            Color colorBase = _brushFill.Color;

            if (_gameModifiers.HasFlag(GameModifiers.ColorDistortion))
            {
                _brushFill.Color = _colorModel.Combine(
                    _brushFill.Color,
                    _colorDistortion);
            }

            if (_gameModifiers.HasFlag(GameModifiers.FadingColors))
            {
                _brushFill.Color = _brushFill.Color.Blend(_colorBlend, _fadeRatio);
                _fadeRatio += _fadeRatio < 1 ? 0.00175f : 0;
            }

            if (_gameModifiers.HasFlag(GameModifiers.InvertedColors))
                _brushFill.Color = _brushFill.Color.Invert();

            _penBoarder.Color = IsNoClick ? _colorBorderNoClick : _colorBorderNormal;
            //Hack: Testing tile boarder
            //_pen.Color = _colorBorderNoClick; 

            graphics.FillRectangle(_brushFill, _rectangleFill);
            graphics.DrawRectangle(_penBoarder, _rectangleFill.X, _rectangleFill.Y, _rectangleFill.Width, _rectangleFill.Height);
            
            _brushFill.Color = colorBase;
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
            _brushFill.Dispose();
            _penBoarder.Dispose();
            _gameString?.Dispose();
        }

        protected override void Update()
        {
            var offset = Game.TileUnitsPerSecond * Game.Unit * Game.DeltaTime;
            Location = new PointF(Location.X, Location.Y + offset);
            _rectangleFill.Offset(0, offset);
            if (_gameString != null) 
                _gameString.Rectangle = _rectangleFill;
        }
    }
}
