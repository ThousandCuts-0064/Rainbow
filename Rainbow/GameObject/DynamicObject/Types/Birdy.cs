using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    class Birdy : DynamicObject
    {
        private static float Speed => Game.TileUnitsPerSecond * Game.Unit * Game.DELTA_TIME * 2;
        private readonly IdleState _idleState;
        private readonly ChaseState _chaseState;
        private readonly LeavingState _leavingState;
        private readonly GameImage _gameImage;
        private readonly Tile _target;
        private readonly Line _lineToTarget;
        /// <summary>
        /// Should only be set using the SetState method.
        /// </summary>
        private State _state;
        public static float Width => Game.TileWidth;
        public static float Height => Game.TileHeight;
        public PointF Location
        {
            get => _gameImage.Rectangle.Location;
            set => _gameImage.Rectangle = new RectangleF(value, _gameImage.Rectangle.Size);
        }

        public Birdy(PointF location, Tile target, Layer layer = Layer.UI) : base(layer)
        {
            var rectangle = new RectangleF(location, new SizeF(Width, Height));
            _gameImage = new GameImage(Resources.Birdy, rectangle, Layer);
            _target = target;
            _lineToTarget = new Line(Color.Black, GetCenter(), _target.GetCenter(), layer, Game.Unit);
            _lineToTarget.Pen.EndCap = LineCap.ArrowAnchor;
            _lineToTarget.Pen.DashStyle = DashStyle.Dash;
            _idleState = new IdleState(this);
            _chaseState = new ChaseState(this);
            _leavingState = new LeavingState(this);
            SetState(_chaseState);
        }

        public override PointF GetCenter() => _gameImage.Rectangle.GetCenter();

        public override void Draw(Graphics graphics) { }

        public override void Dispose()
        {
            base.Dispose();
            _gameImage.Dispose();
            _lineToTarget.Dispose();
            _target.Dispose();
        }

        protected override void Update() => _state.OnUpdate();

        private void SetState(State state)
        {
            _state = state;
            state.OnSet();
        }

        private abstract class State
        {
            protected Birdy Birdy { get; }

            protected State(Birdy birdy) => Birdy = birdy;

            public abstract void OnSet();
            public abstract void OnUpdate();
        }

        private class IdleState : State
        {
            public IdleState(Birdy birdy) : base(birdy) { }

            public override void OnSet() { }
            public override void OnUpdate() { }
        }

        private class ChaseState : State
        {
            public ChaseState(Birdy birdy) : base(birdy) { }

            public override void OnSet() { }

            public override void OnUpdate()
            {
                var targetCenter = Birdy._target.GetCenter();
                var center = Birdy.GetCenter();
                var direction = new PointF(
                    targetCenter.X - center.X,
                    targetCenter.Y - center.Y);
                var magnitude = (float)Math.Sqrt(
                    direction.X * direction.X +
                    direction.Y + direction.Y);
                var normalized = new PointF(
                    direction.X / magnitude,
                    direction.Y / magnitude);
                var step = Math.Min(Speed, magnitude);
                var scaled = new PointF(
                    normalized.X * step,
                    normalized.Y * step);
                Birdy.Location = Birdy.Location.Offset(scaled.X, scaled.Y);

                if (step == magnitude) Birdy._state = Birdy._leavingState;
            }
        }

        private class LeavingState : State
        {
            private Func<bool> _reachedFinish;
            private Direction _directionLeave;
            private float _finishX;

            public LeavingState(Birdy birdy) : base(birdy) { }

            public override void OnSet()
            {
                _directionLeave = Birdy._target.Column <= Game.Level / 2
                ? Direction.Left
                : Game.Level % 2 == 1 && Birdy._target.Column == Game.Level / 2 + 1
                ? (Direction)(Game.Random.Next(2) * 2 - 1) // this will be 1 or -1
                : Direction.Right;
                if (_directionLeave == Direction.Left)
                {
                    _finishX = Game.Screen.Left - Game.TileWidth;
                    _reachedFinish = () => Birdy.Location.X <= _finishX;
                }
                else
                {
                    _finishX = Game.Screen.Right + Game.TileWidth;
                    _reachedFinish = () => Birdy.Location.X >= _finishX;
                }
            }

            public override void OnUpdate()
            {
                Birdy.Location = Birdy.Location.Offset(Speed * (int)_directionLeave, 0);
                if (_reachedFinish()) Birdy.Dispose();
            }

            private enum Direction
            {
                Left = -1,
                Right = 1
            }
        }
    }
}
