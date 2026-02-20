using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SESSION_08
{
    internal class ThreeDPoint : IComparable<ThreeDPoint>, ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public ThreeDPoint(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public ThreeDPoint(int x, int y) : this(x, y, 0) { }
        public ThreeDPoint(int x) : this(x, 0, 0) { }
        public ThreeDPoint() : this(0, 0, 0) { }

        public override string ToString()
        {
            return $"Point Coordinates: ({X}, {Y}, {Z})";
        }
        //public override bool Equals(object obj)
        //{
        //    if (obj is ThreeDPoint P)
        //    {
        //        return this.X == P.X && this.Y == P.Y && this.Z == P.Z;
        //    }
        //    return false;
        //}

        public int CompareTo(ThreeDPoint? other)
        {
            if (other == null) return 1;
            int xCompare = this.X.CompareTo(other.X);
            if (xCompare != 0) return xCompare;
            return this.Y.CompareTo(other.Y);
        }

        public object Clone()
        {
            return new ThreeDPoint(X, Y, Z);
        }
        // because the method Clone is creating a new object with the same values of the properties
        // but with a different reference in the memory.

        //public object Clone()
        //{
        //    return this;
        //}
        // this implementation of the Clone method will return the same reference of the object in the memory,
        
    }
}

