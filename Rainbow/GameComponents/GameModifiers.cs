using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    [Flags]
    public enum GameModifiers
    {
        None = 0,
        ColorDistortion = 1,
        DoubleTiles = 2,
        TripleTiles = 4,
        FadingColors = 8,
        InvertedColors = 16,
        HintButtons = 32,
        ColorWheel = 64,
        ShotgunTiles = 128,
    }
}
