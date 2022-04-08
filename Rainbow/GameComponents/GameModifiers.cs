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
        DoubleClickTiles =  1 << 0,
        DoubleTiles =       1 << 1,
        TripleTiles =       1 << 2,
        Empty1 =            1 << 3,
        Empty2 =            1 << 4,
        ColorDistortion =   1 << 5,
        InvertedColors =    1 << 6,
        FadingColors =      1 << 7,
        Empty3 =            1 << 8,
        Empty4 =            1 << 9,
        ColorWheel =        1 << 10,
        HintButtons =       1 << 11,
        Birdy =             1 << 12,
        Empty5 =            1 << 13,
        Empty6 =            1 << 14,
        ShotgunTiles =      1 << 15,
        DiamondEvent =      1 << 16,
        ChessEvent =        1 << 17,
        Empty8 =            1 << 18,
        Empty9 =            1 << 19,
    }
}
