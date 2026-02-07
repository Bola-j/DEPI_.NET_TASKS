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

        static void CalculateSumAndDifference(int num1, int num2, int num3, int num4, out int sum, out int difference)
        {
            sum = num1 + num2;
            difference = num3 - num4;
        }

        static void CalculateSum(int num, out int digitSum)
        {
            digitSum = 0;
            while (num > 0)
            {
                digitSum += num % 10;
                num /= 10;
            }
        }

        static bool IsPrime(int number)
        {
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }
        static void Main(string[] args)
        {
            #region Ques. 01
            ////value type
            //// By value -> the function operates on a copy of the data,
            ////      and changes it on its scope without affecting the original data outside the function.
            ////By reference -> the function operates on the original data,
            ////      and any changes made to it will affect the original data outside the function.

            //int a = 5;
            //Console.WriteLine($"Value before: {a}");
            //ValueTypeByValue(a);
            //Console.WriteLine($"Value type by value: {a}");
            //ValueTypeByReference(ref a);
            //Console.WriteLine($"Value type by reference: {a}");
            #endregion

            #region Ques. 02
            ////reference type
            //// By value -> the function operates on a copy of the reference (pointer) to the data,
            ////      and internal data member of the variable, whose reference passed by value, can be changed,
            ////      but the reference itself cannot be changed (e.g. reassigning the reference).
            ////By reference -> the function operates on the original reference (pointer) to the data,
            ////      and any changes made to the reference itself (e.g. reassigning the reference or changing its data attributes) will affect the original reference outside the function.

            //Cat myCat = new Cat("Kitty", 3, "White");
            //Console.WriteLine();
            //Console.Write("Cat before: ");
            //myCat.displayInfo();
            //Console.WriteLine();

            //ReferenceTypeByValue(myCat, "change");
            //Console.Write("Reference type by value (change): ");
            //myCat.displayInfo();
            //Console.WriteLine();

            //ReferenceTypeByValue(myCat, "reassign");
            //Console.Write("Reference type by value (reassign): ");
            //myCat.displayInfo();
            //Console.WriteLine();


            //ReferenceTypeByReference(ref myCat, "change");
            //Console.Write("Reference type by reference (change): ");
            //myCat.displayInfo();
            //Console.WriteLine();

            //ReferenceTypeByReference(ref myCat, "reassign");
            //Console.Write("Reference type by reference (reassign): ");
            //myCat.displayInfo();
            //Console.WriteLine();
            #endregion

            #region Ques. 03

            //Console.WriteLine("Enter first number:");
            //int number1 = int.Parse(Console.ReadLine());
            //Console.WriteLine("Enter second number:");
            //int number2 = int.Parse(Console.ReadLine());
            //Console.WriteLine("Enter third number:");
            //int number3 = int.Parse(Console.ReadLine());
            //Console.WriteLine("Enter fourth number:");
            //int number4 = int.Parse(Console.ReadLine());

            //CalculateSumAndDifference(number1, number2, number3, number4, out int sum, out int difference);

            //Console.WriteLine($"Sum (number1 + number2): {sum}");
            //Console.WriteLine($"Difference (number3 - number4): {difference}");
            #endregion

            #region Ques. 04
            //Console.WriteLine("Enter a number to calculate the sum of its digits:");
            //int number = int.Parse(Console.ReadLine());
            //CalculateSum(number, out int digitSum);
            //Console.WriteLine($"Sum of digits: {digitSum}");
            #endregion

            #region Ques. 05
            //Console.WriteLine("Enter a number to check if it's prime:");
            //int numberToCheck = int.Parse(Console.ReadLine());
            //Console.WriteLine($"The number {numberToCheck} is {(IsPrime(numberToCheck) ? "prime" : "not prime")}");
            #endregion

            
        }
    }
}
