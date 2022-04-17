using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rainbow
{
    public class Line : MapElement, ILine
    {
        public Pen Pen { get; }
        public PointF Point1 { get; private set; }
        public PointF Point2 { get; private set; }
        float ILine.Width => Pen.Width;

        public Line(Color color, PointF point1, PointF point2, float width)
        {
            Point1 = point1;
            Point2 = point2;
            Pen = new Pen(color, width);
        }

        public override void Draw(Graphics graphics) => graphics.DrawLine(Pen, Point1, Point2);
    }
}
