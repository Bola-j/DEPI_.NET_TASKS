namespace SESSION_11
{
    internal class Rectangle : Shape
    {
        public double Width  { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height, string color = "White") : base("Rectangle", color)
        {
            Width  = width;
            Height = height;
        }

        public override double GetArea()      => Width * Height;
        public override double GetPerimeter() => 2 * (Width + Height);
    }
}
