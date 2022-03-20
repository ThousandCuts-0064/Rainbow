using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rainbow
{
    public abstract class GameplayElement : IDrawable, IDisposable
    {
        private readonly Update _update;

        public GameplayElement()
        {
            _update = Update;
            Game.GameplayElements.Add(this);
            Game.AddToUpdateCallback(_update);
        }

        public virtual void Dispose()
        {
            Game.GameplayElements.Remove(this);
            Game.RemoveFromUpdateCallback(_update);
        }

        public abstract void Draw(Graphics graphics);

        protected abstract void Update();
    }
}
