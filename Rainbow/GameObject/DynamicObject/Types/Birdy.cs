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
        private static float Speed => Game.TileUnitsPerSecond * Game.Unit * Game.DELTA_TIME * 1.2f;
        private readonly BirdyManager _birdyManager;
        private readonly IdleState _idleState;
        private readonly ChaseState _chaseState;
        private readonly LeavingState _leavingState;
        private readonly GameImage _gameImage;
        private readonly Line _lineToTarget;
        private Tile.Controller _targetController;
        private Tile _target;
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

        public Birdy(BirdyManager birdyManager, PointF location, Layer layer = Layer.UI) : base(layer)
        {
            var rectangle = new RectangleF(location, new SizeF(Width, Height));
            _birdyManager = birdyManager;
            _gameImage = new GameImage(Resources.Birdy, rectangle, Layer);
            _lineToTarget = new Line(Color.Black, new PointF(), new PointF(), layer, Game.Unit);
            _lineToTarget.Pen.EndCap = LineCap.ArrowAnchor;
            _lineToTarget.Pen.DashStyle = DashStyle.Dash;
            _idleState = new IdleState(this);
            _chaseState = new ChaseState(this);
            _leavingState = new LeavingState(this);
            SetState(_idleState);
        }

        public override PointF GetCenter() => _gameImage.Rectangle.GetCenter();

        public override void Draw(Graphics graphics) 
        {
            if (_state != _chaseState) return;

            _lineToTarget.Point1 = GetCenter();
            _lineToTarget.Point2 = _target.GetCenter();
        }

        protected override void Update() => _state.OnUpdate();

        public override void Dispose()
        {
            base.Dispose();
            _gameImage.Dispose();
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

            public override void OnSet() { }

            public override void OnUpdate() 
            {
                if (Birdy._birdyManager.TryGetClosestToFinish(out Birdy._target))
                {
                    Birdy._target.OnDispose += () => Birdy.SetState(Birdy._idleState);
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

                if (magnitude < step * 2)
                {
                    if (Birdy._target.TryGetController(out var controller))
                    {
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
                Move(Speed * (int)_directionLeave, 0);
                if (_reachedFinish()) Birdy.Dispose();
            }

            private void Move(float x, float y)
            {
                Birdy.Location = Birdy.Location.Offset(x, y);
                Birdy._targetController.Move(x, y);
            }

            private enum Direction
            {
                Left = -1,
                Right = 1
            }
        }
    }
}
