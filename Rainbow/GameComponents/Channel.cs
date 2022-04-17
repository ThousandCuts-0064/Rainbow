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
        public LinkedList<Tile> TileList { get; } = new LinkedList<Tile>();
        public Line BoarderRight { get; }
        public Line BoarderLeft { get; }
        public Line Finish { get; }
        public PointF PointSpawn { get; }
        public RectangleF RectangleF { get; }

        public Channel(RectangleF rectangleF)
        {
            RectangleF = rectangleF;

            var finishOffset = new SizeF(0, -Game.TileHeight * 2);

            BoarderLeft = new Line(_colorBorder,
                    new PointF(rectangleF.Left, rectangleF.Top),
                    new PointF(rectangleF.Left, rectangleF.Bottom),
                    Game.HalfUnit);

            BoarderRight = new Line(_colorBorder,
                    new PointF(rectangleF.Right, rectangleF.Top),
                    new PointF(rectangleF.Right, rectangleF.Bottom),
                    Game.HalfUnit);

            Finish = new Line(_colorFinish,
                    BoarderLeft.Point2 + finishOffset,
                    BoarderRight.Point2 + finishOffset,
                    Game.HalfUnit);
        }
    }
}
