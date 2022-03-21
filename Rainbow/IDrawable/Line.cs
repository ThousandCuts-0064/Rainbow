using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rainbow
{
    class Line : MapElement, ILine
    {
        public Pen Pen { get; }
        public PointF First { get; private set; }
        public PointF Second { get; private set; }

        public Line(Color color, PointF first, PointF second)
        {
            First = first;
            Second = second;
            Pen = new Pen(color, Game.Unit / 5);
        }

        public override void Draw(Graphics graphics) => graphics.DrawLine(Pen, First, Second);
    }
}
