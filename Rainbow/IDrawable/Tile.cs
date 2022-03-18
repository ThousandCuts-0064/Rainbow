using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rainbow
{
    public class Tile : GameplayElement
    {
        private readonly SolidBrush _solidBrush;
        private readonly Pen _pen = new Pen(Color.Black);
        public Color Color { get; private set; }
        public PointF Location { get; private set; }

        public Tile(Color fill, PointF location)
        {
            Color = fill;
            _solidBrush = new SolidBrush(fill);
            Location = location;
        }

        public override void Draw(Graphics graphics)
        {
            RectangleF rectangleF = new RectangleF(Location.X, Location.Y, Manager.TileWidth, Manager.TileHeight);
            Fill(graphics, rectangleF);
            Border(graphics, rectangleF);
        }

        protected override void Update() => Location = new PointF(Location.X, Location.Y + Manager.GameSpeed);

        protected virtual void Fill(Graphics graphics, RectangleF rectangleF) =>
            graphics.FillRectangle(_solidBrush, rectangleF);

        protected virtual void Border(Graphics graphics, RectangleF rectangleF) =>
            graphics.DrawRectangle(_pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
    }
}
