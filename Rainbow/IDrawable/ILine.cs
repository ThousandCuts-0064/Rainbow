using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public interface ILine : IDrawable
    {
        PointF First { get; }
        PointF Second { get; }
    }
}
