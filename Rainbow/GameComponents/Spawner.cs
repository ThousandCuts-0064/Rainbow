using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class Spawner
    {
        private const int NO_CLICK_TILES_CHANCE = 10; // % chance in standart state
        private const int DOUBLE_TILES_CHANCE = 15; // % chance in standart state
        private const int TRIPLE_TILES_CHANCE = 10; // % chance in standart state
        private const int DOUBLE_CLICK_TILES_CHANCE = 15; // % chance in standart state
        private const int TRIPLE_CLICK_TILES_CHANCE = 10; // % chance in standart state
        private const int NO_COLOR_TILES_CHANCE = 15; // % chance in standart state

        private const int SHOTGUN_TILES_CHANCE = 5; // % chance for event trigger
        private const int DIAMOND_EVENT_CHANCE = 5; // % chance for event trigger
        private const int CHESS_EVENT_CHANCE = 5; // % chance for event trigger
        private const int RAINBOW_EVENT_CHANCE = 5; // % chance for event trigger

        private readonly Channel[] _channels;
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
        private SpawnerState _spawnerState;
        private Tile _lastSpawned;

        public Spawner(Channel[] channels, IColorModel colorModel, GameModifiers gameModifiers, int level)
        {
            _channels = channels;
            _colorModel = colorModel;
            _gameModifiers = gameModifiers;
            _level = level;

            _normalState = new NormalState(this);
            _shotgunState = new ShotgunState(this);
            _diamondState = new DiamondState(this);
            _chessState = new ChessState(this);
            _rainbowState = new RainbowState(this);
            SetState(_normalState);

            //First Spawn. Spawner needs one spawn to chain spawn.
            Spawn(Game.Random.Next(_level), RandomColor());
        }

        public void OnTick()
        {
            if (_spawnerState.AllowSwap)
            {
                int chanceCap = 100;
                int chanceCurrent = Game.Random.Next(chanceCap);

                if (TryChanceModifier(GameModifiers.ShotgunTiles, ref chanceCurrent, SHOTGUN_TILES_CHANCE, chanceCap))
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

            _spawnerState.OnTick();
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

        private void SetState(SpawnerState state) => _spawnerState = state.OnSet();

        private void Spawn(int ChannelIndex, ColorCode colorCode, int lives = 1, bool noClick = false)
        {
            _lastSpawned = new Tile(_colorModel, colorCode, _gameModifiers, ChannelIndex, lives, noClick);
            _channels[ChannelIndex].TileList.AddFirst(_lastSpawned);
        }

        private ColorCode RandomColor() => (ColorCode)(Game.Random.Next((int)ColorCode.All) + 1);

        private abstract class SpawnerState
        {
            protected Spawner Spawner { get; }
            public bool AllowSwap { get; protected set; }

            public SpawnerState(Spawner spawner) => Spawner = spawner;

            public SpawnerState OnSet()
            {
                AllowSwap = false;
                OnStateSet();
                return this;
            }

            public abstract void OnTick();

            protected abstract void OnStateSet();
        }

        private class NormalState : SpawnerState
        {
            public NormalState(Spawner spawner) : base(spawner) { }

            public override void OnTick()
            {
                //Hack: Compensates for some pixel stuttering, background will flicker less between touching tiles in the same column.
                if (Spawner._lastSpawned.Location.Y < -1) return;

                int chanceCap = 100;
                int chanceCurrent = Game.Random.Next(chanceCap);

                if (Spawner.TryChanceModifier(GameModifiers.NoClickTiles, ref chanceCurrent, NO_CLICK_TILES_CHANCE, chanceCap))
                {
                    Spawner.Spawn(Game.Random.Next(Spawner._level), Spawner.RandomColor(), 1, true);
                }

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

        private class ShotgunState : SpawnerState
        {
            private bool _waitSpace;
            private bool _finishedCycle;
            private int _rowCount;

            public ShotgunState(Spawner spawner) : base(spawner) { }

            public override void OnTick()
            {
                if (_waitSpace &&
                    Spawner._lastSpawned.Location.Y < Game.TileHeight * 3)
                    return;
                else
                {
                    if (_finishedCycle)
                    {
                        AllowSwap = true;
                        return;
                    }

                    _waitSpace = false;
                }

                if (Spawner._lastSpawned.Location.Y < -1) return;
                for (int i = 0; i < Spawner._level; i++)
                {
                    ColorCode colorCode;
                    switch (_rowCount)
                    {
                        case 0:
                            colorCode = ColorCode.I;
                            break;

                        case 1:
                            colorCode = ColorCode.II;
                            break;

                        case 2:
                            colorCode = ColorCode.III;
                            break;

                        default:
                            _waitSpace = true;
                            _finishedCycle = true;
                            return;
                    }
                    Spawner.Spawn(i, colorCode, 1);
                }
                _rowCount++;
            }

            protected override void OnStateSet()
            {
                _waitSpace = true;
                _finishedCycle = false;
                _rowCount = 0;
            }
        }

        private class DiamondState : SpawnerState
        {
            public DiamondState(Spawner spawner) : base(spawner) { }

            public override void OnTick()
            {
                throw new NotImplementedException();
            }

            protected override void OnStateSet()
            {
                throw new NotImplementedException();
            }
        }

        private class ChessState : SpawnerState
        {
            private bool _waitSpace;
            private bool _finishedCycle;
            private int _rowCount;

            public ChessState(Spawner spawner) : base(spawner) { }

            public override void OnTick()
            {
                if (_waitSpace &&
                    Spawner._lastSpawned.Location.Y < Game.TileHeight * 3)
                    return;
                else
                {
                    if (_finishedCycle)
                    {
                        AllowSwap = true;
                        return;
                    }

                    _waitSpace = false;
                }

                if (Spawner._lastSpawned.Location.Y < -1) return;
                for (int i = 0; i < Spawner._level; i++)
                    Spawner.Spawn(i,
                        i % 2 == _rowCount % 2
                        ? ColorCode.All
                        : ColorCode.None,
                        1);
                _rowCount++;
                if (_rowCount == Spawner._level)
                {
                    _waitSpace = true;
                    _finishedCycle = true;
                }
            }

            protected override void OnStateSet()
            {
                _waitSpace = true;
                _finishedCycle = false;
                _rowCount = 0;
            }
        }

        private class RainbowState : SpawnerState
        {
            public RainbowState(Spawner spawner) : base(spawner) { }

            public override void OnTick()
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
