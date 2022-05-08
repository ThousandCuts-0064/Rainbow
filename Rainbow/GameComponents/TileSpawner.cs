using System;
using System.Collections.Generic;
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

        private const int SHOTGUN_EVENT_CHANCE = 5; // % chance for event trigger
        private const int DIAMOND_EVENT_CHANCE = 500; // % chance for event trigger
        private const int CHESS_EVENT_CHANCE = 5; // % chance for event trigger
        private const int RAINBOW_EVENT_CHANCE = 5; // % chance for event trigger

        private readonly NormalState _normalState;
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
                        Spawner.RandomColor(),
                        TotalRowSpawns - _centerRow - Math.Abs(i) - Math.Abs(_centerRow - rowIndex));
            }

            protected override void OnStateSet()
            {
                base.OnStateSet();
                int rndIndex = _weightedIndexes[Game.Random.Next(_weightedIndexes.Length)];
                int size = _sizes[rndIndex];
                TotalRowSpawns = size;
                Space = rndIndex;
                _centerColumn = Game.Random.Next(size - 1, Spawner._level - 1 - rndIndex); // min not tested
                _centerRow = TotalRowSpawns / 2;
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
            public RainbowState(TileSpawner spawner) : base(spawner, 0) { }

            protected override void OnSpawnTick(int rowIndex)
            {
                throw new NotImplementedException();
            }

            protected override void OnStateSet()
            {
                throw new NotImplementedException();
            }
        }
    }
}
