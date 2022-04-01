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
            { ColorCode.I, Color.FromArgb(255, 127, 0) }, // Orange
            { ColorCode.II, Color.FromArgb(0, 255, 127) }, // Spring Green
            { ColorCode.III, Color.FromArgb(127, 0, 255) }, // Violet

            { ColorCode.I_II, Color.FromArgb(127, 255, 0) }, // Chartreuse
            { ColorCode.I_III, Color.FromArgb(255, 0, 127) }, // Rose
            { ColorCode.II_III, Color.FromArgb(0, 127, 255) }, // Azure

            { ColorCode.All, Color.Black },
        };

        public Color CodeToColor(ColorCode cc) => _mapColor.Forward[cc];
        public ColorCode ColorToCode(Color c) => _mapColor.Reverse[c];

        public Color Combine(Color c1, Color c2) => 
            throw new NotImplementedException();

        public override string ToString() => nameof(OSV);
    }
}
