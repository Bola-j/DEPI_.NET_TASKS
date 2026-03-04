namespace SESSION_11
{
    internal abstract class Shape
    {
        public string Name  { get; }
        public string Color { get; set; }

        protected Shape(string name, string color)
        {
            Name  = name;
            Color = color;
        }

        // delegate double MeasureOp(Shape s)
        public delegate double MeasureOp(Shape s);
        public abstract double GetArea();
        public abstract double GetPerimeter();

        // delegate void ShapePrinter(Shape s)
        public delegate void ShapePrinter(Shape s);
        public void PrintInfo()
        {
            Console.WriteLine($"[{Name}] Color: {Color} | Area: {GetArea():F2} | Perimeter: {GetPerimeter():F2}");
        }

        // delegate bool ShapeComparator(Shape a, Shape b)
        public delegate bool ShapeComparator(Shape a, Shape b);
        public bool IsLargerThan(Shape other) => GetArea() > other.GetArea();
    }
}
