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
        private readonly Line _boarderRight;
        private readonly Line _boarderLeft;
        private readonly Line _finish;
        private readonly GameString _hintButtons;
        private readonly GameModifiers _gameModifiers;
        public LinkedList<Tile> TileList { get; } = new LinkedList<Tile>();
        public PointF PointSpawn { get; }
        public RectangleF Rectangle { get; }
        public int Index { get; }
        public IReadOnlyLine BoarderRight => _boarderRight;
        public IReadOnlyLine BoarderLeft => _boarderLeft;
        public IReadOnlyLine Finish => _finish;

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
    }
}
