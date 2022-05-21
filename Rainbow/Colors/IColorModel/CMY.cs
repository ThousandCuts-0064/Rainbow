using CustomCollections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class CMY : IColorModel
    {
        private static readonly ReadOnlyMap<ColorCode, Color> _mapColor = new Map<ColorCode, Color>()
        {
            { ColorCode.None, Color.White },

            { ColorCode.I, Color.Cyan },
            { ColorCode.II, Color.Magenta },
            { ColorCode.III, Color.Yellow },

            { ColorCode.I_II, Color.Blue },
            { ColorCode.I_III, Color.Lime }, // That's Green
            { ColorCode.II_III, Color.Red },

            { ColorCode.All, Color.Black },
        }.ToReadOnly();

        public string Name => nameof(CMY);

        public Color CodeToColor(ColorCode cc) => _mapColor.Forward[cc];
        public ColorCode ColorToCode(Color c) => _mapColor.Reverse[c];

        public Color Combine(Color c1, Color c2)
        {
            int sumR = byte.MaxValue * 2 - (c1.R + c2.R);
            int sumG = byte.MaxValue * 2 - (c1.G + c2.G);
            int sumB = byte.MaxValue * 2 - (c1.B + c2.B);

            int max =
                Math.Max(byte.MaxValue,
                    Math.Max(sumR,
                        Math.Max(sumG, sumB)));

            return Color.FromArgb(
                byte.MaxValue - byte.MaxValue * sumR / max,
                byte.MaxValue - byte.MaxValue * sumG / max,
                byte.MaxValue - byte.MaxValue * sumB / max);
        }
    }
}
