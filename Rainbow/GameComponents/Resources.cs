using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public static class Resources
    {
        private static readonly string _folder;
        private static readonly Image[] _birdyAnimation;
        public static Image ColorWheel { get; }
        public static Image Birdy { get; }
        public static Image[] BirdyAnimation => _birdyAnimation.ToArray();

        static Resources()
        {
            _folder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + $"\\{nameof(Resources)}\\";

            ColorWheel = LoadImage($"{nameof(ColorWheel)}.png");
            Birdy = LoadImage($"{nameof(Birdy)}.jpg");
            _birdyAnimation = BirdyImageToAnimation(LoadImage($"{nameof(BirdyAnimation)}.png"));
        }

        private static Image LoadImage(string name) =>
            Image.FromFile(_folder + name);

        private static Image[] BirdyImageToAnimation(Image image)
        {
            Bitmap bitmap = new Bitmap(image);
            Image[] images = new Image[25];
            int rows = 3;
            int columns = 9;
            int space = 60;
            Size single = new Size((image.Width - space * (columns - 1)) / columns, image.Height / rows);
            Rectangle current = new Rectangle(new Point(0, 0), single);
            for (int c = 0; c < columns - 1; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    images[c * rows + r] = bitmap.Clone(current, image.PixelFormat);
                    current.Location.Offset(0, single.Height);
                }
                current.Offset(single.Width + space, 0);
                current.Y = 0;
            }
            images[images.Length - 1] = bitmap.Clone(current, image.PixelFormat);

            return images;
        }
    }
}
