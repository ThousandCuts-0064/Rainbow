using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCollections
{
    public class ReadOnlyMap<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        private readonly Map<T1, T2> _map;
        public ReadOnlyDictionary<T1, T2> Forward => _map.Forward;
        public ReadOnlyDictionary<T2, T1> Reverse => _map.Reverse;

        public ReadOnlyMap(Map<T1, T2> map) => _map = map;

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => _map.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _map.GetEnumerator();
    }
}
