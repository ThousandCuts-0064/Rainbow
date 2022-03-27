using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public interface IReadOnlyMap<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        IReadOnlyDictionary<T1, T2> Forward { get; }
        IReadOnlyDictionary<T2, T1> Reverse { get; }
    }
}
