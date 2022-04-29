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
        private readonly Dictionary<Tile, Birdy> _targetToBirdy = new Dictionary<Tile, Birdy>();
        private readonly Channel[] _channels;
        private readonly PointF _spawnLocation;

        public BirdyManager(Channel[] channels)
        {
            _channels = channels;
            _spawnLocation = new PointF(Game.Screen.X + Game.Screen.Width / 2, Game.Screen.Bottom + Birdy.Height);
        }

        public void OnTick()
        {
            if (Game.Ticks % SPAWN_TICK_COOLDOWN != 0) return;

            var birdy = new Birdy(this, _spawnLocation);
            birdy.TargetFound += tile => _targetToBirdy.Add(tile, birdy);
            birdy.TargetAcquired += tile => _targetToBirdy.Remove(tile);
        }

        /// <summary>
        /// Gets the tile that is closest to the finish line.
        /// </summary>
        /// <param name="target"></param>
        /// <returns>True if there is spawned tile, otherwise false.</returns>
        public bool RequestTarget(out Tile target)
        {
            target = null;
            int i = 0;
            for (; i < _channels.Length; i++)
            {
                if (_channels[i].TileCount != 0)
                {
                    target = _channels[i].TileNodeLast.Value;
                    break;
                }
            }
            if (target == null) return false;

            for (; i < _channels.Length; i++)
            {
                if (_channels[i].TileCount == 0) continue;

                Tile test = _channels[i].TileNodeLast.Value;
                if (test.Location.Y > target.Location.Y)
                    target = test;
            }
            if (target == null) return false;
            return true;
        }

        public void OnPotentialTargetLost(Tile tile)
        {
            if (_targetToBirdy.TryGetValue(tile, out var birdy))
                birdy.Retarget();
        }
    }
}
