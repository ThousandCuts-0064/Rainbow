using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    [Flags]
    public enum GameModifiers // Note: ShortString version must be added to new modifiers (Utility.cs)
    {
        None = 0,

        NoClickTiles =      1 << 0,
        DoubleTiles =       1 << 1,
        TripleTiles =       1 << 2,
        DoubleClickTiles =  1 << 3,
        TripleClickTiles =  1 << 4,
                            
        NoColorTiles =      1 << 5,
        ColorDistortion =   1 << 6,
        InvertedColors =    1 << 7,
        FadingColors =      1 << 8,
        FlashingColors =    1 << 9,
                            
        ColorWheel =        1 << 10,
        ColorfulBack =      1 << 11,
        HintColumns =       1 << 12,
        HintButtons =       1 << 13,
        Birdy =             1 << 14,
                            
        MessageEvent =      1 << 15,
        ShotgunEvent =      1 << 16,
        DiamondEvent =      1 << 17,
        ChessEvent =        1 << 18,
        RainbowEvent =      1 << 19,
    }
}
