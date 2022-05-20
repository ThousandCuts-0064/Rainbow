using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class TileSpawner
    {
        private const int NO_CLICK_TILES_CHANCE = 10; // % chance in standart state
        private const int DOUBLE_TILES_CHANCE = 15; // % chance in standart state
        private const int TRIPLE_TILES_CHANCE = 10; // % chance in standart state
        private const int DOUBLE_CLICK_TILES_CHANCE = 15; // % chance in standart state
        private const int TRIPLE_CLICK_TILES_CHANCE = 10; // % chance in standart state
        private const int NO_COLOR_TILES_CHANCE = 15; // % chance in standart state

        private const int MESSAGE_EVENT_CHANCE = 5; // % chance for event trigger
        private const int SHOTGUN_EVENT_CHANCE = 5; // % chance for event trigger
        private const int DIAMOND_EVENT_CHANCE = 5; // % chance for event trigger
        private const int CHESS_EVENT_CHANCE = 5; // % chance for event trigger
        private const int RAINBOW_EVENT_CHANCE = 5; // % chance for event trigger

        private readonly NormalState _normalState;
        private readonly MessageState _messageState;
        private readonly ShotgunState _shotgunState;
        private readonly DiamondState _diamondState;
        private readonly ChessState _chessState;
        private readonly RainbowState _rainbowState;
        private readonly IColorModel _colorModel;
        private readonly GameModifiers _gameModifiers;
        private readonly int _level;
        /// <summary>
        /// Should only be set using the SetState method.
        /// </summary>
        private State _state;
        private ulong _lastSpawnTick;

        public event Action<Tile, int> TileSpawned;

        public TileSpawner(IColorModel colorModel, GameModifiers gameModifiers, int level)
        {
            _colorModel = colorModel;
            _gameModifiers = gameModifiers;
            _level = level;

            _normalState = new NormalState(this);
            _messageState = new MessageState(this);
            _shotgunState = new ShotgunState(this);
            _diamondState = new DiamondState(this);
            _chessState = new ChessState(this);
            _rainbowState = new RainbowState(this);
            SetState(_normalState);
        }

        public void OnStart()
        {
            Spawn(Game.Random.Next(_level), RandomColor());
        }

        public void OnTick()
        {
            if (_state.AllowSwap)
            {
                int chanceCap = 100;
                int chanceCurrent = Game.Random.Next(chanceCap);

                if (TryChanceModifier(GameModifiers.MessageEvent, ref chanceCurrent, MESSAGE_EVENT_CHANCE, chanceCap))
                    SetState(_messageState);

                if (TryChanceModifier(GameModifiers.ShotgunEvent, ref chanceCurrent, SHOTGUN_EVENT_CHANCE, chanceCap))
                    SetState(_shotgunState);

                if (TryChanceModifier(GameModifiers.DiamondEvent, ref chanceCurrent, DIAMOND_EVENT_CHANCE, chanceCap))
                    SetState(_diamondState);

                if (TryChanceModifier(GameModifiers.ChessEvent, ref chanceCurrent, CHESS_EVENT_CHANCE, chanceCap))
                    SetState(_chessState);

                if (TryChanceModifier(GameModifiers.RainbowEvent, ref chanceCurrent, RAINBOW_EVENT_CHANCE, chanceCap))
                    SetState(_rainbowState);

                if (chanceCurrent < chanceCap)
                    SetState(_normalState);
            }

            _state.OnTick();
        }

        private bool TryChanceModifier(GameModifiers modifier, ref int chanceCurrent, int chanceTarget, int chanceCap)
        {
            if (!_gameModifiers.HasFlag(modifier) || chanceCurrent == chanceCap) return false;
            if (chanceCurrent >= chanceTarget)
            {
                chanceCurrent -= chanceTarget;
                return false;
            }
            chanceCurrent = chanceCap;
            return true;
        }

        private void SetState(State state) => _state = state.OnSet();

        private bool WaitedTileSpace(int spaces) =>
            Game.Ticks - _lastSpawnTick > (Game.TileHeight * (spaces + 1) - Game.TileSpeed) / Game.TileSpeed;

        private void Spawn(int channelIndex, ColorCode colorCode, int lives = 1, bool noClick = false)
        {
            _lastSpawnTick = Game.Ticks;
            TileSpawned?.Invoke(new Tile(_colorModel, colorCode, _gameModifiers, channelIndex, lives, noClick), channelIndex);
        }

        private ColorCode RandomColor() => (ColorCode)(Game.Random.Next((int)ColorCode.All) + 1);

        private abstract class State
        {
            protected TileSpawner Spawner { get; }
            public bool AllowSwap { get; protected set; }

            protected State(TileSpawner spawner) => Spawner = spawner;

            public State OnSet()
            {
                AllowSwap = false;
                OnStateSet();
                return this;
            }

            public abstract void OnTick();

            protected abstract void OnStateSet();
        }

        private class NormalState : State
        {
            public NormalState(TileSpawner spawner) : base(spawner) { }

            public override void OnTick()
            {
                if (!Spawner.WaitedTileSpace(0)) return;

                int chanceCap = 100;
                int chanceCurrent = Game.Random.Next(chanceCap);

                if (Spawner.TryChanceModifier(GameModifiers.DoubleTiles, ref chanceCurrent, DOUBLE_TILES_CHANCE, chanceCap))
                {
                    int index1 = Game.Random.Next(Spawner._level);
                    Spawner.Spawn(index1, Spawner.RandomColor());
                    int index2 = Game.Random.Next(Spawner._level - 1);
                    if (index2 >= index1) index2++;
                    Spawner.Spawn(index2, Spawner.RandomColor());
                }

                if (Spawner.TryChanceModifier(GameModifiers.TripleTiles, ref chanceCurrent, TRIPLE_TILES_CHANCE, chanceCap))
                {
                    int index1 = Game.Random.Next(Spawner._level);
                    Spawner.Spawn(index1, Spawner.RandomColor());
                    int index2 = Game.Random.Next(Spawner._level - 1);
                    if (index2 >= index1) index2++;
                    Spawner.Spawn(index2, Spawner.RandomColor());
                    int index3 = Game.Random.Next(Spawner._level - 2);
                    if (index3 >= index1) index3++;
                    if (index3 >= index2) index3++;
                    if (index3 == index1) index3++;
                    Spawner.Spawn(index3, Spawner.RandomColor());
                }

                if (Spawner.TryChanceModifier(GameModifiers.NoClickTiles, ref chanceCurrent, NO_CLICK_TILES_CHANCE, chanceCap))
                    Spawner.Spawn(Game.Random.Next(Spawner._level), Spawner.RandomColor(), noClick: true);

                if (Spawner.TryChanceModifier(GameModifiers.DoubleClickTiles, ref chanceCurrent, DOUBLE_CLICK_TILES_CHANCE, chanceCap))
                    Spawner.Spawn(Game.Random.Next(Spawner._level), Spawner.RandomColor(), 2);

                if (Spawner.TryChanceModifier(GameModifiers.TripleClickTiles, ref chanceCurrent, TRIPLE_CLICK_TILES_CHANCE, chanceCap))
                    Spawner.Spawn(Game.Random.Next(Spawner._level), Spawner.RandomColor(), 3);

                if (Spawner.TryChanceModifier(GameModifiers.NoColorTiles, ref chanceCurrent, NO_COLOR_TILES_CHANCE, chanceCap))
                    Spawner.Spawn(Game.Random.Next(Spawner._level), ColorCode.None);

                if (chanceCurrent < chanceCap)
                    Spawner.Spawn(Game.Random.Next(Spawner._level), Spawner.RandomColor());

                AllowSwap = true;
            }

            protected override void OnStateSet() { }
        }

        private abstract class SpecialState : State
        {
            private int _rowIndex;
            private bool _waitSpace;
            private bool _finishedSpawning;
            protected int TotalRowSpawns { get; set; }
            protected int Space { get; set; }

            public SpecialState(TileSpawner spawner, int totalRowSpawns = 0, int space = 3) : base(spawner)
            {
                TotalRowSpawns = totalRowSpawns;
                Space = space;
            }

            public sealed override void OnTick()
            {
                if (_waitSpace &&
                    !Spawner.WaitedTileSpace(Space))
                    return;

                if (_finishedSpawning)
                {
                    AllowSwap = true;
                    return;
                }
                _waitSpace = false;

                if (!Spawner.WaitedTileSpace(0)) return;
                OnSpawnTick(_rowIndex);
                _rowIndex++;

                if (_rowIndex != TotalRowSpawns) return;
                _finishedSpawning = true;
                _waitSpace = true;
            }

            protected abstract void OnSpawnTick(int rowIndex);

            protected override void OnStateSet()
            {
                _rowIndex = 0;
                _waitSpace = true;
                _finishedSpawning = false;
            }
        }

        private class MessageState : SpecialState
        {
            private static readonly string[] _messages = new string[]
            {
                "Loading..........",
                "I love spawning tiles",
                "Dont look at my tiles",
                "Here they come",
                "This is my favorite event",
                "My time has come",
                "Its time to shine",
                "Pssst, I love you",
                "MUHAHAHA",
                "The secret is RAINTILE"
            }.Select(str => str.ToUpper()).ToArray();
            private string _message;

            public MessageState(TileSpawner spawner) : base(spawner, space: 1) { }

            protected override void OnSpawnTick(int rowIndex)
            {
                ColorColumn cc = ColorColumn.FromInput(_message[rowIndex].ToString(), Game.Random.Next(Spawner._level));
                Spawner.Spawn(cc.Column, cc.ColorCode);
            }

            protected override void OnStateSet()
            {
                base.OnStateSet();
                _message = _messages[Game.Random.Next(_messages.Length)];
                TotalRowSpawns = _message.Length;
            }
        }

        private class ShotgunState : SpecialState
        {
            public ShotgunState(TileSpawner spawner) : base(spawner, 3) { }

            protected override void OnSpawnTick(int rowIndex)
            {
                for (int i = 0; i < Spawner._level; i++)
                    Spawner.Spawn(i, (ColorCode)Math.Pow(2, rowIndex));
            }
        }

        private class DiamondState : SpecialState
        {
            private static readonly int[] _sizes = { 3, 5, 7, 9 }; // Number of rows
            private readonly int[] _weightedIndexes; // For size
            private int _centerColumn;
            private int _centerRow;
            private ColorCode colorCode;

            public DiamondState(TileSpawner spawner) : base(spawner)
            {
                int maxSize = (Spawner._level - 1) / 2;
                _weightedIndexes = new int[Function(maxSize)];
                for (int i = 0; i < maxSize; i++)
                    for (int y = i; y < maxSize; y++)
                        _weightedIndexes[i * maxSize - Function(i) + y] = i;

                int Function(int n) => (int)((1 + n) / 2f * n);
            }

            protected override void OnSpawnTick(int rowIndex)
            {
                for (
                    int i = Math.Abs(_centerRow - rowIndex) - _centerRow;
                    i <= _centerRow - Math.Abs(_centerRow - rowIndex);
                    i++)
                    Spawner.Spawn(
                        _centerColumn + i,
                        colorCode,
                        TotalRowSpawns - _centerRow - Math.Abs(i) - Math.Abs(_centerRow - rowIndex));
            }

            protected override void OnStateSet()
            {
                base.OnStateSet();
                int rndIndex = _weightedIndexes[Game.Random.Next(_weightedIndexes.Length)];
                int size = _sizes[rndIndex];
                TotalRowSpawns = size;
                Space = rndIndex;
                _centerColumn = Game.Random.Next(size / 2, Spawner._level - size / 2);
                _centerRow = TotalRowSpawns / 2;
                colorCode = Spawner.RandomColor();
            }
        }

        private class ChessState : SpecialState
        {
            public ChessState(TileSpawner spawner) : base(spawner, spawner._level) { }

            protected override void OnSpawnTick(int rowIndex)
            {
                for (int i = 0; i < Spawner._level; i++)
                    Spawner.Spawn(i, (i + rowIndex) % 2 == 0 ? ColorCode.All : ColorCode.None);
            }
        }

        private class RainbowState : SpecialState
        {
            private readonly ColorCode[] _rainbow = new ColorCode[6];
            private readonly int _size;
            private readonly int _period;
            private int _spawnIndex;
            private bool _tryChain;

            public RainbowState(TileSpawner spawner) : base(spawner)
            {
                _size = Spawner._level - _rainbow.Length;
                _period = 4 * _size + 1;
                TotalRowSpawns = _period;
                _rainbow = new ColorCode[]
                {
                    ColorCode.I,
                    ColorCode.I_II,
                    ColorCode.II,
                    ColorCode.II_III,
                    ColorCode.III,
                    ColorCode.I_III
                }
                    .OrderBy(cc =>
                        {
                            var c = Spawner._colorModel.CodeToColor(cc);
                            return // byte * 2 = 8 + 1 = 9 bits
                                ((c.R > c.B * 2 && c.B > c.G ? (ulong)(c.R * 2 - c.B) : 0) << 9 * 3) +  // Magenta to red
                                ((c.G >= c.B ? (ulong)(byte.MaxValue + c.R - c.G) : 0) << 9 * 2) +      // Red to green
                                ((c.B >= c.R ? (ulong)(byte.MaxValue + c.G - c.B) : 0) << 9) +          // Green to blue
                                (c.R >= c.G ? (ulong)(byte.MaxValue + c.B - c.R) : 0);                  // Blue to red (magenta)
                        })
                    .ToArray();
            }

            protected override void OnSpawnTick(int rowIndex)
            {
                double epsilon = 1e-15; // Fixes small inaccuracy that causes numbers to have fractional part very close to .5 but less
                if (Spawner._level != _rainbow.Length)
                {
                    rowIndex %= _period;
                    _spawnIndex = (int)Math.Round(
                        _size * (Math.Cos(rowIndex * Math.PI / (2 * _size) + Math.PI) + 1) / 2 + epsilon, // Wave function
                        MidpointRounding.AwayFromZero);
                }

                for (int i = 0; i < _rainbow.Length; i++)
                    Spawner.Spawn(_spawnIndex + i, _rainbow[i]);

                if (_tryChain && Game.Random.Next(10) > 0)
                    TotalRowSpawns++; // 90% chance to chain
                else _tryChain = false;
            }

            protected override void OnStateSet()
            {
                base.OnStateSet();
                _spawnIndex = 0;
                _tryChain = true;
            }
        }
    }
}
