using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public class BirdyManager
    {
        private const int SPAWN_TICK_COOLDOWN = 500;
        private readonly Channel[] _channels;
        private readonly PointF _spawnLocation;

        public BirdyManager(Channel[] channels)
        {
            _channels = channels;
            _spawnLocation = new PointF(Game.Screen.X + Game.Screen.Width / 2, Game.Screen.Bottom + Birdy.Height);
        }

        public void OnTick()
        {
            if (Game.Ticks % SPAWN_TICK_COOLDOWN == 0)
                new Birdy(this, _spawnLocation);
        }

        /// <summary>
        /// Gets the tile that is closest to the finish line.
        /// </summary>
        /// <param name="target"></param>
        /// <returns>True if there is spawned tile, otherwise false.</returns>
        public bool TryGetClosestToFinish(out Tile target)
        {
            target = null;
            int i = 0;
            for (; i < _channels.Length; i++)
            {
                var tileList = _channels[i].TileList;
                if (tileList.Count != 0)
                    target = tileList.Last.Value;
            }
            if (target == null) return false;

            for (; i < _channels.Length; i++)
            {
                var tileList = _channels[i].TileList;
                if (tileList.Count == 0) continue;

                Tile test = tileList.Last.Value;
                if (test.Location.Y > target.Location.Y)
                    target = test;
            }
            if (target == null) return false;
            return true;
        }
    }
}
