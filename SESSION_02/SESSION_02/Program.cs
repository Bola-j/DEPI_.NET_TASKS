namespace SESSION_02
{    

    internal class Program
    {
        static void Main(string[] args)
        {
            #region Question 1: Write a program that allows the user to enter a number then print it

            Console.WriteLine("===============Question1===============");
                Console.WriteLine("Enter a number:");
                string input = Console.ReadLine();
                Console.WriteLine($"You entered: {input}");
                Console.WriteLine();
            #endregion

            #region Question 2: Convert a string to an integer, but the string contains non-numeric characters
                Console.WriteLine("===============Question2===============");

                Console.WriteLine("Enter a string");
                string nonNumericString = Console.ReadLine();
                Console.WriteLine($"String to convert: '{nonNumericString}'");
                try
                {
                    int result = Convert.ToInt32(nonNumericString);
                    Console.WriteLine($"Converted value: {result}");
                }
                catch
                {
                }
                Console.WriteLine();
            #endregion
            #region Question 3: Perform a simple arithmetic operation with floating-point numbers
                Console.WriteLine("===============Question3===============");

                // there are an error happens in floating-point number arthmetic because some numbers cannot be represented exactly in binary format.
                // float -> rounding errors after 6-7 decimal places
                // double -> rounding errors after 15-16 decimal places, So it might be more precise than float but bigger.
                //I will use float then round the result
                float num1 = 0.1f;
                float num2 = 0.2f;
                float sum = MathF.Round(num1 + num2,5);
                Console.WriteLine($"The sum of {num1} and {num2} is: {sum}");
                Console.WriteLine();
            #endregion

            #region Question 4: Extract a substring from a given string
                Console.WriteLine("===============Question4===============");

                string originalString = "Ana henak, Etla3ly fo2, ana ta7t";
                Console.WriteLine("Original String: " + originalString);
                Console.WriteLine("Sliced String: " + originalString[4..^11]);
                Console.WriteLine();
            #endregion

            #region Question 5: Assigning one value type variable to another and modifying the value
            Console.WriteLine("===============Question5===============");

            int valueType1 = 10;
            int valueType2 = 20;
            Console.WriteLine($"valueType1: {valueType1}");
            Console.WriteLine($"valueType2: {valueType2}");
            valueType2 = valueType1;
            Console.WriteLine($"After valueType2 = valueType1");
            Console.WriteLine($"valueType1: {valueType1}");
            Console.WriteLine($"valueType2: {valueType2}");
            valueType1 = 20;
            Console.WriteLine($"After modifying valueType1");
            Console.WriteLine($"valueType1: {valueType1}");
            Console.WriteLine($"valueType2: {valueType2}");
            Console.WriteLine();
            //that'a because they're value type variables
            #endregion

            #region Question 6: Assigning one reference type variable to another and modifying the object
                Console.WriteLine("===============Question6===============");

                Duck duck1 = new Duck("Donald", "White");
                Duck duck2 = new Duck("Dewie", "White");
                Console.Write("duck1 - ");
                duck1.Display();
                Console.Write("duck2 - ");
                duck2.Display();
                duck2 = duck1; 
                Console.WriteLine("After duck2 = duck1:");
                Console.Write("duck1 - ");
                duck1.Display();
                Console.Write("duck2 - ");
                duck2.Display();

                duck1.Name = "Daffy";
                duck1.Color = "Black";
                Console.WriteLine("\nAfter modifying duck1:");
                Console.Write("duck1 - ");
                duck1.Display();
                Console.Write("duck2 - ");
                duck2.Display();
                //That's because they're reference type variables
            #endregion


            #region Question 7: Take two string variables and print them as one variable
                Console.WriteLine("===============Question7===============");

                string str1 = "Hello";
                string str2 = "World";
                string combined = str1 + " " + str2;
                Console.WriteLine($"String 1: '{str1}'");
                Console.WriteLine($"String 2: '{str2}'");
                Console.WriteLine($"Combined: '{combined}'");
                Console.WriteLine();
            #endregion


            #region Question 8: Determine output of Convert.ToInt32( !(30 < 20) )
                 Console.WriteLine("===============Question8===============");

                Console.WriteLine("Code: int d = Convert.ToInt32( !(30 < 20) );");
                Console.WriteLine("b)	A value 1 will be assigned to d.");
                int d = Convert.ToInt32(!(30 < 20));
                Console.WriteLine($"Value of d: {d}");
                Console.WriteLine();
            #endregion


            #region Question 9: Determine output of 13 / 2 + " " + 13 % 2
            Console.WriteLine("===============Question9===============");

            Console.WriteLine("Code: Console.WriteLine(13 / 2 + \" \" + 13 % 2);");
            Console.WriteLine("d)   6 1");
            Console.WriteLine(13 / 2 + " " + 13 % 2);
            Console.WriteLine();
            #endregion

            #region Question 10: Determine output of the given code
            Console.WriteLine("===============Question10===============");

            Console.WriteLine("Code:");
            Console.WriteLine("int num = 1, z = 5;");
            Console.WriteLine("if (!(num <= 0))");
            Console.WriteLine("    Console.WriteLine( ++num + z++ + \" \" + ++z );");
            Console.WriteLine("else");
            Console.WriteLine("    Console.WriteLine( --num + z-- + \" \" + --z );");
            Console.WriteLine("Answer:");
            Console.WriteLine("d)   7 7");
            #endregion
        }
    }
}
