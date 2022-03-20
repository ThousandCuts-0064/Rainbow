using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public abstract class UIElement : IDrawable
    {
        public UIElement() => Game.UIElements.Add(this);

        public virtual void Dispose() => Game.UIElements.Remove(this);

        public abstract void Draw(Graphics graphics);
    }
}
