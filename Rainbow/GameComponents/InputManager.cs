using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rainbow
{
    class InputManager
    {
        private const int CHAIN_TICKS_WINDOW = 3;
        private readonly HashSet<Keys> _pressedKeys = new HashSet<Keys>();
        private readonly Column[] _columns;
        public static IReadOnlyMap<Keys, (ColorCode colorCode, int column)> MapKeys { get; } = new Map<Keys, (ColorCode colorCode, int column)>()
        {
            { Keys.Q, (ColorCode.I, 0) }, { Keys.A, (ColorCode.II, 0) }, { Keys.Z, (ColorCode.III, 0) },
            { Keys.W, (ColorCode.I, 1) }, { Keys.S, (ColorCode.II, 1) }, { Keys.X, (ColorCode.III, 1) },
            { Keys.E, (ColorCode.I, 2) }, { Keys.D, (ColorCode.II, 2) }, { Keys.C, (ColorCode.III, 2) },
            { Keys.R, (ColorCode.I, 3) }, { Keys.F, (ColorCode.II, 3) }, { Keys.V, (ColorCode.III, 3) },
            { Keys.T, (ColorCode.I, 4) }, { Keys.G, (ColorCode.II, 4) }, { Keys.B, (ColorCode.III, 4) },
            { Keys.Y, (ColorCode.I, 5) }, { Keys.H, (ColorCode.II, 5) }, { Keys.N, (ColorCode.III, 5) },
            { Keys.U, (ColorCode.I, 6) }, { Keys.J, (ColorCode.II, 6) }, { Keys.M, (ColorCode.III, 6) },
            { Keys.I, (ColorCode.I, 7) }, { Keys.K, (ColorCode.II, 7) }, { Keys.Oemcomma, (ColorCode.III, 7) },
            { Keys.O, (ColorCode.I, 8) }, { Keys.L, (ColorCode.II, 8) }, { Keys.OemPeriod, (ColorCode.III, 8) },
            { Keys.P, (ColorCode.I, 9) }, { Keys.Oem1, (ColorCode.II, 9) }, { Keys.OemQuestion,(ColorCode.III, 9) }, // Oem1 = ';'   OemQuestion = '/'
        };
        public event Action<ColorCode, int> ColorInput;
        public event Action Shotgun;

        public InputManager(int level)
        {
            _columns = new Column[level];
            for (int i = 0; i < level; i++)
                _columns[i] = new Column(this, i);
        }

        public void OnTick()
        {
            for (int i = 0; i < _columns.Length; i++)
                _columns[i].OnTick();
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            if (MapKeys.Forward.TryGetValue(key, out var tuple) &&
                tuple.column < _columns.Length &&
                _pressedKeys.Add(key)) // Send input once per key press
                _columns[tuple.column].OnKeyDown(key);
        }

        public void OnKeyUp(object sender, KeyEventArgs e) => _pressedKeys.Remove(e.KeyCode);

        private class Column
        {
            private readonly InputManager _inputManager;
            private readonly int _column;
            private ColorCode _colorCodeInput = ColorCode.None;
            private ulong _tickChainStarter;

            public Column(InputManager inputManager, int column)
            {
                _inputManager = inputManager;
                _column = column;
            }

            public void OnTick()
            {
                if (_colorCodeInput != ColorCode.None &&
                    Game.Ticks == _tickChainStarter + CHAIN_TICKS_WINDOW)
                    _inputManager.ColorInput?.Invoke(_colorCodeInput, _column);

                if (Game.Ticks > _tickChainStarter + CHAIN_TICKS_WINDOW)
                    _colorCodeInput = ColorCode.None;
            }

            public void OnKeyDown(Keys key)
            {
                if (key == Keys.Space)
                {
                    _inputManager.ColorInput?.Invoke(_colorCodeInput, _column);
                    _colorCodeInput = ColorCode.None;
                    _inputManager.Shotgun?.Invoke();
                    return;
                }

                if (_colorCodeInput == ColorCode.None) _tickChainStarter = Game.Ticks;
                _colorCodeInput |= MapKeys.Forward[key].colorCode;
            }
        }
    }
}
