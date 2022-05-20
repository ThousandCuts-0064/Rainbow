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
        private readonly float[] _penBoarderDashPattern;
        private readonly IColorModel _colorModel;
        private readonly GameString _gameString;
        private readonly SolidBrush _brushFill;
        private readonly Pen _penBoarder = new Pen(_colorBorderNormal, Game.Unit);
        private readonly Controller _controller;
        private readonly GameModifiers _gameModifiers;
        private readonly Color _colorStart;
        private readonly Color _colorDistortion;
        private RectangleF _rectangleFill;
        private int _lives;
        public IReadOnlyChannel Channel { get; }
        public ColorCode ColorCode { get; }
        public int Column => Channel.Index;
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
                        Popped();
                        break;

                    case 1:
                        _penBoarder.DashStyle = DashStyle.Solid;
                        break;

                    default:
                        _penBoarderDashPattern[0] = 6f / value;
                        _penBoarder.DashPattern = _penBoarderDashPattern;
                        break;
                }
            }
        }
        public bool IsInControl { get; private set; } = true;

        public event Action Disposing;
        public event Action ControlLost;
        public event Action Popped;

        public Tile(IColorModel colorModel, ColorCode colorCode, GameModifiers gameModifiers, int column,
            int lives = 1, bool isNoClick = false, Layer layer = Layer.Gameplay) :
            base(layer)
        {
            _colorModel = colorModel;
            _gameModifiers = gameModifiers;
            ColorCode = colorCode;
            if (lives > 1) _penBoarderDashPattern = new float[2] { 0, 1 };
            Lives = lives;
            IsNoClick = isNoClick;
            Channel = Game.Channels[column];
            Location = Channel.PointSpawn;
            _controller = new Controller(this);
            _brushFill = new SolidBrush(colorModel.CodeToColor(colorCode));
            _rectangleFill = new RectangleF(Location, new SizeF(Game.TileWidth, Game.TileHeight));
            _rectangleFill.Inflate(-Game.HalfUnit, -Game.HalfUnit);


            if (gameModifiers.HasFlag(GameModifiers.ColorDistortion))
            {
                var colorMin = colorModel.CodeToColor(ColorCode.None);
                var colorMax = colorModel.CodeToColor(ColorCode.All);

                _colorDistortion = Color.FromArgb(
                    Game.Random.Next(128),
                    Game.Random.Next(128),
                    Game.Random.Next(128));

                if (colorMax.ToArgb() < colorMin.ToArgb())
                {
                    _colorDistortion = Color.FromArgb(
                        byte.MaxValue - _colorDistortion.R,
                        byte.MaxValue - _colorDistortion.G,
                        byte.MaxValue - _colorDistortion.B);
                }
            }

            if (gameModifiers.HasFlag(GameModifiers.HintButtons))
            {
                _gameString = new GameString(
                    new ColorColumn(colorCode, Column).ToInput(),
                    _rectangleFill,
                    _colorBlend,
                    Layer);
            }

            _penBoarder.Color = IsNoClick ? _colorBorderNoClick : _colorBorderNormal;
            _colorStart = _brushFill.Color;

            Popped += Dispose;
        }

        public override PointF GetCenter() => _rectangleFill.GetCenter();

        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(_brushFill, _rectangleFill);
            graphics.DrawRectangle(_penBoarder, _rectangleFill);
        }

        /// <summary>
        /// Reduces tile's lives by 1.
        /// </summary>
        public void Click()
        {
            TimesClicked++;
            Lives--;
        }

        /// <summary>
        /// Tries to get the controller of the tile
        /// </summary>
        /// <param name="controller"></param>
        /// <returns>True if the tile is in control of itself, otherwise false</returns>
        public bool TryGetController(out Controller controller)
        {
            controller = null;
            if (IsInControl == false) return false;

            IsInControl = false;
            ControlLost?.Invoke();
            controller = _controller;
            return true;
        }

        public override void Dispose()
        {
            Disposing?.Invoke();
            base.Dispose();
            _brushFill.Dispose();
            _penBoarder.Dispose();
            _gameString?.Dispose();
        }

        protected override void Update()
        {
            if (!IsInControl) return;

            _controller.Move(0, Game.TileSpeed);

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
                    Math2.Clamp((Location.Y - Channel.BoarderLeft.Point1.Y) / (Channel.Finish.Point1.Y - Channel.BoarderLeft.Point1.Y), 0, 1));
            }
        }

        public class Controller
        {
            private readonly Tile _tile;

            public Controller(Tile tile) => _tile = tile;

            public void Move(float x, float y)
            {
                _tile.Location = _tile.Location.OffsetNew(x, y);
                _tile._rectangleFill.Offset(x, y);
                if (_tile._gameString != null)
                    _tile._gameString.Rectangle = _tile._rectangleFill;
            }
        }
    }
}
