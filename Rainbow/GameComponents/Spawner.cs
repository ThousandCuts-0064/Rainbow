using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class Spawner
    {
        private const int DOUBLE_TILES_CHANCE = 33; // % chance
        private const int TRIPLE_TILES_CHANCE = 20; // % chance
        private readonly IColorModel _colorModel;
        private readonly GameModifiers _gameModifiers;
        private readonly int _level;
        private readonly Queue<Tile>[] _tileQueues;
        private Tile _lastSpawned;

        public Spawner(Queue<Tile>[] tileQueues, IColorModel colorModel, GameModifiers gameModifiers, int level)
        {
            _tileQueues = tileQueues;
            _colorModel = colorModel;
            _gameModifiers = gameModifiers;
            _level = level;

            //First Spawn. Spawner needs one spawn to chain spawn.
            Spawn(Game.Random.Next(_level));
        }

        public void OnTick()
        {
            //Hack: Compensates for 1 or 2 pixel stuttering, background won't flicker between touching tiles in same column.
            if (_lastSpawned.Location.Y < -1) return;

            int chance = Game.Random.Next(100);

            if (_gameModifiers.HasFlag(GameModifiers.DoubleTiles))
            {
                if (chance >= DOUBLE_TILES_CHANCE) // Chance must be between 0 and DOUBLE_TILES_CHANCE
                    chance -= DOUBLE_TILES_CHANCE;
                else
                {
                    int index1 = Game.Random.Next(_level);
                    Spawn(index1);
                    int index2 = Game.Random.Next(_level - 1);
                    if (index2 >= index1) index2++;
                    Spawn(index2);
                    chance = 100;
                }
            }

            if (_gameModifiers.HasFlag(GameModifiers.TripleTiles))
            {
                if (chance >= TRIPLE_TILES_CHANCE) // Chance must be between 0 and TRIPLE_TILES_CHANCE
                    chance -= TRIPLE_TILES_CHANCE;
                else
                {
                    int index1 = Game.Random.Next(_level);
                    Spawn(index1);
                    int index2 = Game.Random.Next(_level - 1);
                    if (index2 >= index1) index2++;
                    Spawn(index2);
                    int index3 = Game.Random.Next(_level - 2);
                    if (index3 >= index1) index3++;
                    if (index3 >= index2) index3++;
                    if (index3 == index1) index3++;
                    Spawn(index3);
                    chance = 100;
                }
            }

            if (chance < 100)
                Spawn(Game.Random.Next(_level));
        }

        private void Spawn(int spawnLocationIndex)
        {
            var randomColorCode = (ColorCode)(Game.Random.Next((int)ColorCode.All) + 1);

            _lastSpawned = new Tile(_colorModel, randomColorCode, _gameModifiers, spawnLocationIndex);
            _tileQueues[spawnLocationIndex].Enqueue(_lastSpawned);
        }
    }
}
