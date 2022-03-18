using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace Rainbow
{
    public static class Manager
    {
        private static readonly Color _borderColor = Color.Black;
        private static readonly Color _finishColor = Color.Black;
        private static Queue<Tile>[] _tiles;
        private static HashSet<Update> _updates;
        private static Line[] _boarders;//
        private static Line[] _finishes;//
        private static PointF[] _spawns;//
        private static Timer _timer;

        /// <summary>
        /// Time interval between ticks in seconds
        /// </summary>
        public static float DeltaTime => 0.016f;
        /// <summary>
        /// % of screen width. Range: [0, 1].
        /// </summary>
        public static float PlayAreaWidthRatio => 0.6f;
        /// <summary>
        /// % of screen height. Range: [0, 1].
        /// </summary>
        public static float TileHeightRatio => 0.1f;

        public static IReadOnlyList<PointF> SpawnLocations => _spawns;
        public static IReadOnlyList<ILine> Boarders => _boarders;
        public static IReadOnlyList<ILine> Finishes => _finishes;

        public static HashSet<GameplayElement> GameplayElements { get; private set; }
        public static List<MapElement> MapElements { get; private set; }
        public static List<UIElement> UIElements { get; private set; }

        public static Random Random { get; private set; } = new Random();
        public static RectangleF PlayArea { get; private set; }

        public static ulong GameTicks { get; private set; } = 0;
        public static float TileWidth { get; private set; }
        public static float TileHeight { get; private set; }
        public static float GameSpeed { get; private set; } = 1;
        public static int Level { get; private set; }
        public static int Lives { get; private set; } = 10;
        public static bool IsPaused { get => !_timer.Enabled; set => _timer.Enabled = !value; }

        /// <summary>
        /// Initializes manager properties.
        /// </summary>
        /// <param name="level"> Game level </param>
        /// <param name="screen"> Screen size </param>
        public static void Initialize(int level, Rectangle screen)
        {
            //Direct initializations
            Level = level;
            MapElements = new List<MapElement>();
            GameplayElements = new HashSet<GameplayElement>();
            UIElements = new List<UIElement>();
            _updates = new HashSet<Update>();
            _timer = new Timer(DeltaTime * 1000);
            _timer.Elapsed += (object sender, ElapsedEventArgs e) => GameTick();
            _spawns = new PointF[level];
            _boarders = new Line[level + 1];
            _finishes = new Line[level];
            _tiles = new Queue<Tile>[level];


            //Calculation
            TileWidth = screen.Width * PlayAreaWidthRatio / level;
            TileHeight = screen.Height * TileHeightRatio;
            PlayArea = new RectangleF(new PointF(screen.Width * (1 - PlayAreaWidthRatio) / 2, 0), new SizeF(screen.Width * PlayAreaWidthRatio, screen.Height));

            //UIElements
            new Bar(Color.Black, new RectangleF(new PointF(PlayArea.Right, PlayArea.Top), new SizeF(TileWidth, TileHeight)));

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
                _boarders[i + 1] = new Line(_borderColor, PlayArea.Location + tileOffset, bottomLeft + tileOffset);
                _finishes[i] = new Line(_finishColor, bottomLeft + finishOffset, bottomLeft + finishOffset + tileOffset);
            }

            _timer.Start();
        }

        public static void AddUpdateCallback(Update action) => _updates.Add(action);
        public static void RemoveUpdateCallback(Update action) => _updates.Remove(action);

        private static void GameTick()
        {
            GameTicks++;
            ManageLives();
            foreach (var update in _updates) update();
            Spawner();
            FormPlay.Get.Refresh();
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
            int spawnLocationIndex = Random.Next() % Level;
            if (GameTicks % 100 == 0) _tiles[spawnLocationIndex].Enqueue(new Tile(Color.Black, SpawnLocations[spawnLocationIndex]));
        }
    }
}
