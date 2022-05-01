using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class Animation : GameObject
    {
        private readonly Image[] _images;
        public int Index { get; private set; }
        public RectangleF Rectangle { get; set; }

        public Animation(Image[] images, RectangleF rectangle, Layer layer) : base(layer)
        {
            _images = images;
            Rectangle = rectangle;
        }

        public override void Draw(Graphics graphics) => 
            graphics.DrawImage(_images[Index == _images.Length ? Index = 0 : Index++], Rectangle);
    }
}
