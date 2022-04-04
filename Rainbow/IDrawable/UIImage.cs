using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class UIImage : UIElement
    {
        private Image _image;
        private RectangleF _rectangleF;

        public UIImage(Image image, RectangleF rectangleF)
        {
            _image = image;
            _rectangleF = rectangleF;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawImage(_image, _rectangleF);
        }
    }
}
