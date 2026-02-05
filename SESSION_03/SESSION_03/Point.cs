using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SESSION_03
{
    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public Point() { }
        public static float EculideanDistance(Point p1, Point p2)
        {
            return (float)Math.Sqrt( (float)Math.Pow(p2.X - p1.X, 2) + (float)Math.Pow(p2.Y - p1.Y, 2) );
        }
        public static bool lieOnSingleStraightLine(Point p1, Point p2, Point p3)
        {
            float distancec12 = EculideanDistance(p1, p2);
            float distancec23 = EculideanDistance(p2, p3);
            float distancec13 = EculideanDistance(p1, p3);
            float[] distances = new float[] { distancec12, distancec23, distancec13 };
            Array.Sort(distances);
            // dayman fe float error ba3d el 6th decimal place, so we need to check the error and set a safe limit for it
            return Math.Abs(distances[0] + distances[1] - distances[2]) < 1e-6;
        }
    }
}
