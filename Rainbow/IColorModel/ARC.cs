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
        private static readonly IReadOnlyMap<ColorCode, Color> _mapColor = new Map<ColorCode, Color>()
        {
            { ColorCode.I, Color.FromArgb(0, 127, 255) }, // Azure
            { ColorCode.II, Color.FromArgb(255, 0, 127) }, // Rose
            { ColorCode.III, Color.FromArgb(127, 255, 0) }, // Chartreuse

            { ColorCode.I_II, Color.FromArgb(127, 0, 255) }, // Violet
            { ColorCode.I_III, Color.FromArgb(0, 255, 127) }, // Spring Green
            { ColorCode.II_III, Color.FromArgb(255, 127, 0) }, // Orange

            { ColorCode.All, Color.Black },
        };

        public Color CodeToColor(ColorCode cc) => _mapColor.Forward[cc];
        public ColorCode ColorToCode(Color c) => _mapColor.Reverse[c];

        public Color Combine(Color c1, Color c2) =>
            throw new NotImplementedException();

        public override string ToString() => nameof(ARC);
    }
}
