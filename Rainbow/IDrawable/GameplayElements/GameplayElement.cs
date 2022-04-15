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
        public PointF Location { get; protected set; }

        public GameplayElement()
        {
            _update = Update;
            Game.GameplayElements.Add(this);
            Game.AddToUpdateCallback(_update);
        }

        public abstract PointF GetCenter();
        public abstract void Draw(Graphics graphics);

        public virtual void Dispose()
        {
            Game.GameplayElements.Remove(this);
            Game.RemoveFromUpdateCallback(_update);
        }

        protected abstract void Update();
    }
}
