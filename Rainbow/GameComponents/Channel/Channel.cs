using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public class Channel : IReadOnlyChannel
    {
        private static readonly Color _colorBorder = Color.Black;
        private static readonly Color _colorFinish = Color.Black;
        private static SizeF DefaultFinishOffset => new SizeF(0, -Game.TileHeight * 2);
        private readonly LinkedList<Tile> _tileList = new LinkedList<Tile>();
        private readonly Dictionary<Tile, LinkedListNode<Tile>> _tileToNode = new Dictionary<Tile, LinkedListNode<Tile>>();
        private readonly Line _boarderRight;
        private readonly Line _boarderLeft;
        private readonly Line _finish;
        private readonly GameString _hintButtons;
        private readonly GameModifiers _gameModifiers;
        public LinkedListNode<Tile> TileNodeFirst => _tileList.First;
        public LinkedListNode<Tile> TileNodeLast => _tileList.Last;
        public int TileCount => _tileList.Count;
        public PointF PointSpawn { get; }
        public RectangleF Rectangle { get; }
        public int Index { get; }
        public IReadOnlyLine BoarderRight => _boarderRight;
        public IReadOnlyLine BoarderLeft => _boarderLeft;
        public IReadOnlyLine Finish => _finish;

        public event Action<Tile> TilePassed;
        public event Action<Tile> NoClickTileClicked;
        public event Action<Tile> TileRemoved;

        public Channel(RectangleF rectangle, GameModifiers gameModifiers, float boarderWidth, int index)
        {
            Index = index;
            Rectangle = rectangle;
            _gameModifiers = gameModifiers;
            var boarderHalfWidth = boarderWidth * 0.5f;

            _boarderLeft = new Line(_colorBorder,
                    new PointF(rectangle.Left + boarderHalfWidth, rectangle.Top),
                    new PointF(rectangle.Left + boarderHalfWidth, rectangle.Bottom),
                    Layer.Map,
                    boarderWidth);

            _boarderRight = new Line(_colorBorder,
                    new PointF(rectangle.Right - boarderHalfWidth, rectangle.Top),
                    new PointF(rectangle.Right - boarderHalfWidth, rectangle.Bottom),
                    Layer.Map,
                    boarderWidth);

            _finish = new Line(_colorFinish,
                    _boarderLeft.Point2 + DefaultFinishOffset,
                    _boarderRight.Point2 + DefaultFinishOffset,
                    Layer.Map,
                    Game.HalfUnit);

            PointSpawn = new PointF(rectangle.Location.X + boarderWidth, rectangle.Top - Game.TileHeight);

            if (gameModifiers.HasFlag(GameModifiers.HintColumns))
                _hintButtons = new GameString(
                    new ColorColumn(ColorCode.All, index).ToInput(),
                    RectangleF.FromLTRB(_finish.Point1.X, _finish.Point1.Y, _boarderRight.Point2.X, _boarderRight.Point2.Y),
                    Color.Black,
                    Layer.Map);
        }

        public void TileListAddFirst(Tile tile)
        {
            _tileToNode.Add(tile, _tileList.AddFirst(tile));
            tile.Disposing += TileListRemove;
            tile.ControlLost += TileListRemove;
            tile.ControlLost += t => t.Disposing -= TileListRemove;
        }

        public void OnTick()
        {
            if (_tileList.Count == 0) return;

            var tile = _tileList.Last.Value;
            if (tile.Location.Y < BoarderLeft.Point2.Y) return;

            TilePassed?.Invoke(tile);
            tile.Dispose();
        }

        public void OnColorInput(ColorCode colorCode)
        {
            if (_tileList.Count == 0) return;

            var lastTile = _tileList.Last.Value;
            while (lastTile.Location.Y + Game.TileHeight >= Finish.Point1.Y)
            {
                var previousNode = _tileToNode[lastTile].Previous;

                if (lastTile.ColorCode == colorCode)
                {
                    lastTile.Click();
                    if (lastTile.Lives <= 0 &&
                        lastTile.IsNoClick)
                        NoClickTileClicked?.Invoke(lastTile);
                }

                if (previousNode == null) return;
                lastTile = previousNode.Value;
            }
        }

        public void OnShotgunUsed()
        {
            var tileList = _tileList;
            while (tileList.Count > 0 &&
                tileList.Last.Value.Location.Y + Game.TileHeight >= Finish.Point1.Y)
                tileList.Last.Value.Dispose();
        }

        private void TileListRemove(Tile tile)
        {
            _tileList.Remove(_tileToNode[tile]);
            _tileToNode.Remove(tile);
            TileRemoved?.Invoke(tile);
        }
    }
}
