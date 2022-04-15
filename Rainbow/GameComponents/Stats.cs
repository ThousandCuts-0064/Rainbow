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
        private const int DEFAULT_MAX_SHOTGUNS = 10;
        private readonly LinkedList<Tile>[] _tileLists;
        private readonly IColorModel _colorModel;
        private readonly Bar _lifeI;
        private readonly Bar _lifeII;
        private readonly Bar _lifeIII;
        private readonly Bar _barShotgun;
        private readonly int _level;

        public Stats(LinkedList<Tile>[] tileLists, IColorModel colorModel, int level)
        {
            _colorModel = colorModel;
            _tileLists = tileLists;
            _level = level;

            var playArea = Game.PlayArea;
            var width = Game.UIElementWidth;
            var height = Game.TileHeight;

            _lifeI = new Bar(
                colorModel.CodeToColor(ColorCode.I),
                new RectangleF(
                    new PointF(playArea.Right, playArea.Top),
                    new SizeF(width, height)),
                DEFAULT_MAX_LIFES);

            _lifeII = new Bar(
                colorModel.CodeToColor(ColorCode.II),
                new RectangleF(
                    new PointF(playArea.Right, playArea.Top + height),
                    new SizeF(width, height)),
                DEFAULT_MAX_LIFES);

            _lifeIII = new Bar(
                colorModel.CodeToColor(ColorCode.III),
                new RectangleF(
                    new PointF(playArea.Right, playArea.Top + height * 2),
                    new SizeF(width, height)),
                DEFAULT_MAX_LIFES);

            _barShotgun = new Bar(
                Color.FromArgb(192, 192, 192),
                new RectangleF(
                    new PointF(playArea.Right, playArea.Top + height * 3),
                    new SizeF(width, height)),
                DEFAULT_MAX_SHOTGUNS);

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
                if (_tileLists[i].Count != 0 &&
                    _tileLists[i].Last.Value.Location.Y > Game.Boarders[i].Second.Y)
                {
                    var tile = _tileLists[i].Last.Value;
                    _tileLists[i].RemoveLast();
                    TakeTile(tile);
                    tile.Dispose();
                }
            }
        }

        public void ColorInput(ColorCode colorCode, int column)
        {
            var tileList = _tileLists[column];
            if (tileList.Count == 0) return;

            var firstTile = tileList.Last.Value;
            while (firstTile.Location.Y + Game.TileHeight > Game.Finishes[column].First.Y)
            {
                if (firstTile.ColorCode != colorCode)
                {
                    var previous = tileList.FindLast(firstTile).Previous;
                    if (previous == null) return;
                    firstTile = previous.Value;
                    continue;
                }

                firstTile.Click();
                var node = tileList.FindLast(firstTile);
                var perviousNode = node.Previous;
                if (firstTile.Lives <= 0)
                {
                    tileList.Remove(node);
                    if (firstTile.IsNoClick) TakeNoClickTile(firstTile);
                }

                if (perviousNode != null)
                    firstTile = perviousNode.Value;
            }
        }

        public void UseShotgun()
        {
            if (_barShotgun.Resource.Current == 0) return;
            for (int i = 0; i < _level; i++)
            {
                var tileList = _tileLists[i];
                while (tileList.Count > 0 &&
                    tileList.Last.Value.Location.Y + Game.TileHeight >= Game.Finishes[i].First.Y)
                {
                    tileList.Last.Value.Dispose();
                    tileList.RemoveLast();
                }
            }
            _barShotgun.Resource.Current--;
        }

        private void TakeTile(Tile tile)
        {
            if (tile.IsNoClick)
            {
                TakeNoClickTile(tile);
                return;
            }

            if (tile.ColorCode.HasFlag(ColorCode.I)) _lifeI.Resource.Current -= tile.Lives;
            if (tile.ColorCode.HasFlag(ColorCode.II)) _lifeII.Resource.Current -= tile.Lives;
            if (tile.ColorCode.HasFlag(ColorCode.III)) _lifeIII.Resource.Current -= tile.Lives;
        }

        private void TakeNoClickTile(Tile tile)
        {
            if (tile.ColorCode.HasFlag(ColorCode.I)) _lifeI.Resource.Current -= tile.TimesClicked;
            if (tile.ColorCode.HasFlag(ColorCode.II)) _lifeII.Resource.Current -= tile.TimesClicked;
            if (tile.ColorCode.HasFlag(ColorCode.III)) _lifeIII.Resource.Current -= tile.TimesClicked;
        }
    }
}
