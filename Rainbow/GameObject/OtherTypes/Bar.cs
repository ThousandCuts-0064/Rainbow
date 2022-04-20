using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class Bar : GameObject
    {
        private readonly Pen _pen = new Pen(Color.Black, Game.Unit);
        private readonly SolidBrush _solidBrush;
        public RectangleF RectangleF { get; }
        public Resource Resource { get; }
        public Color Color { get => _solidBrush.Color; set => _solidBrush.Color = value; }

        public Bar(Color color, RectangleF rectangleF, float max, Layer layer = Layer.UI) : base(layer)
        {
            _solidBrush = new SolidBrush(color);
            RectangleF = rectangleF;
            Resource = new Resource(max);
        }

        public override void Draw(Graphics graphics)
        {
            var fillRectangle = RectangleF.Inflate(RectangleF, -Game.HalfUnit, -Game.HalfUnit);
            graphics.FillRectangle(_solidBrush,
                fillRectangle.X,
                fillRectangle.Y,
                fillRectangle.Width * Resource.GetPercent(),
                fillRectangle.Height);
            graphics.DrawRectangle(_pen,
                RectangleF.X + Game.HalfUnit,
                RectangleF.Y + Game.HalfUnit,
                RectangleF.Width - Game.Unit,
                RectangleF.Height - Game.Unit);
        }

        public override void Dispose()
        {
            base.Dispose();
            _pen.Dispose();
            _solidBrush.Dispose();
        }
    }
}
