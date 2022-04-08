using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    readonly struct ColorColumn
    {
        public ColorCode ColorCode { get; }
        public int Column { get; }

        public ColorColumn(ColorCode colorCode, int column)
        {
            ColorCode = colorCode;
            Column = column;
        }

        public static implicit operator ColorColumn((ColorCode code, int column) tuple) => 
            new ColorColumn(tuple.code, tuple.column);
    }
}
