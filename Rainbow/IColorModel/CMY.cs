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
        private static readonly Dictionary<Color, ColorCode> _colorToColorCode = new Dictionary<Color, ColorCode>()
        {
            { Color.Cyan, ColorCode.I },
            { Color.Magenta, ColorCode.II },
            { Color.Yellow, ColorCode.III },
            { Color.Blue, ColorCode.I_II },
            { Color.Green, ColorCode.I_III },
            { Color.Red, ColorCode.II_III },
            { Color.Black, ColorCode.All },
        };
        private static readonly Dictionary<ColorCode, Color> _colorCodeToColor = new Dictionary<ColorCode, Color>()
        {
            { ColorCode.I, Color.Cyan },
            { ColorCode.II, Color.Magenta},
            { ColorCode.III, Color.Yellow},
            { ColorCode.I_II, Color.Blue},
            { ColorCode.I_III, Color.Green},
            { ColorCode.II_III, Color.Red},
            { ColorCode.All, Color.Black},
        };

        public Color I => Color.Cyan;
        public Color II => Color.Magenta;
        public Color III => Color.Yellow;
        public Color I_II => Color.Blue;
        public Color I_III => Color.Green;
        public Color II_III => Color.Red;
        public Color All => Color.Black;

        public Color CodeToColor(ColorCode cc) => _colorCodeToColor[cc];
        public ColorCode ColorToCode(Color c) => _colorToColorCode[c];

        public Color Combine(Color c1, Color c2) => throw new NotImplementedException();
    }
}
