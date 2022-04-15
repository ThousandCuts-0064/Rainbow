using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class Birdy : GameplayElement
    {
        private Image _image;
        private Tile _target;
        private SizeF _size;

        public Birdy()
        {
            _size = new SizeF(Game.TileHeight, Game.TileHeight);
            _image = Resources.Birdy.Resize((int)_size.Width, (int)_size.Height);
        }

        public override PointF GetCenter() =>
            new PointF(
                Location.X + _size.Width * 0.5f,
                Location.Y + _size.Height * 0.5f);

        public override void Draw(Graphics graphics) =>
            graphics.DrawImage(_image, Location);

        protected override void Update()
        {
            var speed = Game.TileUnitsPerSecond * Game.Unit * Game.DeltaTime * 2;
            var targetCenter = _target.GetCenter();
            var center = GetCenter();
            var direction = new PointF(
                targetCenter.X - center.X, 
                targetCenter.Y - center.Y);
            var magnitude = (float)Math.Sqrt(
                direction.X * direction.X + 
                direction.Y + direction.Y);
            var normalized = new PointF(
                direction.X / magnitude,
                direction.Y / magnitude);
            var scaled = new PointF(
                normalized.X * speed,
                normalized.Y * speed);
            Location = new PointF(
                Location.X + scaled.X,
                Location.Y + scaled.Y);
        }
    }
}
