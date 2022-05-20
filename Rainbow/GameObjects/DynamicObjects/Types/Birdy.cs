using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public class Birdy : DynamicObject
    {
        private static float Speed => Game.TileUnitsPerSecond * Game.Unit * Game.DELTA_TIME * 2f;
        private readonly BirdyManager _birdyManager;
        private readonly IdleState _idleState;
        private readonly ChaseState _chaseState;
        private readonly LeavingState _leavingState;
        private readonly Animation _animation;
        private readonly Line _lineToTarget;
        private Tile.Controller _targetController;
        private Tile _target;
        /// <summary>
        /// Should only be set using the SetState method.
        /// </summary>
        private State _state;
        public static float Width => Game.TileHeight;
        public static float Height => Game.TileHeight;
        public PointF Location
        {
            get => _animation.Rectangle.Location;
            set
            {
                _animation.IsFlipped = _animation.Rectangle.X > value.X;
                _animation.Rectangle = new RectangleF(value, _animation.Rectangle.Size);
            }
        }

        public event Action<Tile> TargetFound;
        public event Action<Tile> TargetTaken;

        public Birdy(BirdyManager birdyManager, PointF location, Tile target = null, Layer layer = Layer.UI) : base(layer)
        {
            var rectangle = new RectangleF(location, new SizeF(Width, Height));
            _birdyManager = birdyManager;
            _animation = new Animation(Resources.BirdyAnimation, null, rectangle, Layer);
            _lineToTarget = new Line(Color.Black, GetCenter(), GetCenter(), layer, Game.Unit);
            _lineToTarget.Pen.EndCap = LineCap.ArrowAnchor;
            _lineToTarget.Pen.DashStyle = DashStyle.Dash;
            _idleState = new IdleState(this);
            _chaseState = new ChaseState(this);
            _leavingState = new LeavingState(this);
            if (target == null) SetState(_idleState);
            else
            {
                _target = target;
                TargetFound?.Invoke(target);
                SetState(_chaseState);
            }
        }

        public override PointF GetCenter() => _animation.Rectangle.GetCenter();

        public override void Draw(Graphics graphics)
        {
            if (_state != _chaseState) return;

            _lineToTarget.Point1 = GetCenter();
            _lineToTarget.Point2 = _target.GetCenter();
        }

        protected override void Update() => _state.OnUpdate();

        public void Retarget() => SetState(_idleState);

        public override void Dispose()
        {
            base.Dispose();
            _animation.Dispose();
            _lineToTarget.Dispose();
            _target.Dispose();
        }

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

            public override void OnSet() 
            {
                Birdy._lineToTarget.Point2 = Birdy._lineToTarget.Point1;
            }

            public override void OnUpdate()
            {
                if (Birdy._birdyManager.RequestTarget(out Birdy._target))
                {
                    Birdy.TargetFound?.Invoke(Birdy._target);
                    Birdy.SetState(Birdy._chaseState);
                }
            }
        }

        private class ChaseState : State
        {
            public ChaseState(Birdy birdy) : base(birdy) { }

            public override void OnSet() { }

            public override void OnUpdate()
            {
                var targetCenter = Birdy._target.GetCenter();
                var center = Birdy.GetCenter();
                if (center == targetCenter) TryTakeTarget();

                var direction = new PointF(
                    targetCenter.X - center.X,
                    targetCenter.Y - center.Y);
                var magnitude = (float)Math.Sqrt(
                    direction.X * direction.X +
                    direction.Y * direction.Y);
                var normalized = new PointF(
                    direction.X / magnitude,
                    direction.Y / magnitude);
                var step = Math.Min(Speed, magnitude);
                var scaled = new PointF(
                    normalized.X * step,
                    normalized.Y * step);
                Birdy.Location = Birdy.Location.OffsetNew(scaled.X, scaled.Y);

                if (step * 2 >= magnitude) TryTakeTarget();

                void TryTakeTarget()
                {
                    if (Birdy._target.TryGetController(out var controller))
                    {
                        Birdy.TargetTaken?.Invoke(Birdy._target);
                        Birdy._targetController = controller;
                        Birdy.SetState(Birdy._leavingState);
                    }
                    else Birdy.SetState(Birdy._idleState);
                }
            }
        }

        private class LeavingState : State
        {
            private Func<bool> _reachedFinish;
            private int _directionLeave; // -1 or 1
            private float _finishX;

            public LeavingState(Birdy birdy) : base(birdy) { }

            public override void OnSet()
            {
                _directionLeave = Birdy._target.Column <= Game.Level / 2
                    ? -1
                    : Game.Level % 2 == 1 && Birdy._target.Column == Game.Level / 2 + 1
                    ? Game.Random.Next(2) * 2 - 1 // this will be -1 or 1
                    : 1;
                if (_directionLeave == -1)
                {
                    _finishX = Game.Screen.Left - Game.TileWidth;
                    _reachedFinish = () => Birdy.Location.X <= _finishX;
                }
                else
                {
                    _finishX = Game.Screen.Right + Game.TileWidth;
                    _reachedFinish = () => Birdy.Location.X >= _finishX;
                }
                Birdy._lineToTarget.Dispose();
            }

            public override void OnUpdate()
            {
                Move(Speed * _directionLeave, 0);
                if (_reachedFinish()) Birdy.Dispose();
            }

            private void Move(float x, float y)
            {
                Birdy.Location = Birdy.Location.OffsetNew(x, y);
                Birdy._targetController.Move(x, y);
            }
        }
    }
}
