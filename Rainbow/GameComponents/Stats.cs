using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Rainbow.Game;

namespace Rainbow
{
    public class Stats
    {
        private const int DEFAULT_MAX_LIFES = 10;
        private const int DEFAULT_MAX_SHOTGUNS = 10;
        private readonly IColorModel _colorModel;
        private readonly Bar _lifeI;
        private readonly Bar _lifeII;
        private readonly Bar _lifeIII;
        private readonly Bar _barShotgun;
        private readonly int _level;

        public event Action Death;
        public event Action ShotgunUsed;

        public Stats(IColorModel colorModel, int level)
        {
            _colorModel = colorModel;
            _level = level;

            var playArea = Game.PlayArea;
            var locationX = Game.BoarderRight.Point1.X + Game.BoarderRight.Width / 2;
            var width = Game.UIElementWidth;
            var height = Game.TileHeight;

            _lifeI = new Bar(
                colorModel.CodeToColor(ColorCode.I),
                new RectangleF(
                    new PointF(locationX, playArea.Top),
                    new SizeF(width, height)),
                DEFAULT_MAX_LIFES);

            _lifeII = new Bar(
                colorModel.CodeToColor(ColorCode.II),
                new RectangleF(
                    new PointF(locationX, playArea.Top + height),
                    new SizeF(width, height)),
                DEFAULT_MAX_LIFES);

            _lifeIII = new Bar(
                colorModel.CodeToColor(ColorCode.III),
                new RectangleF(
                    new PointF(locationX, playArea.Top + height * 2),
                    new SizeF(width, height)),
                DEFAULT_MAX_LIFES);

            _barShotgun = new Bar(
                Color.FromArgb(192, 192, 192),
                new RectangleF(
                    new PointF(locationX, playArea.Top + height * 3),
                    new SizeF(width, height)),
                DEFAULT_MAX_SHOTGUNS);

            _lifeI.Resource.Empty += () => Death?.Invoke();
            _lifeII.Resource.Empty += () => Death?.Invoke();
            _lifeIII.Resource.Empty += () => Death?.Invoke();
        }

        public void OnTick()
        {
            if (Game.Ticks % 1000 != 0) return;

            _barShotgun.Resource.Current++;
            _lifeI.Resource.Current++;
            _lifeII.Resource.Current++;
            _lifeIII.Resource.Current++;
        }

        public void OnShotgunPressed()
        {
            if (_barShotgun.Resource.Current == 0) return;
            ShotgunUsed?.Invoke();
            _barShotgun.Resource.Current--;
        }

        public void OnTakeTile(Tile tile)
        {
            if (tile.IsNoClick)
            {
                OnTakeNoClickTile(tile);
                return;
            }

            if (tile.ColorCode.HasFlag(ColorCode.I)) _lifeI.Resource.Current -= tile.Lives;
            if (tile.ColorCode.HasFlag(ColorCode.II)) _lifeII.Resource.Current -= tile.Lives;
            if (tile.ColorCode.HasFlag(ColorCode.III)) _lifeIII.Resource.Current -= tile.Lives;
        }

        public void OnTakeNoClickTile(Tile tile)
        {
            if (tile.ColorCode.HasFlag(ColorCode.I)) _lifeI.Resource.Current -= tile.TimesClicked;
            if (tile.ColorCode.HasFlag(ColorCode.II)) _lifeII.Resource.Current -= tile.TimesClicked;
            if (tile.ColorCode.HasFlag(ColorCode.III)) _lifeIII.Resource.Current -= tile.TimesClicked;
        }
    }
}
