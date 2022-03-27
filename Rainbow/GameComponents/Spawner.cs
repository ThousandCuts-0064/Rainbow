using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class Spawner
    {
        private const int SECOND_TILE_CHANCE = 3; // Actual chance is 1 / SECOND_TILE_CHANCE
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
            //Hack: Compensates for 1 pixel stuttering, background won't flicker between touching tiles in same column.
            if (_lastSpawned.Location.Y < -1) return;

            int spawnLocationIndex = Game.Random.Next(_level);
            Spawn(spawnLocationIndex);

            if (!_gameModifiers.HasFlag(GameModifiers.DoubleTiles)) return;
            if (Game.Random.Next(SECOND_TILE_CHANCE) != 0) return;

            int newSpawnLocationIndex = Game.Random.Next(_level - 1);
            if (newSpawnLocationIndex >= spawnLocationIndex) newSpawnLocationIndex++;
            if (newSpawnLocationIndex == _level - 1) newSpawnLocationIndex = 0;
            Spawn(newSpawnLocationIndex);
        }

        private void Spawn(int spawnLocationIndex)
        {
            var randomColorCode = (ColorCode)(Game.Random.Next((int)ColorCode.All) + 1);

            _lastSpawned = new Tile(
                _colorModel,
                randomColorCode,
                spawnLocationIndex,
                _gameModifiers.HasFlag(GameModifiers.HintButtons));
            _tileQueues[spawnLocationIndex].Enqueue(_lastSpawned);
        }
    }
}
