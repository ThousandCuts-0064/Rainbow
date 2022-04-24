using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class GameImage : GameObject
    {
        private readonly Image _image;
        public RectangleF Rectangle { get; set; }


        public GameImage(Image image, RectangleF rectangle, Layer layer) : base(layer)
        {
            Rectangle = rectangle;
            _image = image.Resize((int)rectangle.Width, (int)rectangle.Height);
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawImage(_image, Rectangle);
        }
    }
}
