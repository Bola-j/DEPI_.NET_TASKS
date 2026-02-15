using System.Drawing;

namespace SESSION_05
{
    internal enum WeekDays
    {
        Sunday = 1,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
        
    }
    internal enum Permissions
    {   None = 0,
        Read ,
        Write ,
        Delete ,
        Execute 
    }
    internal enum Seasons
    {
        spring = 1,
        summer,
        autumn,
        winter
    }
    internal struct Person
    {
        public string Name;
        public int Age;
    }

    internal enum Colors
    {
        Red,
        Green,
        Blue
    }
    struct Point
    {
        public int _x;
        public int _y;
        
        public double DistanceTo(Point other)
        {
            int dx = _x - other._x;
            int dy = _y - other._y;
            return Math.Sqrt((double)(dx * dx + dy * dy));
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Ques. 01

            foreach (WeekDays day in Enum.GetValues<WeekDays>())
            {       
                Console.WriteLine($"{ day} : {(int)day}");
            }
            #endregion


            Console.WriteLine();


            #region Ques. 02
            Person[] array = new Person[3];
            array[0] = new Person { Name = "Hany" , Age = 23 };
            array[1].Name = "Bola";
            array[1].Age = 21;
            array[2] = new();
            array[2].Name = "Soliman";
            array[2].Age = 55;

            foreach(var person in array)
            {
                Console.Write($"Name : {person.Name} , ");
                Console.WriteLine($"Age : {person.Age}");
            }
            #endregion


            Console.WriteLine();


            #region Ques. 03
            Dictionary<int, string> dict = new Dictionary<int, string>
            {
                { 1, "March to May" },
                { 2, "June to August" },
                { 3, "September to November" },
                { 4, "December to February" }
            };
            Console.WriteLine("Enter the SEASON name: ");
            string seasonName = Console.ReadLine().ToLower();
            int seasonValue = 0;
            switch (seasonName)
            {
                case "spring":
                    seasonValue = (int)Seasons.spring;
                    break;
                case "summer":
                    seasonValue = (int)Seasons.summer;
                    break;
                case "autumn":
                    seasonValue = (int)Seasons.autumn;
                    break;
                case "winter":
                    seasonValue = (int)Seasons.winter;
                    break;
                default:
                    Console.WriteLine("Invalid season name.");
                    break;
            }
            if (seasonValue != 0)
            {
                Console.WriteLine($"the month {seasonName.ToUpper()} lasts from {dict[seasonValue]}");
            }
            #endregion


            Console.WriteLine();

            #region Ques. 04
            Permissions user = Permissions.Read | Permissions.Delete;
            user |= Permissions.Write;

            user |= Permissions.Read;

            bool canDelete = user.HasFlag(Permissions.Delete);
            bool canWrite = user.HasFlag(Permissions.Write);
            bool canRead = user.HasFlag(Permissions.Read);
            bool canExcute = user.HasFlag(Permissions.Execute);

            Console.WriteLine($" User can {(canRead ? "Read , " : "")}{(canWrite ? "Write , " : "")}{(canDelete ? "Delete , " : "")}{(canExcute ? "Execute " : "")}");

            #endregion

            Console.WriteLine();

            #region Ques. 05
            Console.Write("Enter a color name: ");
            string colorName = Console.ReadLine().ToLower();
            if(Enum.TryParse<Colors>(colorName, true, out Colors color))
            {
                Console.WriteLine($"The color {colorName} is {(color == Colors.Red || color == Colors.Green || color == Colors.Blue ? "primary" : "not primary")}");
            }
            else
            {
                Console.WriteLine($"Some Error happened");
            }
            #endregion


            Console.WriteLine();

            #region Ques. 06
            Console.WriteLine("Enter the coordinates of the first point : ");
            Point P1 = new(), P2 = new();
            Console.Write("x : ");
            P1._x = int.Parse(Console.ReadLine());
            Console.Write("y : ");
            P1._y = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the coordinates of the second point : ");
            Console.Write("x : ");
            P2._x = int.Parse(Console.ReadLine());
            Console.Write("y : ");
            P2._y = int.Parse(Console.ReadLine());

            double distance = P1.DistanceTo(P2);
            Console.WriteLine($"The distance between the two points is: {distance}");
            #endregion

            Console.WriteLine();

            #region Ques. 07
            Person[] people = new Person[3];
            int maxAge = int.MinValue, idx = 0;
            Console.WriteLine("Enter details for 3 people: ");
            for (int i = 0; i < people.Length; i++)
            {
                Console.Write($"Person {i + 1} Name: ");
                string name = Console.ReadLine();
                Console.Write($"Person {i + 1} Age: ");
                int age = int.Parse(Console.ReadLine());
                people[i] = new Person { Name = name, Age = age };
                if (age > maxAge)
                {
                    maxAge = age;
                    idx = i;

                }
            }
            
            Console.WriteLine($"The oldest person is {people[idx].Name} with {people[idx].Age} years old.");
            #endregion

        }
    }
}
