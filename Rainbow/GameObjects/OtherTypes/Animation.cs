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
        private readonly Image[] _flipped;
        public int Index { get; private set; }
        public RectangleF Rectangle { get; set; }
        public bool IsFlipped { get; set; }

        /// <summary>
        /// Simple animation that cant be flipped and changing IsFlipped will throw an error
        /// </summary>
        /// <param name="images"></param>
        /// <param name="rectangle"></param>
        /// <param name="layer"></param>
        public Animation(Image[] images, RectangleF rectangle, Layer layer) : base(layer)
        {
            _images = images;
            Rectangle = rectangle;
        }

        /// <summary>
        /// Using this constructors signals that the animation can bi flipped.
        /// If flipped is null, it will be calcualted from images flipping pixels on the X axis
        /// </summary>
        /// <param name="images"></param>
        /// <param name="flipped"></param>
        /// <param name="rectangle"></param>
        /// <param name="layer"></param>
        public Animation(Image[] images, Image[] flipped, RectangleF rectangle, Layer layer) : this (images, rectangle, layer)
        {
            if (flipped == null)
            {
                _flipped = new Image[images.Length];
                for (int i = 0; i < images.Length; i++)
                {
                    _flipped[i] = (Image)images[i].Clone();
                    _flipped[i].RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
            }
            else _flipped = flipped;
        }

        public override void Draw(Graphics graphics) => 
            graphics.DrawImage((IsFlipped ? _flipped : _images)[Index == _images.Length ? Index = 0 : Index++], Rectangle);
    }
}
