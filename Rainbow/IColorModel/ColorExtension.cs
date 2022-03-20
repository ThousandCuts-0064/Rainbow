using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public static class ColorExtension
    {
        public static ColorComplexity ToComplexity(this ColorCode code)
        {
            if (code < ColorCode.I || code > ColorCode.All) return ColorComplexity.Invalid;
            //1 bit is set
            if ((code & (code - 1)) != 0) return ColorComplexity.Primary;
            if (code == ColorCode.All) return ColorComplexity.All;
            return ColorComplexity.Secondary;
        }
    }
}
