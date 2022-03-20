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
        public Color I => throw new NotImplementedException();
        public Color II => throw new NotImplementedException();
        public Color III => throw new NotImplementedException();
        public Color I_II => throw new NotImplementedException();
        public Color I_III => throw new NotImplementedException();
        public Color II_III => throw new NotImplementedException();
        public Color All => throw new NotImplementedException();

        public Color this[ColorCode cc] => throw new NotImplementedException();
        public ColorCode this[Color c] => throw new NotImplementedException();

        public Color Combine(Color c1, Color c2)
        {
            throw new NotImplementedException();
        }
    }
}
