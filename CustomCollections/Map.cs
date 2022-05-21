using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCollections
{
    public class Map<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        private readonly Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private readonly Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        public ReadOnlyDictionary<T1, T2> Forward { get; }
        public ReadOnlyDictionary<T2, T1> Reverse { get; }

        public Map()
        {
            Forward = new ReadOnlyDictionary<T1, T2>(_forward);
            Reverse = new ReadOnlyDictionary<T2, T1>(_reverse);
        }

        public ReadOnlyMap<T1, T2> ToReadOnly() => new ReadOnlyMap<T1, T2>(this);

        public void Add(T1 t1, T2 t2)
        {
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
        }

        public void RemoveForward(T1 t1)
        {
            _reverse.Remove(_forward[t1]);
            _forward.Remove(t1);
        }

        public void RemoveReverse(T2 t2)
        {
            _forward.Remove(_reverse[t2]);
            _reverse.Remove(t2);
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => _forward.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
