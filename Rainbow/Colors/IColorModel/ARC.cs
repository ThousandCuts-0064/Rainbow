using CustomCollections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class ARC : IColorModel
    {
        private static readonly ReadOnlyMap<ColorCode, Color> _mapColor = new Map<ColorCode, Color>()
        {
            { ColorCode.None, Color.Black },

            { ColorCode.I, Color.FromArgb(0, 127, 255) }, // Azure
            { ColorCode.II, Color.FromArgb(255, 0, 127) }, // Rose
            { ColorCode.III, Color.FromArgb(127, 255, 0) }, // Chartreuse

            { ColorCode.I_II, Color.FromArgb(127, 0, 255) }, // Violet
            { ColorCode.I_III, Color.FromArgb(0, 255, 127) }, // Spring Green
            { ColorCode.II_III, Color.FromArgb(255, 127, 0) }, // Orange

            { ColorCode.All, Color.White },
        }.ToReadOnly();

        public string Name => nameof(ARC);

        public Color CodeToColor(ColorCode cc) => _mapColor.Forward[cc];
        public ColorCode ColorToCode(Color c) => _mapColor.Reverse[c];

        public Color Combine(Color c1, Color c2)
        {
            int sumR = byte.MaxValue - (c1.R + c2.R);
            int sumG = byte.MaxValue - (c1.G + c2.G);
            int sumB = byte.MaxValue - (c1.B + c2.B);

            int min =
                -Math.Min(byte.MinValue,
                    Math.Min(sumR,
                        Math.Min(sumG, sumB)));

            sumR += min;
            sumG += min;
            sumB += min;

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
