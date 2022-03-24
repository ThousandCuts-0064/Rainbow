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
        private static readonly Dictionary<Color, ColorCode> _colorToColorCode = new Dictionary<Color, ColorCode>()
        {
            { Color.Red, ColorCode.I },
            { Color.Green, ColorCode.II },
            { Color.Blue, ColorCode.III },
            { Color.Yellow, ColorCode.I_II },
            { Color.Purple, ColorCode.I_III },
            { Color.Cyan, ColorCode.II_III },
            { Color.White, ColorCode.All },
        };
        private static readonly Dictionary<ColorCode, Color> _colorCodeToColor = new Dictionary<ColorCode, Color>()
        {
            { ColorCode.I, Color.Red },
            { ColorCode.II, Color.Green},
            { ColorCode.III, Color.Blue},
            { ColorCode.I_II, Color.Yellow},
            { ColorCode.I_III, Color.Purple},
            { ColorCode.II_III, Color.Cyan},
            { ColorCode.All, Color.White},
        };

        public Color I => Color.Red;
        public Color II => Color.Green;
        public Color III => Color.Blue;
        public Color I_II => Color.Yellow;
        public Color I_III => Color.Purple;
        public Color II_III => Color.Cyan;
        public Color All => Color.White;

        public Color CodeToColor(ColorCode cc) => _colorCodeToColor[cc];
        public ColorCode ColorToCode(Color c) => _colorToColorCode[c];

        public Color Combine(Color c1, Color c2) => Color.FromArgb((c1.R + c2.R) / 2, (c1.G + c2.G) / 2, (c1.B + c2.B) / 2);
    }
}
