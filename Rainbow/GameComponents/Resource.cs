using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public class Resource
    {
        private float _max;
        private float _current;
        public event Action Empty;
        public float Max 
        { 
            get => _max;
            set
            {
                if (value <= 0) Empty?.Invoke();
                _max = value;
            }
        }
        public float Current 
        { 
            get => _current; 
            set
            {
                _current = value;
                if (_current > 0) return;
                _current = 0;
                Empty?.Invoke();
            }
        }

        public Resource(float max)
        {
            Max = max;
            Current = max;
        }

        public float GetPercent() => Current / Max;
    }
}
