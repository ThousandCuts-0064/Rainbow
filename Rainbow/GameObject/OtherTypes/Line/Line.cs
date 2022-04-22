using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rainbow
{
    public class Line : GameObject, IReadOnlyLine
    {
        public Pen Pen { get; }
        public PointF Point1 { get; private set; }
        public PointF Point2 { get; private set; }
        float IReadOnlyLine.Width => Pen.Width;

        public Line(Color color, PointF point1, PointF point2, Layer layer, float width) : base(layer)
        {
            Point1 = point1;
            Point2 = point2;
            Pen = new Pen(color, width);
        }

        public override void Draw(Graphics graphics) => graphics.DrawLine(Pen, Point1, Point2);
    }
}
