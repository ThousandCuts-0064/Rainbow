﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rainbow
{
    public interface IDrawable
    {
        Layer Layer { get; }

        void Draw(Graphics graphics);
    }
}
