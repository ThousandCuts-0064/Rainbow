using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rainbow
{
    public abstract class DynamicObject : GameObject
    {
        private readonly Update _update;
        public PointF Location { get; protected set; }

        public DynamicObject(Layer layer) : base(layer)
        {
            _update = Update;
            Game.AddToUpdateCallback(_update);
        }

        public abstract PointF GetCenter();

        public override void Dispose()
        {
            base.Dispose();
            Game.RemoveFromUpdateCallback(_update);
        }

        protected abstract void Update();
    }
}
