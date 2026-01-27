namespace SESSION_01
{
    public class dogOwner
    {
        private string _ownerName;
        private bool _givesTreats;
        private Dog _dog;

        public dogOwner()
        {
            _ownerName = "Unknown";
            _givesTreats = false;
            _dog = new Dog();
        }

        public dogOwner(string ownerName, bool givesTreats, Dog dog)
        {
            _ownerName = ownerName;
            _givesTreats = givesTreats;
            _dog = dog;
        }

        public string getOwnerName() => _ownerName;
        public void setOwnerName(string ownerName) => _ownerName = ownerName;

        public bool getGivesTreats() => _givesTreats;
        public void setGivesTreats(bool givesTreats) => _givesTreats = givesTreats;

        public Dog getDog() => _dog;
        public void setDog(Dog dog) => _dog = dog;

        public void getAttacked()
        {
            Console.WriteLine($"{_ownerName} is being attacked! {_dog.getName()} is protecting!");
            _dog.bark();
        }

        public void playWithDog()
        {
            Console.Write($"{_ownerName} is playing with {_dog.getName()}. ");
            _dog.Mumming(_givesTreats);
        }
    }
}
