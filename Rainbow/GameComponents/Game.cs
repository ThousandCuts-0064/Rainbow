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
        private const int START_TILE_UNITS_PER_SECOND = 10;
        private static readonly Color _colorBoarder = Color.Black;
        private static Dictionary<Layer, List<IDrawable>> _layerToList;
        private static HashSet<Update> _updates;
        private static Channel[] _channels; // Pass ReadOnly to Channels Property
        private static IColorModel _colorModel;
        private static Timer _timer;
        private static Stats _stats;
        private static InputManager _inputManager;
        private static TileSpawner _tileSpawner;
        private static BirdyManager _birdyManager;
        private static Record _record;
        private static FormPlay _formPlay;
        private static Line _boarderLeft;
        private static Line _boarderRight;
        private static GameImage _colorDiagram;
        private static GameModifiers _gameModifiers;
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
        /// <summary>
        /// Number of units
        /// </summary>
        public const int TILE_HIGHT_UNITS = 10;
        public static ILine BoarderRight => _boarderRight;
        public static ILine BoarderLeft => _boarderLeft;
        public static Random Random { get; } = new Random();
        public static string RecordsDirectoryName { get; }
        public static IReadOnlyList<IChannel> Channels { get; private set; }
        public static RectangleF PlayArea { get; private set; }
        public static Rectangle Screen { get; private set; }
        public static ulong Ticks { get; private set; } // This will overflow after approximately 9,359,078,677 years with 16ms delta time
        public static float Unit { get; private set; }
        public static float HalfUnit { get; private set; }
        public static float MapLineWidth { get; private set; }
        public static float ChannelWidth { get; private set; }
        public static float UIElementWidth { get; private set; }
        public static float TileWidth { get; private set; }
        public static float TileHeight { get; private set; }
        public static float TileSpeed { get; private set; }
        public static float TileUnitsPerSecond { get; private set; }
        public static int Level { get; private set; }
        public static bool IsActive { get; private set; }
        public static bool IsPaused { get => !_timer.Enabled; set => _timer.Enabled = !value; }

        static Game()
        {
            RecordsDirectoryName = System.Data.Entity.Design.PluralizationServices.PluralizationService
                .CreateService(new System.Globalization.CultureInfo("en-US", false))
                .Pluralize(nameof(Record));
        }

        public static void Initialize(FormPlay formPlay, IColorModel colorModel, GameModifiers gameModifiers, int level) // Possible Reinitialization
        {
            //Direct initialization
            Ticks = 0;
            TileUnitsPerSecond = START_TILE_UNITS_PER_SECOND;
            _formPlay = formPlay;
            _colorModel = colorModel;
            _gameModifiers = gameModifiers;
            Level = level;
            _record = new Record
            {
                ColorModelName = colorModel.Name,
                GameModifiers = gameModifiers,
                Level = Level
            };
            IsActive = true;

            //Direct object Creation
            _updates = new HashSet<Update>();
            _layerToList = new Dictionary<Layer, List<IDrawable>>();
            for (int i = 0; Enum.IsDefined(typeof(Layer), i); i++)
                _layerToList.Add((Layer)i, new List<IDrawable>());

            _timer = new Timer { Interval = (int)(DELTA_TIME * 1000) };
            _inputManager = new InputManager(level);
            _tileSpawner = new TileSpawner(colorModel, gameModifiers, level);

            //Calculation
            var screen = formPlay.ClientRectangle;
            Screen = screen;
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
            Channels = Array.AsReadOnly(_channels);
            for (int i = 0; i < level; i++)
            {
                var rectangleF = new RectangleF(
                    PlayArea.Location,
                    new SizeF(ChannelWidth, PlayArea.Height));

                rectangleF.Offset(ChannelWidth * i, 0);
                _channels[i] = new Channel(rectangleF, gameModifiers, MapLineWidth, i);
            }

            //Dependant object creation
            _stats = new Stats(colorModel, level);

            if (gameModifiers.HasFlag(GameModifiers.Birdy))
            {
                _birdyManager = new BirdyManager();
                _tileSpawner.TileSpawned += (tile, index) => _birdyManager.OnTargetAppear(tile);
            }

            if (gameModifiers.HasFlag(GameModifiers.ColorWheel))
                _colorDiagram = new GameImage(
                    Resources.ColorWheel,
                    CalculateColorWheelRectangle(),
                    Layer.UI);

            //Events
            _timer.Tick += GameTick;
            _inputManager.ShotgunPressed += _stats.OnShotgunPressed;
            _inputManager.ColorInput += (colorCode, column) => _channels[column].OnColorInput(colorCode);
            _tileSpawner.TileSpawned += (tile, index) => _channels[index].TileListAddFirst(tile);
            _tileSpawner.TileSpawned += (tile, index) => tile.Popped += () => _record.TilesPopped++;
            _stats.Death += () =>
            {
                if (!IsActive) return; // Ensures single invoke

                _record.TimeSpan = TimeSpan.FromMilliseconds(Ticks / DELTA_TIME);
                ReadOnlyRecord readOnlyRecord = new ReadOnlyRecord(_record);
                if (!Save.DirectoryExists(RecordsDirectoryName))
                    Save.CreateDirectory(RecordsDirectoryName);
                Save.Serialize(
                    Path.Combine(RecordsDirectoryName, DateTime.Now.ToString().Replace('/', '-').Replace(':', '-')) + ".dat",
                    readOnlyRecord);
                new FormRecord(formPlay, readOnlyRecord, true).Show();
                IsActive = false;
                IsPaused = true;
            };
            foreach (var channel in _channels)
            {
                _stats.ShotgunUsed += channel.OnShotgunUsed;
                channel.TileClicked += tile => _record.TilesClicked++;
                channel.TilePassed += tile => _record.TilesPassed++;
                channel.TilePassed += _stats.OnTakeTile;
                channel.NoClickTilePopped += _stats.OnTakeNoClickTile;
                if (_birdyManager != null) channel.TileRemoved += _birdyManager.OnTargetDisappear;
                if (gameModifiers.HasFlag(GameModifiers.ColorfulBack))
                {
                    _tileSpawner.TileSpawned += OneTimeTrackSpawner;

                    channel.TileRemoved += tile =>
                    {
                        var lastTileNode = _channels.MaxBy(ch => ch.TileNodeLast?.Value.Location.Y ?? -TileHeight * 2).TileNodeLast;
                        if (lastTileNode == null) _tileSpawner.TileSpawned += OneTimeTrackSpawner; // If somehow there aren't any tiles on the screen
                        else _formPlay.BackColor = colorModel.CodeToColor(lastTileNode.Value.ColorCode);
                    };

                    void OneTimeTrackSpawner(Tile t, int index)
                    {
                        _formPlay.BackColor = colorModel.CodeToColor(t.ColorCode);
                        _tileSpawner.TileSpawned -= OneTimeTrackSpawner;
                    }
                }
            }
            formPlay.KeyDown += _inputManager.OnKeyDown;
            formPlay.KeyUp += _inputManager.OnKeyUp;


            //Start Game
            _timer.Start();

            //Start calls
            _tileSpawner.OnStart();

            //Local functions
            RectangleF CalculateColorWheelRectangle() =>
            new RectangleF(
                new PointF(
                    _boarderRight.Point2.X + MapLineWidth / 2,
                    _boarderRight.Point2.Y + MapLineWidth / 2 - TileHeight * 3),
                new SizeF(
                    UIElementWidth,
                    TileHeight * 3));
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

        /// <summary>
        /// Tries to return ReadOnlyRecord with current values.
        /// </summary>
        /// <returns>ReadOnlyRecord with current value or null if game is not active.</returns>
        public static ReadOnlyRecord RequestCurrentRecord()
        {
            if (!IsActive) return null;

            _record.TimeSpan = TimeSpan.FromMilliseconds(Ticks / DELTA_TIME);
            return new ReadOnlyRecord(_record);
        }

        private static void GameTick(object sender, EventArgs e)
        {
            Ticks++;
            TileSpeed = TileUnitsPerSecond * Unit * DELTA_TIME;
            _inputManager.OnTick();
            foreach (var update in _updates.ToList()) update();
            foreach (var channel in _channels) channel.OnTick();
            _stats.OnTick();
            _tileSpawner.OnTick();
            _birdyManager?.OnTick();
            _formPlay.Invalidate();
            TileUnitsPerSecond += Ticks % 100 == 0 ? 0.1f : 0;
        }

        [Serializable]
        public class Record
        {
            public TimeSpan TimeSpan { get; set; }
            public GameModifiers GameModifiers { get; set; }
            public string ColorModelName { get; set; }
            public int Level { get; set; }
            public int TilesClicked { get; set; }
            public int TilesPopped { get; set; } // Shotgun doesn't pop tiles
            public int TilesPassed { get; set; }
        }

        [Serializable]
        public class ReadOnlyRecord
        {
            private readonly Record _record;

            public TimeSpan TimeSpan => _record.TimeSpan;
            public GameModifiers GameModifiers => _record.GameModifiers;
            public string ColorModelName => _record.ColorModelName;
            public int Level => _record.Level;
            public int TilesClicked => _record.TilesClicked;
            public int TilesPopped => _record.TilesPopped;
            public int TilesPassed => _record.TilesPassed;

            public ReadOnlyRecord(Record record) => _record = record;

            public object CreateShort() => ShortString.From(this);

            private class ShortString
            {
                private static readonly StringBuilder _stringBuilder = new StringBuilder();
                private string _str;

                public static object From(ReadOnlyRecord record)
                {
                    var @short = new ShortString();
                    _stringBuilder
                        .Append(record.Level)
                        .Append('\t')
                        .Append(record.ColorModelName)
                        .Append('\t')
                        .Append(record.TimeSpan.ToString("G").TrimEnd(4));
                    @short._str = _stringBuilder.ToString();
                    _stringBuilder.Clear();
                    return @short;
                }

                public override string ToString() => _str;
            }
        }
    }
}