namespace SESSION_01
{
    public class Dog
    {
        private string _name;
        private string _skinColor;
        private int _age;

        public Dog()
        {
            _name = "Unknown";
            _skinColor = "Unknown";
            _age = 0;
        }

        public Dog(string name, string skinColor, int age)
        {
            _name = name;
            _skinColor = skinColor;
            _age = age;
        }

        public string getName() => _name;
        public void setName(string name) => _name = name;

        public string getSkinColor() => _skinColor;
        public void setSkinColor(string skinColor) => _skinColor = skinColor;

        public int getAge() => _age;
        public void setAge(int age) => _age = age;

        public void bark()
        {
            Console.WriteLine("Woof!");
        }

        public void Mumming(bool hasTreats)
        {
            if (hasTreats)
                Console.WriteLine("Dog is happy and mumming!");
            else
                Console.WriteLine("Dog is not interested.");
        }
    }
}
