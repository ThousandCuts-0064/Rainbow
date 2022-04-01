﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class CMY : IColorModel
    {
        private static readonly IReadOnlyMap<ColorCode, Color> _mapColor = new Map<ColorCode, Color>()
        {
            { ColorCode.I, Color.Cyan },
            { ColorCode.II, Color.Magenta },
            { ColorCode.III, Color.Yellow },

            { ColorCode.I_II, Color.Blue },
            { ColorCode.I_III, Color.Lime }, // That's Green
            { ColorCode.II_III, Color.Red },

            { ColorCode.All, Color.Black },
        };

        public Color CodeToColor(ColorCode cc) => _mapColor.Forward[cc];
        public ColorCode ColorToCode(Color c) => _mapColor.Reverse[c];

        public Color Combine(Color c1, Color c2) => 
            throw new NotImplementedException();

        public override string ToString() => nameof(CMY);
    }
}
