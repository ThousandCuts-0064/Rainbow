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
        private const int keyChainTicksWindow = 3;
        private static readonly Dictionary<Keys, int> _keyToColumn = new Dictionary<Keys, int>()
        {
            { Keys.Q, 0 }, { Keys.A, 0 }, { Keys.Z, 0 },
            { Keys.W, 1 }, { Keys.S, 1 }, { Keys.X, 1 },
            { Keys.E, 2 }, { Keys.D, 2 }, { Keys.C, 2 },
            { Keys.R, 3 }, { Keys.F, 3 }, { Keys.V, 3 },
            { Keys.T, 4 }, { Keys.G, 4 }, { Keys.B, 4 },
            { Keys.Y, 5 }, { Keys.H, 5 }, { Keys.N, 5 },
            { Keys.U, 6 }, { Keys.J, 6 }, { Keys.M, 6 },
            { Keys.I, 7 }, { Keys.K, 7 }, { Keys.Oemcomma, 7 },
            { Keys.O, 8 }, { Keys.L, 8 }, { Keys.OemPeriod, 8 },
            { Keys.P, 9 }, { Keys.Oem1 /* ; */, 9 }, { Keys.OemQuestion /* / */, 9 },
        };
        private static readonly Dictionary<Keys, ColorCode> _keyToColor = new Dictionary<Keys, ColorCode>()
        {
            { Keys.Q, ColorCode.I }, { Keys.A, ColorCode.II }, { Keys.Z, ColorCode.III },
            { Keys.W, ColorCode.I }, { Keys.S, ColorCode.II }, { Keys.X, ColorCode.III },
            { Keys.E, ColorCode.I }, { Keys.D, ColorCode.II }, { Keys.C, ColorCode.III },
            { Keys.R, ColorCode.I }, { Keys.F, ColorCode.II }, { Keys.V, ColorCode.III },
            { Keys.T, ColorCode.I }, { Keys.G, ColorCode.II }, { Keys.B, ColorCode.III },
            { Keys.Y, ColorCode.I }, { Keys.H, ColorCode.II }, { Keys.N, ColorCode.III },
            { Keys.U, ColorCode.I }, { Keys.J, ColorCode.II }, { Keys.M, ColorCode.III },
            { Keys.I, ColorCode.I }, { Keys.K, ColorCode.II }, { Keys.Oemcomma, ColorCode.III },
            { Keys.O, ColorCode.I }, { Keys.L, ColorCode.II }, { Keys.OemPeriod, ColorCode.III },
            { Keys.P, ColorCode.I }, { Keys.Oem1 /* ; */, ColorCode.II }, { Keys.OemQuestion /* / */, ColorCode.III },
        };
        private readonly HashSet<Keys> _pressedKeys = new HashSet<Keys>();
        private readonly Column[] _columns;
        public event Action<ColorCode, int> ColorInput;

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
            if (_keyToColumn.TryGetValue(key, out int coulmnIndex) &&
                coulmnIndex < _columns.Length &&
                _pressedKeys.Add(key)) // Send input once per key press
                _columns[coulmnIndex].OnKeyDown(key);
        }

        public void OnKeyUp(object sender, KeyEventArgs e) => _pressedKeys.Remove(e.KeyCode);

        private class Column
        {
            private readonly InputManager _inputManager;
            private readonly int _column;
            private ColorCode _colorCodeInput = ColorCode.Invalid;
            private ulong _tickChainStarter;

            public Column(InputManager inputManager, int column)
            {
                _inputManager = inputManager;
                _column = column;
            }

            public void OnTick()
            {
                if (_colorCodeInput != ColorCode.Invalid &&
                    Game.Ticks == _tickChainStarter + keyChainTicksWindow)
                    _inputManager.ColorInput(_colorCodeInput, _column);

                if (Game.Ticks > _tickChainStarter + keyChainTicksWindow)
                    _colorCodeInput = ColorCode.Invalid;
            }

            public void OnKeyDown(Keys key)
            {
                if (_colorCodeInput == ColorCode.Invalid) _tickChainStarter = Game.Ticks;
                _colorCodeInput |= _keyToColor[key];
            }
        }
    }
}
