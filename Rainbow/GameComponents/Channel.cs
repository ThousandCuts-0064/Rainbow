using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public class Channel
    {
        private static readonly Color _colorBorder = Color.Black;
        private static readonly Color _colorFinish = Color.Black;
        private static SizeF DefaultFinishOffset => new SizeF(0, -Game.TileHeight* 2);
        public LinkedList<Tile> TileList { get; } = new LinkedList<Tile>();
        public Line BoarderRight { get; }
        public Line BoarderLeft { get; }
        public Line Finish { get; }
        public PointF PointSpawn { get; }
        public RectangleF RectangleF { get; }

        public Channel(RectangleF rectangleF, float boarderWidth)
        {
            RectangleF = rectangleF;
            var boarderHalfWidth = boarderWidth * 0.5f;

            BoarderLeft = new Line(_colorBorder,
                    new PointF(rectangleF.Left + boarderHalfWidth, rectangleF.Top),
                    new PointF(rectangleF.Left + boarderHalfWidth, rectangleF.Bottom),
                    boarderWidth);

            BoarderRight = new Line(_colorBorder,
                    new PointF(rectangleF.Right + boarderHalfWidth, rectangleF.Top),
                    new PointF(rectangleF.Right + boarderHalfWidth, rectangleF.Bottom),
                    boarderWidth);

            Finish = new Line(_colorFinish,
                    BoarderLeft.Point2 + DefaultFinishOffset,
                    BoarderRight.Point2 + DefaultFinishOffset,
                    Game.HalfUnit);
        }
    }
}
