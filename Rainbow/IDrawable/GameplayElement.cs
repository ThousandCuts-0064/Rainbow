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
            Manager.GameplayElements.Add(this);
            Manager.AddUpdateCallback(_update);
        }

        public void Dispose()
        {
            Manager.GameplayElements.Remove(this);
            Manager.RemoveUpdateCallback(_update);
        }

        public abstract void Draw(Graphics graphics);

        protected abstract void Update();
    }
}
