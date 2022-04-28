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
        private readonly Channel _channel;
        private readonly LinkedListNode<Tile> _listNode;
        private readonly IColorModel _colorModel;
        private readonly GameString _gameString;
        private readonly SolidBrush _brushFill;
        private readonly Pen _penBoarder = new Pen(_colorBorderNormal, Game.Unit);
        private readonly Controller _controller;
        private readonly GameModifiers _gameModifiers;
        private readonly Color _colorStart;
        private readonly Color _colorDistortion;
        private readonly ulong _tickSpawned;
        private RectangleF _rectangleFill;
        private int _lives;
        public IReadOnlyChannel Channel => _channel;
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
        public bool IsInControl { get; private set; } = true;
        public event Action OnDispose;

        public Tile(Channel channel, IColorModel colorModel, ColorCode colorCode, GameModifiers gameModifiers,
            int lives = 1, bool isNoClick = false, Layer layer = Layer.Gameplay) :
            base(layer)
        {
            _controller = new Controller(this);
            _colorModel = colorModel;
            _gameModifiers = gameModifiers;
            _tickSpawned = Game.Ticks;
            ColorCode = colorCode;
            Lives = lives;
            IsNoClick = isNoClick;
            _channel = channel;
            Location = Channel.PointSpawn;
            _listNode = channel.TileList.AddFirst(this);
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
                    new ColorColumn(colorCode, Column).ToInput(),
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
            _channel.TileList.Remove(_listNode);
            controller = _controller;
            return true;
        }

        public override void Dispose()
        {
            base.Dispose();
            _brushFill.Dispose();
            _penBoarder.Dispose();
            _gameString?.Dispose();
            OnDispose?.Invoke();
        }

        protected override void Update()
        {
            if (!IsInControl) return;

            _controller.Move(0, Game.TileUnitsPerSecond * Game.Unit * Game.DELTA_TIME);
            
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
                _tile.Location = _tile.Location.Offset(x, y);
                _tile._rectangleFill.Offset(x, y);
                if (_tile._gameString != null)
                    _tile._gameString.Rectangle = _tile._rectangleFill;
            }
        }
    }
}
