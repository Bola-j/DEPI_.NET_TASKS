using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SESSION_10
{
    internal class FixedSizeList<T> : List<T>
    {
        public readonly int _maxSize;
        public int MaxSize => _maxSize; // Read-only property to expose max size

        public FixedSizeList(int maxSize)
        {
            if (maxSize <= 0)
                throw new ArgumentException("Max size must be greater than zero.");
            _maxSize = maxSize;
        }
        public new void Add(T item) // 'new' keyword is used to hide the base class method
        {
            if (Count >= _maxSize) // Count is a property of List<T>
                throw new InvalidOperationException($"Cannot add more than {_maxSize} items to this list.");
            base.Add(item);
        }

        public T Get(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            return this[index];
        }
    }
}
