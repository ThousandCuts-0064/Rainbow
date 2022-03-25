using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class Spawner
    {
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
            Spawn();
        }

        public void OnTick()
        {
            //Hack: Compensates for 1 pixel stuttering, background won't flicker between touching tiles in same column.
            if (_lastSpawned.Location.Y >= -1)
                Spawn();
        }

        private void Spawn()
        {
            int spawnLocationIndex = Game.Random.Next(_level);
            var randomColorCode = (ColorCode)(Game.Random.Next((int)ColorCode.All) + 1);

            _lastSpawned = new Tile(
                _colorModel.CodeToColor(randomColorCode),
                Game.SpawnLocations[spawnLocationIndex],
                _gameModifiers.HasFlag(GameModifiers.HintButtons));
            _tileQueues[spawnLocationIndex].Enqueue(_lastSpawned);
        }
    }
}
