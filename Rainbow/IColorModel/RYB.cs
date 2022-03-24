using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class RYB : IColorModel
    {
        private static readonly Dictionary<Color, ColorCode> _colorToColorCode = new Dictionary<Color, ColorCode>()
        {
            { Color.Red, ColorCode.I },
            { Color.Yellow, ColorCode.II },
            { Color.Blue, ColorCode.III },
            { Color.Orange, ColorCode.I_II },
            { Color.Purple, ColorCode.I_III },
            { Color.Green, ColorCode.II_III },
            { Color.Black, ColorCode.All },
        };
        private static readonly Dictionary<ColorCode, Color> _colorCodeToColor = new Dictionary<ColorCode, Color>()
        {
            { ColorCode.I, Color.Red },
            { ColorCode.II, Color.Yellow},
            { ColorCode.III, Color.Blue},
            { ColorCode.I_II, Color.Orange},
            { ColorCode.I_III, Color.Purple},
            { ColorCode.II_III, Color.Green},
            { ColorCode.All, Color.Black},
        };

        public Color I => Color.Red;
        public Color II => Color.Yellow;
        public Color III => Color.Blue;
        public Color I_II => Color.Orange;
        public Color I_III => Color.Purple;
        public Color II_III => Color.Green;
        public Color All => Color.Black;

        public Color CodeToColor(ColorCode cc) => _colorCodeToColor[cc];
        public ColorCode ColorToCode(Color c) => _colorToColorCode[c];

        public Color Combine(Color c1, Color c2) => throw new NotImplementedException();
    }
}
