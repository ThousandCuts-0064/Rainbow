using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public abstract class GameObject : IDisposable, IDrawable
    {
        public Layer Layer { get; }

        protected GameObject(Layer layer)
        {
            Layer = layer;
            Game.AddToDrawList(this);
        }

        public abstract void Draw(Graphics graphics);
        public virtual void Dispose() =>
            Game.RemoveFromDrawList(this);
    }
}
