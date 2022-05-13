using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    /// <summary>
    /// 3 primary colors of a model are named as single roman number. Underscore is addition.
    /// </summary>
    [Flags]
    public enum ColorCode
    {
        None = 0,
        I = 1,
        II = 2,
        I_II = I | II, // 3
        III = 4,
        I_III = I | III, // 5
        II_III = II | III, // 6
        All = I | II | III, // 7
    }
}
