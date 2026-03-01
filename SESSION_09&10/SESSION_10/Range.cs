using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SESSION_10
{
    internal class Range<T> where T : INumber<T> // INumber<T> is better than IComparable<T> for numerics
    {
        private T _high;
        private T _low;
        public T High
        {
            get { return _high; }
            set
            {
                if (value >= _low)
                    _high = value;
                else
                {
                    Console.WriteLine("High value cannot be less than Low value. High remains unchanged.");
                }
            }
        }

        public T Low
        {
            get { return _low; }
            set
            {
                if (value <= _high)
                    _low = value;
                else
                {
                    Console.WriteLine("Low value cannot be greater than High value. Low remains unchanged.");
                }
            }
        }

        public Range(T low, T high)
        {
            if (low > high)
                throw new ArgumentException("Low value cannot be greater than High value.");

            _high = high;
            _low = low;
        }

        public bool IsInRange(T value)
        {
            return value >= _low && value <= _high;
        }

        public T Length()
        {
            return _high - _low;
        }
    }
}
