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
        private readonly Pen _pen = new Pen(Color.Black, Game.Unit);
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
            RectangleF rectangleF = new RectangleF(Location.X, Location.Y, Game.TileWidth, Game.TileHeight);
            Fill(graphics, rectangleF);
            rectangleF.Inflate(-Game.Unit / 2, -Game.Unit / 2);
            Border(graphics, rectangleF);
        }

        public override void Dispose()
        {
            base.Dispose();
            _solidBrush.Dispose();
            _pen.Dispose();
        }

        protected override void Update() => Location = new PointF(Location.X, 
            Location.Y + Game.TileSpeed * Game.Unit * Game.DeltaTime);

        protected virtual void Fill(Graphics graphics, RectangleF rectangleF) =>
            graphics.FillRectangle(_solidBrush, rectangleF);

        protected virtual void Border(Graphics graphics, RectangleF rectangleF) =>
            graphics.DrawRectangle(_pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
    }
}
