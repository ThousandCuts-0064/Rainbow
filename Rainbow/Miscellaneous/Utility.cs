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
    public static class Utility
    {
        public static TSource MaxBy<TSource, TComparable>(this IEnumerable<TSource> source, Func<TSource, TComparable> selector) 
            where TComparable : IComparable<TComparable> =>
            source.Select(s => (element: s, value: selector(s))).Aggregate((max, next) => max.value.CompareTo(next.value) < 0 ? next : max).element;

        public static TSource MinBy<TSource, TComparable>(this IEnumerable<TSource> source, Func<TSource, TComparable> selector)
            where TComparable : IComparable<TComparable> =>
            source.Select(s => (element: s, value: selector(s))).Aggregate((min, next) => min.value.CompareTo(next.value) > 0 ? next : min).element;

        public static string TrimEnd(this string str, int count) =>
            str.Substring(0, str.Length - count);

        public static bool HasAnyFlag(this Enum @enum, Enum flags) =>
            (Convert.ToInt64(@enum) & Convert.ToInt64(flags)) != 0;

        public static int CountBits(this int i)
        {
            int count = 0;
            while (i != 0)
            {
                count += i & 1;
                i >>= 1;
            }
            return count;
        }

        public static List<int> AddBitsToList(this int i, List<int> list)
        {
            list = list ?? new List<int>();
            for (int y = 0; i != 0; i >>= 1)
            {
                if ((i & 1) == 1) list.Add(1 << y);
                y++;
            }
            return list;
        }

        public static void DrawRectangle(this Graphics graphics, Pen pen, RectangleF rectangle) =>
            graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

        public static PointF OffsetNew(this PointF point, float x, float y) =>
            new PointF(point.X + x, point.Y + y);

        public static Point OffsetNew(this Point point, int x, int y) =>
            new Point(point.X + x, point.Y + y);

        public static PointF GetCenter(this RectangleF rectangle) =>
            new PointF(rectangle.X + rectangle.Width * 0.5f, rectangle.Y + rectangle.Height * 0.5f);

        public static Bitmap Resize(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static string ToShortString(this GameModifiers modifier)
        {
            switch (modifier) // Note: 4 chars max
            {
                case GameModifiers.NoClickTiles: return "NClT";
                case GameModifiers.DoubleTiles: return "DT";
                case GameModifiers.TripleTiles: return "TT";
                case GameModifiers.DoubleClickTiles: return "DCT";
                case GameModifiers.TripleClickTiles: return "TCT";
                case GameModifiers.NoColorTiles: return "NCoT";
                case GameModifiers.ColorDistortion: return "CD";
                case GameModifiers.InvertedColors: return "IC";
                case GameModifiers.FadingColors: return "FaC";
                case GameModifiers.FlashingColors: return "FlC";
                case GameModifiers.ColorWheel: return "CW";
                case GameModifiers.ColorfulBack: return "CB";
                case GameModifiers.HintColumns: return "HC";
                case GameModifiers.HintButtons: return "HB";
                case GameModifiers.Birdy: return "B";
                case GameModifiers.MessageEvent: return "ME";
                case GameModifiers.ShotgunEvent: return "SE";
                case GameModifiers.DiamondEvent: return "DE";
                case GameModifiers.ChessEvent: return "CE";
                case GameModifiers.RainbowEvent: return "RE";
                default: throw new ArgumentException($"{modifier} is not supported");
            }
        }
    }
}
