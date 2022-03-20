using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class Bar : UIElement
    {
        private readonly Pen _pen = new Pen(Color.Black);
        private readonly SolidBrush _solidBrush;
        public Color Color { get => _solidBrush.Color; set => _solidBrush.Color = value; }
        public RectangleF RectangleF { get; set; }

        public Bar(Color color, RectangleF rectangleF)
        {
            _solidBrush = new SolidBrush(color);
            RectangleF = rectangleF;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(_solidBrush, RectangleF);
            graphics.DrawRectangle(_pen, RectangleF.X, RectangleF.Y, RectangleF.Width, RectangleF.Height);
        }

        public override void Dispose()
        {
            base.Dispose();
            _pen.Dispose();
            _solidBrush.Dispose();
        }
    }
}
