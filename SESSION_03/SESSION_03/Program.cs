using System.Runtime.Intrinsics.X86;

namespace SESSION_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Question 1
            //Console.Write("Enter a number: ");
            //int X; X = int.Parse(Console.ReadLine());
            //if (X%3 == 0 && X%4 == 0)
            //{
            //    Console.WriteLine("YES");
            //}
            //else
            //{
            //    Console.WriteLine("NO");
            //}
            #endregion

            #region Question 2
            //Console.Write("Enter a number: ");
            //int Y; Y = int.Parse(Console.ReadLine());
            ////Console.WriteLine(Y);
            //if (Y < 0)
            //{
            //    Console.WriteLine("negative");
            //}
            //else if (Y == 0)
            //{
            //    Console.WriteLine("ZERO");
            //}
            //else
            //{
            //    Console.WriteLine("positive");
            //}
            #endregion

            #region Question 3
            //    int A,B,C;
            //    Console.Write("Enter first number: ");
            //    A = int.Parse(Console.ReadLine());
            //    Console.Write("Enter second number: ");
            //    B = int.Parse(Console.ReadLine());
            //    Console.Write("Enter third number: ");
            //    C = int.Parse(Console.ReadLine());

            //    if (A >= B)
            //    {
            //        if (A >= C)
            //        {
            //            Console.WriteLine($"max element = {A}");
            //            Console.WriteLine($"min element = {((B>=C)? C:B)}");

            //        }
            //        else
            //        {
            //            Console.WriteLine($"max element = {C}");
            //            Console.WriteLine($"min element = {B}");
            //        }
            //}
            //else
            //{
            //    if(B >= C)
            //    {
            //        Console.WriteLine($"max element = {B}");
            //        Console.WriteLine($"min element = {((A>=C)? C:A)}");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"max element = {C}");
            //        Console.WriteLine($"min element = {A}");
            //    }
            //}
            #endregion

            #region Question 4
            //int num; 
            //Console.Write("Enter a number: ");
            //num = int.Parse(Console.ReadLine());
            //if (num % 2 == 0)
            //{
            //    Console.WriteLine("Even");
            //}
            //else
            //{
            //    Console.WriteLine("Odd");
            //}
            #endregion

            #region Question 5
            //Console.Write("Enter a character: ");
            //char ch = Console.ReadLine()[0];
            //ch = ch.ToString().ToLower()[0];
            //if ("aeiou".Contains(ch))
            //{
            //    Console.WriteLine("Vowel");

            //}
            //else
            //{
            //    Console.WriteLine("Consonant");
            //}
            #endregion

            #region Question 6
            //Console.Write("Enter a number greater than 1:");
            //int n;
            //n = int.Parse(Console.ReadLine());
            //for(int i = 1; i <= n; i++)
            //{
            //    Console.Write($"{i} ");
            //}
            //Console.WriteLine();
            #endregion

            #region Ques 7
            //Console.Write("Enter a number:");
            //int number;
            //number = int.Parse(Console.ReadLine());
            //for (int i = 1; i <= 12; i++)
            //{
            //    Console.Write($"{i*number} ");
            //}
            //Console.WriteLine();
            #endregion

            #region Question 8
            //Console.Write("Enter a number greater than 1:");
            //int Z;
            //Z = int.Parse(Console.ReadLine());
            //for (int i = 2; i <= Z; i+=2)
            //{
            //    Console.Write($"{i} ");
            //}
            //Console.WriteLine();
            #endregion

            #region Question 9
            //int Base, Power;
            //Console.Write("Enter Base: ");
            //Base = int.Parse(Console.ReadLine());
            //Console.Write("Enter Power: ");
            //Power =  int.Parse(Console.ReadLine());
            ////Console.WriteLine(Math.Pow(Base, Power);
            //int temp = Base;
            //for(int i = 1; i < Power; i++)
            //{
            //    temp *= Base;
            //}
            //Console.WriteLine(temp);
            #endregion

            #region Question 10
            //int total = 0, noOfSubs = 5;
            //float Avg, Percent;
            //for(int i = 0; i < noOfSubs; i++)
            //{
            //    Console.WriteLine($"Enter grade for subject{i+1}: ");
            //    total += int.Parse(Console.ReadLine());
            //}
            //Console.WriteLine($"total: {total}");
            //Avg = total / noOfSubs;
            //Percent = total / noOfSubs;
            //Console.WriteLine($"Average: {Avg}");
            //Console.WriteLine($"Percentage: %{Percent}");
            #endregion

            #region Question 11
            //Console.Write("Enter the month number: ");
            //int month = int.Parse(Console.ReadLine());
            //switch (month)
            //{
            //    case 1:
            //    case 3:
            //    case 5:
            //    case 7:
            //    case 8:
            //    case 10:
            //    case 12:
            //        Console.WriteLine("31 days");
            //        break;
            //    case 4:
            //    case 6:
            //    case 9:
            //    case 11:
            //        Console.WriteLine("30 days");
            //        break;
            //    case 2:
            //        Console.WriteLine("28 or 29 days");
            //        break;
            //    default:
            //        Console.WriteLine("Invalid month number");
            //        break;

            //}
            #endregion

            #region Question 12
            //double num1, num2, result;
            //char operation;

            //Console.Write("Enter first number: ");
            //num1 = double.Parse(Console.ReadLine());

            //Console.Write("Enter operation (+, -, *, /): ");
            //operation = Console.ReadLine()[0];

            //Console.Write("Enter second number: ");
            //num2 = double.Parse(Console.ReadLine());

            //switch (operation)
            //{
            //    case '+':
            //        result = num1 + num2;
            //        Console.WriteLine($"{num1} + {num2} = {result}");
            //        break;
            //    case '-':
            //        result = num1 - num2;
            //        Console.WriteLine($"{num1} - {num2} = {result}");
            //        break;
            //    case '*':
            //        result = num1 * num2;
            //        Console.WriteLine($"{num1} * {num2} = {result}");
            //        break;
            //    case '/':
            //        if (num2 != 0)
            //        {
            //            result = num1 / num2;
            //            Console.WriteLine($"{num1} / {num2} = {result}");
            //        }
            //        else
            //        {
            //            Console.WriteLine("Error: Division by zero");
            //        }
            //        break;
            //    default:
            //        Console.WriteLine("Invalid operation");
            //        break;
            //}
            #endregion

            #region Question 13
            //Console.Write("Enter a string: ");
            //string str = Console.ReadLine();
            //string revStr = "";
            ////revStr = string.Concat(str.Reverse());
            ////Console.WriteLine($"Reversed string: {revStr}");
            //for(int i = str.Length - 1; i >= 0; i--)
            //{
            //    revStr += str[i];
            //};
            //Console.WriteLine($"Reversed string: {revStr}");
            #endregion

            #region Question 14
            //Console.Write("Enter a number: ");
            //int normalInt = int.Parse(Console.ReadLine());
            //int revInt = 0;
            //while (normalInt > 0)
            //{
            //    revInt = revInt * 10 + normalInt % 10;

            //    normalInt /= 10;
            //}
            //Console.WriteLine($"reversed intger is: {revInt}");
            #endregion


        }
    }
}
