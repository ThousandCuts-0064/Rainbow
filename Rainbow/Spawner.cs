//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rainbow
//{
//    class Spawner
//    {
//        private IColorModel _colorModel;
//        private Tile _lastSpawned;

//        public Spawner(, IColorModel colorModel)
//        {
//            _colorModel = colorModel;
//        }

//        private void Spawn()
//        {
//            int spawnLocationIndex = Game.Random.Next(Game.Level);
//            var randomColorCode = (ColorCode)(Game.Random.Next((int)ColorCode.All) + 1);

//            _lastSpawned = new Tile(
//                _colorModel[randomColorCode],
//                Game.SpawnLocations[spawnLocationIndex]);
//            _tiles[spawnLocationIndex].Enqueue(_lastSpawned);
//        }
//    }
//}
