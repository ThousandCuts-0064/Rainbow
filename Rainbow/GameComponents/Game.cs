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
        private static readonly Color _colorFinish = Color.Black;
        private static LinkedList<Tile>[] _tileLists;
        private static HashSet<Update> _updates;
        private static Line[] _boarders;//
        private static Line[] _finishes;//
        private static PointF[] _spawns;//

        private static Channel[] _channels;
        private static Line _boarderLeft;
        private static Line _boarderRight;

        private static Timer _timer;
        private static Stats _stats;
        private static Spawner _spawner;
        private static InputManager _inputManager;
        private static FormPlay _formPlay;
        private static UIImage _colorDiagram;
        private static IColorModel _colorModel;
        private static GameModifiers _gameModifiers;
        private static int _level;


        /// <summary>
        /// Time interval between ticks in seconds
        /// </summary>
        public static float DeltaTime => 0.016f;
        /// <summary>
        /// % of screen width. Range: [0, 1].
        /// </summary>
        public static float PlayAreaWidthRatio => 0.6f;
        /// <summary>
        /// % of screen width. Range: [0, 1].
        /// </summary>
        public static float MapLineWidthRatio => 0.002f;
        public static float MapLineWidth { get; private set; }

        public static IReadOnlyList<PointF> SpawnLocations => _spawns;
        public static IReadOnlyList<ILine> Boarders => _boarders;
        public static IReadOnlyList<ILine> Finishes => _finishes;
        public static Random Random { get; } = new Random();

        public static HashSet<GameplayElement> GameplayElements { get; private set; }
        public static List<MapElement> MapElements { get; private set; }
        public static List<UIElement> UIElements { get; private set; }
        public static RectangleF PlayArea { get; private set; }

        public static ulong Ticks { get; private set; } = 0;
        public static float TileUnitsPerSecond { get; private set; } = 10f;
        public static float Unit { get; private set; }
        public static float HalfUnit { get; private set; }
        public static float UIElementWidth { get; private set; }
        public static float TileWidth { get; private set; }
        public static float TileHeight { get; private set; }
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
            _tileLists = new LinkedList<Tile>[level];
            _updates = new HashSet<Update>();
            _boarders = new Line[level + 1];
            _finishes = new Line[level];
            _spawns = new PointF[level];
            MapElements = new List<MapElement>();
            GameplayElements = new HashSet<GameplayElement>();
            UIElements = new List<UIElement>();
            _timer = new Timer { Interval = (int)(DeltaTime * 1000) };
            _inputManager = new InputManager(level);

            //Calculation
            var screen = formPlay.ClientRectangle;
            Unit = screen.Height * UNIT_HIGHT_RATIO;
            HalfUnit = Unit / 2;
            TileHeight = TILE_HIGHT_UNITS * Unit;
            TileWidth = screen.Width * PlayAreaWidthRatio / level;
            UIElementWidth = screen.Width * (1 - PlayAreaWidthRatio) / 2;
            PlayArea = new RectangleF(
                new PointF(screen.Width * (1 - PlayAreaWidthRatio) / 2, 0),
                new SizeF(screen.Width * PlayAreaWidthRatio, screen.Height));

            //New
            MapLineWidth = screen.Width * MapLineWidthRatio;

            _boarderLeft = new Line(_colorBoarder, 
                new PointF(
                    PlayArea.Left - MapLineWidth / 2, 
                    PlayArea.Top), 
                new PointF(
                    PlayArea.Left - MapLineWidth / 2,
                    PlayArea.Bottom),
                MapLineWidth);

            _boarderRight = new Line(_colorBoarder,
                 new PointF(
                    PlayArea.Right - MapLineWidth / 2,
                    PlayArea.Top),
                new PointF(
                    PlayArea.Right - MapLineWidth / 2,
                    PlayArea.Bottom),
                MapLineWidth);

            _channels = new Channel[level];
            for (int i = 0; i < level; i++)
            {
                var rectangleF = new RectangleF(
                    PlayArea.Location, 
                    new SizeF(TileWidth, PlayArea.Height));

                rectangleF.Offset(TileWidth * i, 0);
                _channels[i] = new Channel(rectangleF, MapLineWidth);
            }

            //Leftmost boarder
            var bottomLeft = new PointF(PlayArea.Left, PlayArea.Bottom);
            _boarders[0] = new Line(_colorBoarder, PlayArea.Location, bottomLeft, HalfUnit);

            //Level scale
            for (int i = 0; i < level; i++)
            {
                //Tile lists
                _tileLists[i] = new LinkedList<Tile>();

                //Spawn locations
                _spawns[i] = new PointF(PlayArea.Location.X + TileWidth * i, -TileHeight);

                //Rest of boarders and finish lines
                var tileOffset = new SizeF(TileWidth * (i + 1), 0);
                var finishOffset = new SizeF(0, -TileHeight * 2);

                //Lines are automaticaly added to the draw list
                _boarders[i + 1] = new Line(_colorBoarder,
                    PlayArea.Location + tileOffset,
                    bottomLeft + tileOffset,
                    HalfUnit);
                _finishes[i] = new Line(_colorFinish,
                    bottomLeft + finishOffset,
                    bottomLeft + finishOffset + tileOffset,
                    HalfUnit);
            }

            //Dependant object creation
            _stats = new Stats(_tileLists, colorModel, level); //Depends on UIElements, Calculation, Boarders
            _spawner = new Spawner(_tileLists, colorModel, gameModifiers, level); // Depends on SpawnLocations, Random
            if (gameModifiers.HasFlag(GameModifiers.ColorWheel))
                _colorDiagram = new UIImage(
                    Resources.ColorWheel,
                    CalculateColorWheelRectangle());

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
        public static void AddToUpdateCallback(Update action) => _updates.Add(action);
        public static void RemoveFromUpdateCallback(Update action) => _updates.Remove(action);

        private static void GameTick(object sender, EventArgs e)
        {
            Ticks++;
            _stats.OnTick();
            _inputManager.OnTick();
            foreach (var update in _updates) update();
            _spawner.OnTick();
            _formPlay.Invalidate();
        }

        private static RectangleF CalculateColorWheelRectangle() =>
            new RectangleF(
                new PointF(
                    Boarders.Last().Point2.X,
                    Boarders.Last().Point2.Y - TileHeight * 3),
                new SizeF(
                    UIElementWidth,
                    TileHeight * 3));
    }
}