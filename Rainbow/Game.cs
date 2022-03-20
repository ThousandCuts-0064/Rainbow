using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace Rainbow
{
    public static class Game
    {
        /// <summary>
        /// % of screen height. Range: [0, 1].
        /// </summary>
        private const float unitRatio = 0.01f;
        /// <summary>
        /// Number of units
        /// </summary>
        private const int tileHightUnits = 10;
        private static readonly Color _borderColor = Color.Black;
        private static readonly Color _finishColor = Color.Black;
        private static Queue<Tile>[] _tiles;
        private static HashSet<Update> _updates;
        private static Line[] _boarders;//
        private static Line[] _finishes;//
        private static PointF[] _spawns;//
        private static IColorModel _colorModel;
        private static Timer _timer;
        private static Update _updateScreen;
        private static Tile _lastSpawned;

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

        public static HashSet<GameplayElement> GameplayElements { get; private set; }
        public static List<MapElement> MapElements { get; private set; }
        public static List<UIElement> UIElements { get; private set; }

        public static FormPlay FormPlay { get; private set; }
        public static Random Random { get; private set; } = new Random();
        public static RectangleF PlayArea { get; private set; }

        public static ulong Ticks { get; private set; } = 0;
        public static float TileSpeed { get; private set; } = 10f;
        public static float Unit { get; private set; }
        public static float TileWidth { get; private set; }
        public static float TileHeight { get; private set; }
        public static int Level { get; private set; }
        public static int Lives { get; private set; } = 10;
        public static bool IsPaused { get => !_timer.Enabled; set => _timer.Enabled = !value; }

        public static void Initialize(FormPlay formPlay, IColorModel colorModel, int level)
        {
            //Direct initializations
            Level = level;
            FormPlay = formPlay;
            _colorModel = colorModel;
            MapElements = new List<MapElement>();
            GameplayElements = new HashSet<GameplayElement>();
            UIElements = new List<UIElement>();
            _tiles = new Queue<Tile>[level];
            _updates = new HashSet<Update>();
            _boarders = new Line[level + 1];
            _finishes = new Line[level];
            _spawns = new PointF[level];
            _timer = new Timer(DeltaTime * 1000);
            _timer.Elapsed += (object sender, ElapsedEventArgs e) => GameTick();
            _updateScreen = new Update(FormPlay.Refresh);

            //Calculation
            var screen = FormPlay.ClientRectangle;
            Unit = screen.Height * unitRatio;
            TileHeight = tileHightUnits * Unit;
            TileWidth = screen.Width * PlayAreaWidthRatio / level;
            PlayArea = new RectangleF(
                new PointF(screen.Width * (1 - PlayAreaWidthRatio) / 2, 0),
                new SizeF(screen.Width * PlayAreaWidthRatio, screen.Height));

            //UIElements
            new Bar(Color.Red,
                new RectangleF(new PointF(PlayArea.Right, PlayArea.Top),
                new SizeF(TileWidth, TileHeight)));

            //Leftmost boarder
            var bottomLeft = new PointF(PlayArea.Left, PlayArea.Bottom);
            _boarders[0] = new Line(_borderColor, PlayArea.Location, bottomLeft);

            //Scale to level
            for (int i = 0; i < level; i++)
            {
                //Tile queues
                _tiles[i] = new Queue<Tile>();

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

            //First Spawn. Spawner needs one spawn to chain spawn
            Spawn();

            //Start Game
            _timer.Start();
        }

        public static void AddToUpdateCallback(Update action) => _updates.Add(action);
        public static void RemoveFromUpdateCallback(Update action) => _updates.Remove(action);

        private static void GameTick()
        {
            Ticks++;
            ManageLives();
            foreach (var update in _updates) update();
            Spawner();
            //Sometimes exceptions is thrown on closing the program
            try { FormPlay.Invoke(_updateScreen); }
            catch (ObjectDisposedException) { }
        }

        private static void ManageLives()
        {
            for (int i = 0; i < Level; i++)
            {
                if (_tiles[i].Count != 0 &&
                    _tiles[i].Peek().Location.Y > Boarders[i].Second.Y)
                {
                    _tiles[i].Dequeue().Dispose();
                    Lives--;
                    //if (Lives == 0)
                }
            }
        }

        private static void Spawner()
        {
            //Hack: Compensates for 1 pixel stuttering, background won't flicker between touching tiles in same column
            if (_lastSpawned.Location.Y >= -1) 
                Spawn();
        }

        private static void Spawn()
        {
            int spawnLocationIndex = Random.Next(Level);
            //ColorCode.0 is invalid
            int colorCode = Random.Next((int)ColorCode.All);
            _lastSpawned = new Tile(_colorModel[(ColorCode)(colorCode + 1)], SpawnLocations[spawnLocationIndex]);
            _tiles[spawnLocationIndex].Enqueue(_lastSpawned);
        }
    }
}
