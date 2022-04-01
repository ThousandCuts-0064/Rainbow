using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Rainbow.Game;

namespace Rainbow
{
    class Stats
    {
        private const int DEFAULT_MAX_LIFES = 10;
        private readonly Queue<Tile>[] _tileQueues;
        private readonly IColorModel _colorModel;
        private readonly Bar _lifeI;
        private readonly Bar _lifeII;
        private readonly Bar _lifeIII;
        private readonly int _level;

        public Stats(Queue<Tile>[] tileQueues, IColorModel colorModel, int level)
        {
            _colorModel = colorModel;
            _tileQueues = tileQueues;
            _level = level;

            var playArea = Game.PlayArea;
            var width = Game.UIElementWidth;
            var hight = Game.TileHeight;

            _lifeI = new Bar(
                colorModel.CodeToColor(ColorCode.I),
                new RectangleF(
                    new PointF(playArea.Right, playArea.Top),
                    new SizeF(width, hight)),
                DEFAULT_MAX_LIFES);

            _lifeII = new Bar(
                colorModel.CodeToColor(ColorCode.II),
                new RectangleF(
                    new PointF(playArea.Right, playArea.Top + hight),
                    new SizeF(width, hight)),
                DEFAULT_MAX_LIFES);

            _lifeIII = new Bar(
                colorModel.CodeToColor(ColorCode.III),
                new RectangleF(
                    new PointF(playArea.Right, playArea.Top + hight * 2),
                    new SizeF(width, hight)),
                DEFAULT_MAX_LIFES);

            //Hack: Game is just paused on Game Over
            var a = new Action(() => Game.IsPaused = true);
            _lifeI.Resource.Empty += a;
            _lifeII.Resource.Empty += a;
            _lifeIII.Resource.Empty += a;
        }

        public void OnTick()
        {
            for (int i = 0; i < _level; i++)
            {
                if (_tileQueues[i].Count != 0 &&
                    _tileQueues[i].Peek().Location.Y > Game.Boarders[i].Second.Y)
                {
                    var tile = _tileQueues[i].Dequeue();
                    TakeTile(tile);
                    tile.Dispose();
                }
            }
        }

        private void TakeTile(Tile tile)
        {
            if (tile.ColorCode.HasFlag(ColorCode.I)) _lifeI.Resource.Current--;
            if (tile.ColorCode.HasFlag(ColorCode.II)) _lifeII.Resource.Current--;
            if (tile.ColorCode.HasFlag(ColorCode.III)) _lifeIII.Resource.Current--;
        }
    }
}
