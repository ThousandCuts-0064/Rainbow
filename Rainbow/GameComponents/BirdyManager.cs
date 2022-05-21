using CustomCollections;
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
        private readonly HashedLinkedList<Tile> _listAllTargets = new HashedLinkedList<Tile>();
        private readonly PointF _defaultSpawnLocation;

        public BirdyManager()
        {
            _defaultSpawnLocation = new PointF(Game.Screen.X + Game.Screen.Width / 2, Game.Screen.Bottom + Birdy.Height);
        }

        public void OnTick()
        {
            if (Game.Ticks % SPAWN_TICK_COOLDOWN != 0) return;

            Birdy birdy = null;
            if (RequestTarget(out var tile))
            {
                birdy = new Birdy(this, new PointF(tile.Location.X, _defaultSpawnLocation.Y), tile);
                OnTargetFound(tile);
            }
            else birdy = new Birdy(this, _defaultSpawnLocation);

            birdy.TargetFound += OnTargetFound;
            birdy.TargetTaken += target => _targetToBirdy.Remove(target);

            void OnTargetFound(Tile target)
            {
                _targetToBirdy.Add(target, birdy);
                _listAllTargets.Remove(target);
            }
        }

        public void OnTargetAppear(Tile tile)
        {
            _listAllTargets.AddFirst(tile);
        }

        public void OnTargetDisappear(Tile tile)
        {
            _listAllTargets.Remove(tile);
            if (_targetToBirdy.TryGetValue(tile, out var birdy))
            {
                birdy.Retarget();
                _targetToBirdy.Remove(tile);
            }
        }

        /// <summary>
        /// Gets the closest tile to the finish line.
        /// </summary>
        /// <param name="target"></param>
        /// <returns>True if there is spawned tile, otherwise false.</returns>
        public bool RequestTarget(out Tile target)
        {
            target = null;
            if (_listAllTargets.Count == 0) return false;

            target = _listAllTargets.Last.Value;
            return true;
        }
    }
}
