using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SESSION_02
{
    internal class Duck
    {
        public string Name { get; set; }
        public string Color { get; set; }

        public Duck(string name, string color)
        {
            Name = name;
            Color = color;
        }

        public void Display()
        {
            Console.WriteLine($"Duck Name: {Name}, Color: {Color}");
        }
    }
}
