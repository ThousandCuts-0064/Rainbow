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
                Blend(@base.R, target.R, ratio),
                Blend(@base.G, target.G, ratio),
                Blend(@base.B, target.B, ratio));

        public static Color Invert(this Color color) =>
            Color.FromArgb(
                Invert(color.R),
                Invert(color.G),
                Invert(color.B));

        public static ColorComplexity ToComplexity(this ColorCode code)
        {
            if (code < ColorCode.I || code > ColorCode.All) return ColorComplexity.Invalid;
            //1 bit is set
            if ((code & (code - 1)) != 0) return ColorComplexity.Primary;
            if (code == ColorCode.All) return ColorComplexity.All;
            return ColorComplexity.Secondary;
        }

        private static int Blend(byte @base, byte target, float ratio) =>
            (int)((@base * 2 * (1 - ratio) + target * 2 * ratio) / 2);

        private static int Invert(byte specter) =>
            byte.MaxValue - specter;
    }
}
