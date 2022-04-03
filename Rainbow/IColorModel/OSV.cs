using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class OSV : IColorModel
    {
        private static readonly IReadOnlyMap<ColorCode, Color> _mapColor = new Map<ColorCode, Color>()
        {
            { ColorCode.None, Color.White },

            { ColorCode.I, Color.FromArgb(255, 127, 0) }, // Orange
            { ColorCode.II, Color.FromArgb(0, 255, 127) }, // Spring Green
            { ColorCode.III, Color.FromArgb(127, 0, 255) }, // Violet

            { ColorCode.I_II, Color.FromArgb(127, 255, 0) }, // Chartreuse
            { ColorCode.I_III, Color.FromArgb(255, 0, 127) }, // Rose
            { ColorCode.II_III, Color.FromArgb(0, 127, 255) }, // Azure

            { ColorCode.All, Color.Black },
        };

        public string Name => nameof(OSV);

        public Color CodeToColor(ColorCode cc) => _mapColor.Forward[cc];
        public ColorCode ColorToCode(Color c) => _mapColor.Reverse[c];

        public Color Combine(Color c1, Color c2)
        {
            int sumR = c1.R + c2.R - byte.MaxValue;
            int sumG = c1.G + c2.G - byte.MaxValue;
            int sumB = c1.B + c2.B - byte.MaxValue;

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
                byte.MaxValue * sumR / max,
                byte.MaxValue * sumG / max,
                byte.MaxValue * sumB / max);
        }
    }
}
