namespace SESSION_04
{
    internal class Program
    {
        static void ValueTypeByValue(int x)
        {
            x = 10;
        }
        static void ValueTypeByReference(ref int x)
        {
            x = 10;
        }

        static void ReferenceTypeByValue(Cat cat, string s)
        {
            if(s== "change")
                cat.Name = "Tom";
             else if(s == "reassign")
                cat = new Cat("Whiskers", 2, "Black");
        }
        static void ReferenceTypeByReference(ref Cat cat, string s)
        {
            if (s == "change")
                cat.Age = 20;
            else if (s == "reassign")
                cat = new Cat("Whiskers", 2, "Black");
        }
        static void Main(string[] args)
        {
            #region Ques. 01
            //value type
            // By value -> the function operates on a copy of the data,
            //      and changes it on its scope without affecting the original data outside the function.
            //By reference -> the function operates on the original data,
            //      and any changes made to it will affect the original data outside the function.

            int a = 5;
            Console.WriteLine($"Value before: {a}");
            ValueTypeByValue(a);
            Console.WriteLine($"Value type by value: {a}");
            ValueTypeByReference(ref a);
            Console.WriteLine($"Value type by reference: {a}");
            #endregion

            #region Ques. 02
            //reference type
            // By value -> the function operates on a copy of the reference (pointer) to the data,
            //      and internal data member of the variable, whose reference passed by value, can be changed,
            //      but the reference itself cannot be changed (e.g. reassigning the reference).
            //By reference -> the function operates on the original reference (pointer) to the data,
            //      and any changes made to the reference itself (e.g. reassigning the reference or changing its data attributes) will affect the original reference outside the function.

            Cat myCat = new Cat("Kitty", 3, "White");
            Console.WriteLine();
            Console.Write("Cat before: ");
            myCat.displayInfo();
            Console.WriteLine();

            ReferenceTypeByValue(myCat, "change");
            Console.Write("Reference type by value (change): ");
            myCat.displayInfo();
            Console.WriteLine();

            ReferenceTypeByValue(myCat, "reassign");
            Console.Write("Reference type by value (reassign): ");
            myCat.displayInfo();
            Console.WriteLine();


            ReferenceTypeByReference(ref myCat, "change");
            Console.Write("Reference type by reference (change): ");
            myCat.displayInfo();
            Console.WriteLine();

            ReferenceTypeByReference(ref myCat, "reassign");
            Console.Write("Reference type by reference (reassign): ");
            myCat.displayInfo();
            Console.WriteLine();
            #endregion


        }
    }
}
