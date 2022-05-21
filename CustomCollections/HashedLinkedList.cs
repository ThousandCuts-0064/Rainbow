using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CustomCollections
{
    public class HashedLinkedList<T> : ICollection<T>, IReadOnlyCollection<T>
    {
        private readonly LinkedList<T> _list = new LinkedList<T>();
        private readonly Dictionary<T, LinkedListNode<T>> _dictionary = new Dictionary<T, LinkedListNode<T>>();
        public LinkedListNode<T> First => _list.First;
        public LinkedListNode<T> Last => _list.Last;
        public int Count => _list.Count;
        bool ICollection<T>.IsReadOnly => false;

        void ICollection<T>.Add(T item) => AddLast(item);

        public void AddFirst(T item) => 
            _dictionary.Add(item, _list.AddFirst(item));

        public void AddBefore(T existing, T @new) => 
            _dictionary.Add(@new, _list.AddBefore(_dictionary[existing], @new));

        public void AddAfter(T existing, T @new) => 
            _dictionary.Add(@new, _list.AddAfter(_dictionary[existing], @new));

        public void AddLast(T item) => 
            _dictionary.Add(item, _list.AddLast(item));

        public bool Remove(T item)
        {
            if (!Contains(item)) return false;

            _list.Remove(_dictionary[item]);
            _dictionary.Remove(item);
            return true;
        }

        public void Clear()
        {
            _list.Clear();
            _dictionary.Clear();
        }

        public bool Contains(T item) => _dictionary.ContainsKey(item);
        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
