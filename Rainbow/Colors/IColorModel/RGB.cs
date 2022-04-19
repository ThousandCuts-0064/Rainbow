using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class RGB : IColorModel
    {
        private static readonly IReadOnlyMap<ColorCode, Color> _mapColor = new Map<ColorCode, Color>()
        {
            { ColorCode.None, Color.Black },

            { ColorCode.I, Color.Red },
            { ColorCode.II, Color.Lime }, // That's Green
            { ColorCode.III, Color.Blue },

            { ColorCode.I_II, Color.Yellow },
            { ColorCode.I_III, Color.Magenta },
            { ColorCode.II_III, Color.Cyan },

            { ColorCode.All, Color.White },
        };

        public string Name => nameof(RGB);

        public Color CodeToColor(ColorCode cc) => _mapColor.Forward[cc];
        public ColorCode ColorToCode(Color c) => _mapColor.Reverse[c];

        public Color Combine(Color c1, Color c2)
        {
            int sumR = c1.R + c2.R;
            int sumG = c1.G + c2.G;
            int sumB = c1.B + c2.B;

            int max = 
                Math.Max(byte.MaxValue, 
                    Math.Max(sumR,
                        Math.Max(sumG, sumB)));

            return Color.FromArgb(
                byte.MaxValue * sumR / max,
                byte.MaxValue * sumG / max,
                byte.MaxValue * sumB / max);
        }
    }
}
