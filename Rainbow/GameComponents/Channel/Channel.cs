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
        private readonly GameModifiers _gameModifiers;
        private readonly GameString _hintButtons;
        public LinkedList<Tile> TileList { get; } = new LinkedList<Tile>();
        public Line BoarderRight { get; }
        public Line BoarderLeft { get; }
        public Line Finish { get; }
        public PointF PointSpawn { get; }
        public RectangleF Rectangle { get; }
        IReadOnlyLine IReadOnlyChannel.BoarderRight => BoarderRight;
        IReadOnlyLine IReadOnlyChannel.BoarderLeft => BoarderLeft;
        IReadOnlyLine IReadOnlyChannel.Finish => Finish;

        public Channel(RectangleF rectangle, GameModifiers gameModifiers, float boarderWidth, int index)
        {
            Rectangle = rectangle;
            _gameModifiers = gameModifiers;
            var boarderHalfWidth = boarderWidth * 0.5f;

            BoarderLeft = new Line(_colorBorder,
                    new PointF(rectangle.Left + boarderHalfWidth, rectangle.Top),
                    new PointF(rectangle.Left + boarderHalfWidth, rectangle.Bottom),
                    Layer.Map,
                    boarderWidth);

            BoarderRight = new Line(_colorBorder,
                    new PointF(rectangle.Right - boarderHalfWidth, rectangle.Top),
                    new PointF(rectangle.Right - boarderHalfWidth, rectangle.Bottom),
                    Layer.Map,
                    boarderWidth);

            Finish = new Line(_colorFinish,
                    BoarderLeft.Point2 + DefaultFinishOffset,
                    BoarderRight.Point2 + DefaultFinishOffset,
                    Layer.Map,
                    Game.HalfUnit);

            PointSpawn = new PointF(rectangle.Location.X + boarderWidth, rectangle.Top - Game.TileHeight);

            if (gameModifiers.HasFlag(GameModifiers.HintColumns))
                _hintButtons = new GameString(
                    new ColorColumn(ColorCode.All, index).ToInput(),
                    RectangleF.FromLTRB(Finish.Point1.X, Finish.Point1.Y, BoarderRight.Point2.X, BoarderRight.Point2.Y),
                    Color.Black,
                    Layer.Map);
        }
    }
}
