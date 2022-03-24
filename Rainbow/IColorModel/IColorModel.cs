using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    /// <summary>
    /// 3 primary colors of a model are named as rome numbers. Underscore is addition
    /// </summary>
    public interface IColorModel
    {
        Color I { get; }
        Color II { get; }
        Color III { get; }
        Color I_II { get; }
        Color I_III { get; }
        Color II_III { get; }
        Color All { get; }

        Color CodeToColor(ColorCode cc);
        ColorCode ColorToCode(Color c);
        Color Combine(Color c1, Color c2);
    }
}
