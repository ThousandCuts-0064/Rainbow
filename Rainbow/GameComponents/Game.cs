using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Rainbow
{
    public static class Game
    {
        /// <summary>
        /// % of screen height. Range: [0, 1].
        /// </summary>
        private const float UNIT_HIGHT_RATIO = 0.01f;
        /// <summary>
        /// Number of units
        /// </summary>
        private const int TILE_HIGHT_UNITS = 10;
        private static readonly Color _colorBoarder = Color.Black;
        private static Dictionary<Layer, List<IDrawable>> _layerToList;
        private static HashSet<Update> _updates;
        private static Channel[] _channels;
        private static Timer _timer;
        private static Stats _stats;
        private static Spawner _spawner;
        private static InputManager _inputManager;
        private static Line _boarderLeft;
        private static Line _boarderRight;
        private static FormPlay _formPlay;
        private static GameImage _colorDiagram;
        private static IColorModel _colorModel;
        private static GameModifiers _gameModifiers;
        private static int _level;
        /// <summary>
        /// Time interval between ticks in seconds
        /// </summary>
        public const float DELTA_TIME = 0.016f;
        /// <summary>
        /// % of screen width. Range: [0, 1].
        /// </summary>
        public const float PLAY_AREA_WIDTH_RATIO = 0.6f;
        /// <summary>
        /// % of screen width. Range: [0, 1].
        /// </summary>
        public const float MAP_LINE_WIDTH_RATIO = 0.002f;
        public static IReadOnlyList<IReadOnlyChannel> Channels => _channels;
        public static IReadOnlyLine BoarderRight => _boarderRight;
        public static IReadOnlyLine BoarderLeft => _boarderLeft;
        public static Random Random { get; } = new Random();
        public static RectangleF PlayArea { get; private set; }
        public static ulong Ticks { get; private set; } = 0;
        public static float Unit { get; private set; }
        public static float HalfUnit { get; private set; }
        public static float MapLineWidth { get; private set; }
        public static float ChannelWidth { get; private set; }
        public static float UIElementWidth { get; private set; }
        public static float TileWidth { get; private set; }
        public static float TileHeight { get; private set; }
        public static int TileUnitsPerSecond { get; private set; } = 10;
        public static bool IsPaused { get => !_timer.Enabled; set => _timer.Enabled = !value; }
        public static bool IsLoaded { get; private set; }

        public static void Initialize(FormPlay formPlay, IColorModel colorModel, GameModifiers gameModifiers, int level)
        {
            //Direct initializations
            _formPlay = formPlay;
            _colorModel = colorModel;
            _gameModifiers = gameModifiers;
            _level = level;
            IsLoaded = true;

            //Direct object Creation
            _updates = new HashSet<Update>();
            _timer = new Timer { Interval = (int)(DELTA_TIME * 1000) };
            _inputManager = new InputManager(level);
            _layerToList = new Dictionary<Layer, List<IDrawable>>();
            for (int i = 0; Enum.IsDefined(typeof(Layer), i); i++)
                _layerToList.Add((Layer)i, new List<IDrawable>());


            //Calculation
            var screen = formPlay.ClientRectangle;
            Unit = screen.Height * UNIT_HIGHT_RATIO;
            HalfUnit = Unit / 2;
            ChannelWidth = screen.Width * PLAY_AREA_WIDTH_RATIO / level;
            MapLineWidth = screen.Width * MAP_LINE_WIDTH_RATIO;
            UIElementWidth = screen.Width * (1 - PLAY_AREA_WIDTH_RATIO) / 2 - MapLineWidth;
            TileHeight = TILE_HIGHT_UNITS * Unit;
            TileWidth = ChannelWidth - MapLineWidth * 2;
            PlayArea = new RectangleF(
                new PointF(screen.Width * (1 - PLAY_AREA_WIDTH_RATIO) / 2, 0),
                new SizeF(screen.Width * PLAY_AREA_WIDTH_RATIO, screen.Height));

            //Boarders
            _boarderLeft = new Line(_colorBoarder,
                new PointF(
                    PlayArea.Left - MapLineWidth / 2,
                    PlayArea.Top),
                new PointF(
                    PlayArea.Left - MapLineWidth / 2,
                    PlayArea.Bottom),
                Layer.Map,
                MapLineWidth);

            _boarderRight = new Line(_colorBoarder,
                 new PointF(
                    PlayArea.Right + MapLineWidth / 2,
                    PlayArea.Top),
                new PointF(
                    PlayArea.Right + MapLineWidth / 2,
                    PlayArea.Bottom),
                Layer.Map,
                MapLineWidth);

            //Channels
            _channels = new Channel[level];
            for (int i = 0; i < level; i++)
            {
                var rectangleF = new RectangleF(
                    PlayArea.Location,
                    new SizeF(ChannelWidth, PlayArea.Height));

                rectangleF.Offset(ChannelWidth * i, 0);
                _channels[i] = new Channel(rectangleF, gameModifiers, MapLineWidth, i);
            }

            //Dependant object creation
            _stats = new Stats(_channels, colorModel, level);
            _spawner = new Spawner(_channels, colorModel, gameModifiers, level);
            if (gameModifiers.HasFlag(GameModifiers.ColorWheel))
                _colorDiagram = new GameImage(
                    Resources.ColorWheel,
                    CalculateColorWheelRectangle(),
                    Layer.UI);

            //Events
            _inputManager.ColorInput += _stats.ColorInput;
            _inputManager.Shotgun += _stats.UseShotgun;
            _timer.Tick += GameTick;
            formPlay.KeyDown += _inputManager.OnKeyDown;
            formPlay.KeyUp += _inputManager.OnKeyUp;

            //Start Game
            _timer.Start();
        }

        public static void FocusFormPlay() => _formPlay.Focus();
        public static void AddToDrawList(GameObject gameObject) =>
            _layerToList[gameObject.Layer].Add(gameObject);
        public static void RemoveFromDrawList(GameObject gameObject) =>
            _layerToList[gameObject.Layer].Remove(gameObject);
        public static void AddToUpdateCallback(Update action) => _updates.Add(action);
        public static void RemoveFromUpdateCallback(Update action) => _updates.Remove(action);

        public static void Draw(Graphics graphics)
        {
            foreach (var layer in _layerToList.Values)
                foreach (var item in layer)
                    item.Draw(graphics);
        }

        private static void GameTick(object sender, EventArgs e)
        {
            Ticks++;
            _inputManager.OnTick();
            foreach (var update in _updates) update();
            _stats.OnTick();
            _spawner.OnTick();
            _formPlay.Invalidate();
        }

        private static RectangleF CalculateColorWheelRectangle() =>
            new RectangleF(
                new PointF(
                    _boarderRight.Point2.X + MapLineWidth / 2,
                    _boarderRight.Point2.Y + MapLineWidth / 2 - TileHeight * 3),
                new SizeF(
                    UIElementWidth,
                    TileHeight * 3));
    }
}