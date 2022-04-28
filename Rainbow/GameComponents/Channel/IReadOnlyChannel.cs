using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public interface IReadOnlyChannel
    {
        IReadOnlyLine BoarderRight { get; }
        IReadOnlyLine BoarderLeft { get; }
        IReadOnlyLine Finish { get; }
        PointF PointSpawn { get; }
        RectangleF Rectangle { get; }
        int Index { get; }
    }
}
