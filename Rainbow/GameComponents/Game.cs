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
        private static readonly Color _borderColor = Color.Black;
        private static readonly Color _finishColor = Color.Black;
        private static LinkedList<Tile>[] _tileLists;
        private static HashSet<Update> _updates;
        private static Line[] _boarders;//
        private static Line[] _finishes;//
        private static PointF[] _spawns;//

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

            //Leftmost boarder
            var bottomLeft = new PointF(PlayArea.Left, PlayArea.Bottom);
            _boarders[0] = new Line(_borderColor, PlayArea.Location, bottomLeft);

            //Level scale
            for (int i = 0; i < level; i++)
            {
                //Tile queues
                _tileLists[i] = new LinkedList<Tile>();

                //Spawn locations
                _spawns[i] = new PointF(PlayArea.Location.X + TileWidth * i, -TileHeight);

                //Rest of boarders and finish lines
                var tileOffset = new SizeF(TileWidth * (i + 1), 0);
                var finishOffset = new SizeF(0, -TileHeight * 2);

                //Lines are automaticaly added to the draw list
                _boarders[i + 1] = new Line(_borderColor,
                    PlayArea.Location + tileOffset,
                    bottomLeft + tileOffset);
                _finishes[i] = new Line(_finishColor,
                    bottomLeft + finishOffset,
                    bottomLeft + finishOffset + tileOffset);
            }

            //Dependant object creation
            _stats = new Stats(_tileLists, colorModel, level); //Depends on UIElements, Calculation, Boarders
            _spawner = new Spawner(_tileLists, colorModel, gameModifiers, level); // Depends on SpawnLocations, Random
            try
            {
                _colorDiagram = gameModifiers.HasFlag(GameModifiers.ColorWheel)
                    ? new UIImage(
                        Image.FromFile(
                            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName +
                            @"\Resources\ColorWheel.png"),
                        new RectangleF(
                            new PointF(
                                Boarders.Last().Second.X,
                                Boarders.Last().Second.Y - TileHeight * 3),
                            new SizeF(
                                UIElementWidth,
                                TileHeight * 3)))
                    : null;
            }
            catch { }

            //Events
            _inputManager.ColorInput += OnColorInput;
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
            _formPlay.Refresh();
        }

        private static void OnColorInput(ColorCode colorCode, int column)
        {
            var tileList = _tileLists[column];
            if (tileList.Count == 0) return;

            var firstTile = tileList.Last.Value;
            while (firstTile.Location.Y + TileHeight > Finishes[column].First.Y)
            {
                if (firstTile.ColorCode != colorCode)
                {
                    firstTile = tileList.Last.Previous.Value;
                    if (firstTile == null) return;
                    continue;
                }

                firstTile.Click();
                if (firstTile.Lives <= 0)
                    tileList.RemoveLast();
                firstTile = tileList.Last.Value;
                if (firstTile == null) return;
            }
        }
    }
}