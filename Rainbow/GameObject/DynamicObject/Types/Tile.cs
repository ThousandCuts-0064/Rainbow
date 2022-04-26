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
        private const int FLASH_TICKS = 10;
        private const int FLASH_COOLDOWN_TICKS = 40;
        private static readonly Color _colorBlend = Color.Gray;
        private static readonly Color _colorBorderNormal = Color.Black;
        private static readonly Color _colorBorderNoClick = Color.White;
        private readonly IColorModel _colorModel;
        private readonly GameString _gameString;
        private readonly SolidBrush _brushFill;
        private readonly Pen _penBoarder = new Pen(_colorBorderNormal, Game.Unit);
        private readonly IReadOnlyChannel _channel;
        private readonly GameModifiers _gameModifiers;
        private readonly Color _colorStart;
        private readonly Color _colorDistortion;
        private readonly ulong _tickSpawned;
        private RectangleF _rectangleFill;
        private int _lives;
        public ColorCode ColorCode { get; }
        public int Column { get; }
        public bool IsNoClick { get; }
        public Color Color => _brushFill.Color;
        public PointF Location { get; private set; }
        public int TimesClicked { get; private set; }
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
        public bool IsInControl { get; set; } = true;

        public Tile(IColorModel colorModel, ColorCode colorCode, GameModifiers gameModifiers, int column,
            int lives = 1, bool isNoClick = false, Layer layer = Layer.Gameplay) :
            base(layer)
        {
            _colorModel = colorModel;
            ColorCode = colorCode;
            _gameModifiers = gameModifiers;
            Column = column;
            Lives = lives;
            IsNoClick = isNoClick;
            _tickSpawned = Game.Ticks;
            _channel = Game.Channels[column];
            Location = _channel.PointSpawn;
            _brushFill = new SolidBrush(colorModel.CodeToColor(colorCode));
            _rectangleFill = new RectangleF(Location, new SizeF(Game.TileWidth, Game.TileHeight));
            _rectangleFill.Inflate(-Game.HalfUnit, -Game.HalfUnit);


            if (gameModifiers.HasFlag(GameModifiers.ColorDistortion))
            {
                //TODO: CMY 0 is black
                _colorDistortion = Color.FromArgb(
                    Game.Random.Next(128),
                    Game.Random.Next(128),
                    Game.Random.Next(128));
            }

            if (gameModifiers.HasFlag(GameModifiers.HintButtons))
            {
                _gameString = new GameString(
                    new ColorColumn(colorCode, column).ToInput(),
                    _rectangleFill,
                    _colorBlend,
                    Layer);
            }

            _penBoarder.Color = IsNoClick ? _colorBorderNoClick : _colorBorderNormal;
            _colorStart = _brushFill.Color;
        }

        public override PointF GetCenter() => _rectangleFill.GetCenter();

        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(_brushFill, _rectangleFill);
            graphics.DrawRectangle(_penBoarder, _rectangleFill);
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
            if (!IsInControl) return;

            var offset = Game.TileUnitsPerSecond * Game.Unit * Game.DELTA_TIME;
            Location = Location.Offset(0, offset);
            _rectangleFill.Offset(0, offset);
            if (_gameString != null)
                _gameString.Rectangle = _rectangleFill;

            //Color calculations
            _brushFill.Color = _colorStart;

            if (_gameModifiers.HasFlag(GameModifiers.FlashingColors))
            {
                if (Game.Ticks % (FLASH_TICKS + FLASH_COOLDOWN_TICKS) < FLASH_COOLDOWN_TICKS)
                {
                    _brushFill.Color = _colorBlend;
                    return;
                }
            }

            if (_gameModifiers.HasFlag(GameModifiers.ColorDistortion))
            {
                _brushFill.Color = _colorModel.Combine(
                    _brushFill.Color,
                    _colorDistortion);
            }

            if (_gameModifiers.HasFlag(GameModifiers.InvertedColors))
                _brushFill.Color = _brushFill.Color.Invert();

            if (_gameModifiers.HasFlag(GameModifiers.FadingColors))
            {
                _brushFill.Color = _brushFill.Color.Blend(_colorBlend, 
                    Math2.Clamp((Location.Y - _channel.BoarderLeft.Point1.Y) / (_channel.Finish.Point1.Y - _channel.BoarderLeft.Point1.Y), 0, 1));
            }
        }
    }
}
