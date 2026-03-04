namespace SESSION_11
{
    internal class Circle : Shape
    {
        public double Radius { get; set; }

        public Circle(double radius, string color = "White") : base("Circle", color)
        {
            Radius = radius;
        }

        public override double GetArea()      => Math.PI * Radius * Radius;
        public override double GetPerimeter() => 2 * Math.PI * Radius;
    }
}
