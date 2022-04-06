using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class Spawner
    {
        private const int DOUBLE_TILES_CHANCE = 33; // % chance in standart state
        private const int TRIPLE_TILES_CHANCE = 20; // % chance in standart state
        private const int SHOTGUN_TILES_CHANCE = 10; // % chance for event trigger
        private readonly IColorModel _colorModel;
        private readonly GameModifiers _gameModifiers;
        private readonly int _level;
        private readonly Queue<Tile>[] _tileQueues;
        private SpawnerState _spawnerState;
        private NormalState _normalState;
        private ShotgunState _shotgunState;
        private Tile _lastSpawned;

        public Spawner(Queue<Tile>[] tileQueues, IColorModel colorModel, GameModifiers gameModifiers, int level)
        {
            _tileQueues = tileQueues;
            _colorModel = colorModel;
            _gameModifiers = gameModifiers;
            _level = level;

            _normalState = new NormalState(this);
            _shotgunState = new ShotgunState(this);
            SetState(_normalState);

            //First Spawn. Spawner needs one spawn to chain spawn.
            NormalSpawn(Game.Random.Next(_level));
        }

        public void OnTick()
        {
            if (_spawnerState.AllowSwap)
            {
                int chanceCap = 100;
                int chance = Game.Random.Next(chanceCap);
                if (_gameModifiers.HasFlag(GameModifiers.ShotgunTiles))
                {
                    if (chance >= SHOTGUN_TILES_CHANCE) // Chance must be between 0 and SHOTGUN_TILES_CHANCE
                        chance -= SHOTGUN_TILES_CHANCE;
                    else
                    {
                        SetState(_shotgunState);
                        chance = chanceCap;
                    }
                }

                if (chance < chanceCap)
                    SetState(_normalState);
            }

            _spawnerState.OnTick();
        }

        private void SetState(SpawnerState state) => _spawnerState = state.OnSet();

        private void NormalSpawn(int spawnLocationIndex)
        {
            var randomColorCode = (ColorCode)(Game.Random.Next((int)ColorCode.All) + 1);
            Spawn(spawnLocationIndex, randomColorCode);
        }

        private void Spawn(int spawnLocationIndex, ColorCode colorCode)
        {
            _lastSpawned = new Tile(_colorModel, colorCode, _gameModifiers, spawnLocationIndex);
            _tileQueues[spawnLocationIndex].Enqueue(_lastSpawned);
        }

        private abstract class SpawnerState
        {
            protected Spawner Spawner { get; }
            public bool AllowSwap { get; protected set; }

            public SpawnerState(Spawner spawner) => Spawner = spawner;

            public SpawnerState OnSet()
            {
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
                //Hack: Compensates for 1 or 2 pixel stuttering, background won't flicker between touching tiles in same column.
                if (Spawner._lastSpawned.Location.Y < -1) return;

                int chanceCap = 100;
                int chance = Game.Random.Next(chanceCap);

                if (Spawner._gameModifiers.HasFlag(GameModifiers.DoubleTiles))
                {
                    if (chance >= DOUBLE_TILES_CHANCE) // Chance must be between 0 and DOUBLE_TILES_CHANCE
                        chance -= DOUBLE_TILES_CHANCE;
                    else
                    {
                        int index1 = Game.Random.Next(Spawner._level);
                        Spawner.NormalSpawn(index1);
                        int index2 = Game.Random.Next(Spawner._level - 1);
                        if (index2 >= index1) index2++;
                        Spawner.NormalSpawn(index2);
                        chance = chanceCap;
                    }
                }

                if (Spawner._gameModifiers.HasFlag(GameModifiers.TripleTiles))
                {
                    if (chance >= TRIPLE_TILES_CHANCE) // Chance must be between 0 and TRIPLE_TILES_CHANCE
                        chance -= TRIPLE_TILES_CHANCE;
                    else
                    {
                        int index1 = Game.Random.Next(Spawner._level);
                        Spawner.NormalSpawn(index1);
                        int index2 = Game.Random.Next(Spawner._level - 1);
                        if (index2 >= index1) index2++;
                        Spawner.NormalSpawn(index2);
                        int index3 = Game.Random.Next(Spawner._level - 2);
                        if (index3 >= index1) index3++;
                        if (index3 >= index2) index3++;
                        if (index3 == index1) index3++;
                        Spawner.NormalSpawn(index3);
                        chance = chanceCap;
                    }
                }

                if (chance < chanceCap)
                    Spawner.NormalSpawn(Game.Random.Next(Spawner._level));

                AllowSwap = true;
            }

            protected override void OnStateSet() => AllowSwap = false;
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
                    Spawner.Spawn(i, colorCode);
                }
                _rowCount++;
            }

            protected override void OnStateSet()
            {
                AllowSwap = false;
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
    }
}
