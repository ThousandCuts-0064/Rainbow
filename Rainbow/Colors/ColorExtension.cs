using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public static class ColorExtension
    {
        public static Color Blend(this Color @base, Color target, float ratio) =>
            Color.FromArgb(
                (int)Math2.Lerp(@base.R, target.R, ratio),
                (int)Math2.Lerp(@base.G, target.G, ratio),
                (int)Math2.Lerp(@base.B, target.B, ratio));

        public static Color Invert(this Color color) =>
            Color.FromArgb(
                Invert(color.R),
                Invert(color.G),
                Invert(color.B));

        public static ColorComplexity ToComplexity(this ColorCode code)
        {
            if (code < ColorCode.I || code > ColorCode.All) return ColorComplexity.None;
            //1 bit is set
            if ((code & (code - 1)) != 0) return ColorComplexity.Primary;
            if (code == ColorCode.All) return ColorComplexity.All;
            return ColorComplexity.Secondary;
        }

        private static int Invert(byte specter) =>
            byte.MaxValue - specter;
    }
}
