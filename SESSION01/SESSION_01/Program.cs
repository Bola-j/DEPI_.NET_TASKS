namespace SESSION_01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Testing Dog Class ===\n");

            // Test default constructor
            Dog dog1 = new Dog();
            Console.WriteLine($"Dog1 (default): {dog1.getName()}, {dog1.getSkinColor()}, Age: {dog1.getAge()}");

            // Test parameterized constructor
            Dog dog2 = new Dog("Max", "Brown", 3);
            Console.WriteLine($"\nDog2 (parameterized): {dog2.getName()}, {dog2.getSkinColor()}, Age: {dog2.getAge()}");

            // Test setters
            dog1.setName("Buddy");
            dog1.setSkinColor("Golden");
            dog1.setAge(5);
            Console.WriteLine($"\nDog1 after setters: {dog1.getName()}, {dog1.getSkinColor()}, Age: {dog1.getAge()}");

            // Test bark method
            Console.Write("\nDog1 barking: ");
            dog1.bark();

            // Test Mumming method with treats
            Console.Write("Dog2 with treats: ");
            dog2.Mumming(true);

            // Test Mumming method without treats
            Console.Write("Dog2 without treats: ");
            dog2.Mumming(false);

            Console.WriteLine("\n\n=== Testing dogOwner Class ===\n");

            // Test default constructor
            dogOwner owner1 = new dogOwner();
            Console.WriteLine($"Owner1 (default): {owner1.getOwnerName()}, Gives treats: {owner1.getGivesTreats()}");
            Console.WriteLine($"Owner1's dog: {owner1.getDog().getName()}");

            // Test parameterized constructor
            dogOwner owner2 = new dogOwner("John", true, dog2);
            Console.WriteLine($"\nOwner2 (parameterized): {owner2.getOwnerName()}, Gives treats: {owner2.getGivesTreats()}");
            Console.WriteLine($"Owner2's dog: {owner2.getDog().getName()}");

            // Test setters
            owner1.setOwnerName("Sarah");
            owner1.setGivesTreats(true);
            owner1.setDog(dog1);
            Console.WriteLine($"\nOwner1 after setters: {owner1.getOwnerName()}, Gives treats: {owner1.getGivesTreats()}");
            Console.WriteLine($"Owner1's dog: {owner1.getDog().getName()}");

            // Test getAttacked method
            Console.Write("\nOwner2 getting attacked: ");
            owner2.getAttacked();

            // Test playWithDog method (with treats)
            Console.Write("Owner2 playing with dog (gives treats): ");
            owner2.playWithDog();

            // Test playWithDog method (without treats)
            owner1.setGivesTreats(false);
            Console.Write("Owner1 playing with dog (no treats): ");
            owner1.playWithDog();

            Console.WriteLine("\n\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
