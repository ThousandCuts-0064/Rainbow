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
        private RectangleF _rectangleF;

        public GameImage(Image image, RectangleF rectangleF, Layer layer) : base(layer)
        {
            _image = image.Resize((int)rectangleF.Width, (int)rectangleF.Height);
            _rectangleF = rectangleF;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawImage(_image, _rectangleF);
        }
    }
}
