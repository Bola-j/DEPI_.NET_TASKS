namespace SESSION_08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Project 1

            Console.WriteLine("Enter Coordinates for Point1 (X,Y,Z)");
            Console.Write("Enter X: ");
            int x1 = int.TryParse(Console.ReadLine(), out x1) ? x1 : 0;
            Console.Write("Enter Y: ");
            int y1 = int.TryParse(Console.ReadLine(), out y1) ? y1 : 0;
            Console.Write("Enter Z: ");
            int z1 = int.TryParse(Console.ReadLine(), out z1) ? z1 : 0;
            ThreeDPoint point1 = new ThreeDPoint(x1, y1, z1);

            Console.WriteLine("Enter Coordinates for Point2 (X,Y,Z)");
            Console.Write("Enter X: ");
            x1 = int.TryParse(Console.ReadLine(), out x1) ? x1 : 0;
            Console.Write("Enter Y: ");
            y1 = int.TryParse(Console.ReadLine(), out y1) ? y1 : 0;
            Console.Write("Enter Z: ");
            z1 = int.TryParse(Console.ReadLine(), out z1) ? z1 : 0;
            ThreeDPoint point2 = new ThreeDPoint(x1, y1, z1);

            Console.WriteLine(point1.ToString());
            Console.WriteLine(point2.ToString());
            
            Console.WriteLine();

            Console.WriteLine($"Is P1 == P2?? \nAnswer: {point1==point2}");
            
            Console.WriteLine();
            Console.WriteLine($"Is P1 == P2?? \nAnswer: {point1.Equals(point2)}");
            //both returns false because we are comparing the reference of the objects not the values of the properties.
            // So if we need to compare the values of the properties we need to override the Equals method in the ThreeDPoint class and compare the values of the properties.
            Console.WriteLine();

            ThreeDPoint[] arr = new ThreeDPoint[] { point1, point2, new ThreeDPoint(3) , new ThreeDPoint(5,4) , new ThreeDPoint()};
            Array.Sort(arr);
            foreach (var item in arr)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();

            ThreeDPoint cloned = (ThreeDPoint)point1.Clone();
            Console.WriteLine();
            Console.WriteLine($"Cloned -> {cloned.ToString()}");
            Console.WriteLine($"Are they of the same reference? -> {cloned.GetHashCode() == point1.GetHashCode()}");
            // because the method Clone is creating a new object with the same values of the properties
            // but with a different reference in the memory.
            

            #endregion


            Console.WriteLine();
            
            #region Project 2

            Console.WriteLine($"5 + 3 = {Maths.Add(5, 3)}");
            Console.WriteLine($"5 - 3 = {Maths.Subtract(5, 3)}");
            Console.WriteLine($"5 * 3 = {Maths.Multiply(5, 3)}");
            Console.WriteLine($"5 / 3 = {Maths.Divide(5, 3)}");
            #endregion

            Console.WriteLine();
            Console.WriteLine();

            #region Project 3
            Duration D1 = new Duration(1, 10, 15);
            Console.WriteLine(D1.ToString());
            Console.WriteLine();

            Duration D2 = new Duration(3600);
            Console.WriteLine(D2.ToString());
            Console.WriteLine();

            Duration D3 = new Duration(7800);
            Console.WriteLine(D3.ToString());
            Console.WriteLine();

            Duration D4 = new Duration(666);
            Console.WriteLine(D4.ToString());

            Console.WriteLine();
            Console.WriteLine(D4.GetHashCode());

            Console.WriteLine();

            Console.WriteLine($" Are they equal in terms of values?? \n {D4.Equals(new Duration(D4.GetHashCode()))}");
            #endregion
        }
    }
}
