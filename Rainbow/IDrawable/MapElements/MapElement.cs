using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rainbow
{
    public abstract class MapElement : IDrawable
    {
        public MapElement() => Game.MapElements.Add(this);

        public abstract void Draw(Graphics graphics);
    }
}
