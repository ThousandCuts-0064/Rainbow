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
        private const int defaultMaxLifes = 10;
        private readonly IColorModel _colorModel;
        private readonly Bar _lifeI;
        private readonly Bar _lifeII;
        private readonly Bar _lifeIII;

        public Stats(IColorModel colorModel)
        {
            _colorModel = colorModel;

            var playArea = Game.PlayArea;
            var width = Game.UIElementWidth;
            var hight = Game.TileHeight;

            _lifeI = new Bar(_colorModel.I,
                new RectangleF(
                    new PointF(playArea.Right, playArea.Top),
                    new SizeF(width, hight)),
                defaultMaxLifes);

            _lifeII = new Bar(_colorModel.II,
                new RectangleF(
                    new PointF(playArea.Right, playArea.Top + hight),
                    new SizeF(width, hight)),
                defaultMaxLifes);

            _lifeIII = new Bar(_colorModel.III,
                new RectangleF(
                    new PointF(playArea.Right, playArea.Top + hight * 2),
                    new SizeF(width, hight)),
                defaultMaxLifes);

            //Hack: Game is just paused on Game Over
            var a = new Action(() => Game.IsPaused = true);
            _lifeI.Resource.Empty += a;
            _lifeII.Resource.Empty += a;
            _lifeIII.Resource.Empty += a;
        }

        public void TakeTile(Tile tile)
        {
            var colorCode = _colorModel.ColorToCode(tile.Color);

            if (colorCode.HasFlag(ColorCode.I)) _lifeI.Resource.Current--;
            if (colorCode.HasFlag(ColorCode.II)) _lifeII.Resource.Current--;
            if (colorCode.HasFlag(ColorCode.III)) _lifeIII.Resource.Current--;
        }
    }
}
