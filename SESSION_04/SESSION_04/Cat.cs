using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SESSION_04
{
    internal class Cat
    {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Color { get; set; }
    
            public Cat(string name, int age, string color)
            {
                Name = name;
                Age = age;
                Color = color;
            }
    
            public void Meow()
            {
                Console.WriteLine($"{Name} says: Meow!");
            }
            public void displayInfo()
            {
                Console.WriteLine($"Name: {Name}, Age: {Age}, Color: {Color}");
            }
    }
}
