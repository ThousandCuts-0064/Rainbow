using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public static class Math2
    {
        public static float Clamp(float value, float min, float max) =>
            value < min ? min : value > max ? max : value;

        public static float Lerp(byte min, byte max, float by) =>
            min * (1 - by) + max * by;
    }
}
