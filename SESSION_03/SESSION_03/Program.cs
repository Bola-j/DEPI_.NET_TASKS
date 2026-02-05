using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Transactions;

namespace SESSION_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Ques. 01
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

            #region Ques. 02
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

            #region Ques. 03
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

            #region Ques. 04
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

            #region Ques. 05
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

            #region Ques. 06
            //Console.Write("Enter a number greater than 1:");
            //int n;
            //n = int.Parse(Console.ReadLine());
            //for(int i = 1; i <= n; i++)
            //{
            //    Console.Write($"{i} ");
            //}
            //Console.WriteLine();
            #endregion

            #region Ques. 07
            //Console.Write("Enter a number:");
            //int number;
            //number = int.Parse(Console.ReadLine());
            //for (int i = 1; i <= 12; i++)
            //{
            //    Console.Write($"{i*number} ");
            //}
            //Console.WriteLine();
            #endregion

            #region Ques. 08
            //Console.Write("Enter a number greater than 1:");
            //int Z;
            //Z = int.Parse(Console.ReadLine());
            //for (int i = 2; i <= Z; i+=2)
            //{
            //    Console.Write($"{i} ");
            //}
            //Console.WriteLine();
            #endregion

            #region Ques. 09
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

            #region Ques. 10
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

            #region Ques. 11
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

            #region Ques. 12
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

            #region Ques. 13
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

            #region Ques. 14
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

            #region Ques. 15
            //Console.Write("Enter starting number of the range: ");
            //int startingNum = int.Parse(Console.ReadLine());
            //Console.Write("Enter ending number of the range: ");
            //int endingNum = int.Parse(Console.ReadLine());
            //Console.Write($"Prime numbers between {startingNum} and {endingNum} are: ");
            //for (int i = startingNum; i <= endingNum; i++)
            //{
            //    if (isPrime(i))
            //    {
            //        Console.Write($"{i} ");
            //    }
            //}
            #endregion

            #region Ques. 16
            //Console.Write("Enter a decimal number to be converted into binary: ");
            //int decimalNum = int.Parse(Console.ReadLine());
            ////string binaryNum = Convert.ToString(decimalNum, 2);
            ////Console.WriteLine($"Binary representation of {decimalNum} is: {binaryNum}");
            //StringBuilder binaryNum = new StringBuilder();
            //binaryNum.Append('0', 32);
            //while (decimalNum > 0)
            //{
            //    int MSBB = Convert.ToInt32(Math.Log2(decimalNum));
            //    decimalNum -= (int)Math.Pow(2, MSBB);
            //    binaryNum[31 - MSBB] = '1';
            //}
            //Console.WriteLine($"Binary representation of {decimalNum} is: {binaryNum}");
            #endregion

            #region Ques. 17
            //Console.WriteLine("Enter first point");
            //Point p1 = new Point();
            //Console.Write("X1: ");
            //p1.X = int.Parse(Console.ReadLine());
            //Console.Write("Y1: ");
            //p1.Y = int.Parse(Console.ReadLine());

            //Console.WriteLine("Enter second point");
            //Point p2 = new Point();
            //Console.Write("X2: ");
            //p2.X = int.Parse(Console.ReadLine());
            //Console.Write("Y2: ");
            //p2.Y = int.Parse(Console.ReadLine());

            //Console.WriteLine("Enter third point");
            //Point p3 = new Point();
            //Console.Write("X3: ");
            //p3.X = int.Parse(Console.ReadLine());
            //Console.Write("Y3: ");
            //p3.Y = int.Parse(Console.ReadLine());


            //if (Point.lieOnSingleStraightLine(p1, p2, p3))
            //{
            //    Console.WriteLine("The points lie on a single straight line.");
            //}
            //else
            //{
            //    Console.WriteLine("The points do not lie on a single straight line.");
            //}
            #endregion

            #region Ques. 18
            //Console.WriteLine("Enter the number of hours you spend on a task: ");
            //float hours = float.Parse(Console.ReadLine());

            //if (hours >= 2 && hours < 3)
            //{
            //    Console.WriteLine("You are highly efficient.");
            //}
            //else if (hours >= 3 && hours < 4)
            //{
            //    Console.WriteLine("You need to increase your speed.");
            //}
            //else if (hours >= 4 && hours < 5)
            //{
            //    Console.WriteLine("You will be provided with training to enhance your speed.");
            //}
            //else if (hours >= 5)
            //{
            //    Console.WriteLine("You are required to leave the company.");
            //}
            //else
            //{
            //    Console.WriteLine("Invalid input: hours must be at least 2.");
            //}
            #endregion

            #region Ques. 19
            //Console.WriteLine("Enter a number: ");
            //int num = int.Parse(Console.ReadLine());
            //for(int i = 1; i <= num; i++)
            //{
            //    for (int j = 1; j <= num; j++)
            //    {
            //        if(j == i)
            //        {
            //            Console.Write("1 ");
            //            continue;
            //        }
            //        Console.Write("0 ");
            //    }
            //    Console.WriteLine();
            //}
            #endregion

            #region Ques. 20
            //int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7 };
            ////Console.WriteLine($"Sum = {numbers.Sum()}");
            //int sum = 0;
            //foreach(int num in numbers)
            //{
            //    sum += num;
            //}
            //Console.WriteLine(sum);
            #endregion

            #region Ques. 21
            //Console.Write("Enter the size of the arrays: ");
            //int size = int.Parse(Console.ReadLine());

            //int[] array1 = new int[size];
            //int[] array2 = new int[size];

            //Console.WriteLine("Enter elements for the first array:");
            //for (int i = 0; i < size; i++)
            //{
            //    Console.Write($"Element {i + 1}: ");
            //    array1[i] = int.Parse(Console.ReadLine());
            //}

            //Console.WriteLine("Enter elements for the second array:");
            //for (int i = 0; i < size; i++)
            //{
            //    Console.Write($"Element {i + 1}: ");
            //    array2[i] = int.Parse(Console.ReadLine());
            //}

            //int[] mergedArray = new int[size * 2];

            //for (int i = 0; i < size; i++)
            //{
            //    mergedArray[i] = array1[i];
            //    mergedArray[size + i] = array2[i];
            //}

            //Array.Sort(mergedArray);

            //Console.WriteLine("Merged array in ascending order:");
            //foreach (int num in mergedArray)
            //{
            //    Console.Write($"{num} ");
            //}
            //Console.WriteLine();
            #endregion

            #region Ques. 22
            //Dictionary<int, int> elementsCount = new Dictionary<int, int>();
            //int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3 };
            //foreach (var num in arr)
            //{
            //    Console.Write($"{num} ");
            //    if (elementsCount.ContainsKey(num))
            //    {
            //        elementsCount[num]++;
            //    }
            //    else
            //    {
            //        elementsCount[num] = 1; 
            //    }
            //}
            //Console.WriteLine();
            //foreach (var element in elementsCount)
            //{
            //    Console.WriteLine($"Element: {element.Key}, Count: {element.Value}");
            //}
            #endregion

            #region Ques. 23
            //int max = int.MinValue, min = int.MaxValue;
            //int[] array = new int[] { 42 ,56,67,243,234, 6000, 6790,2,3,53,6,8,9,22,1,0 };
            //foreach (var num in array) 
            //{
            //    Console.Write($"{num} ");
            //    if(max <= num)
            //    {
            //        max = num;
            //    }
            //    if (min >= num)
            //    {
            //        min = num;
            //    }
            //}
            //Console.WriteLine();
            //Console.WriteLine($"Minimum = {min}, Maximum = {max}");
            #endregion

            #region Ques. 24
            //int[] array = new int[] { 42, 56, 67, 243, 234 ,6790, 2, 3, 53, 6, 8, 9, 22, 1, 0, 6000 };
            ////the best method for the best performance,
            ////better than looping once for the max and once for the secondMax
            //Array.Sort(array);

            //Console.WriteLine();
            //Console.WriteLine($"second largest value = {array[^2]}");
            #endregion

            #region Ques. 25

            //Console.WriteLine("Enter integers separated by space:");
            //int[] array = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            //int maxDistance = int.MinValue;
            //Dictionary<int, int> leftElements = new Dictionary<int, int>();
            //for (int i = 0; i < array.Length; i++)
            //{
            //    if (leftElements.ContainsKey(array[i]))
            //    {
            //        int distance = i - leftElements[array[i]] - 1;
            //        if (distance >= maxDistance)
            //        {
            //            maxDistance = distance;
            //        }
            //    }
            //    else
            //    {
            //        leftElements[array[i]] = i; 
            //    }

            //}

            //Console.WriteLine($"Maximum distance between identical elements: {maxDistance}");
            #endregion

            #region Ques. 26
            //Console.WriteLine("Enter a sentence:");
            //string[] array = Console.ReadLine().Split(' ');
            //Array.Reverse(array);
            //Console.WriteLine(string.Join(" ", array));
            #endregion

            #region Ques. 27
            //Console.Write("Enter a number for first dimension: ");
            //int n = int.Parse(Console.ReadLine());
            //Console.Write("Enter a number for second dimension: ");
            //int m = int.Parse(Console.ReadLine());

            //int[,] x = new int[n,m];
            //for (int i = 0; i < n; i++)
            //{
            //    Console.WriteLine($"Dimension {i+1}");
            //    for (int j = 0; j < m; j++)
            //    {       
            //        Console.WriteLine($"Enter integer NO. {j+1}:");
            //        x[i, j] = int.Parse(Console.ReadLine());
            //    }
            //}
            //int[,] y = new int[n, m];
            //y = (int[,])x.Clone();

            //Console.WriteLine("\n\nX (original)\n");
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j < m; j++)
            //    {
            //        Console.Write($"{x[i, j]} ");
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine("\n\nY (copy from X)\n");
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j < m; j++)
            //    {
            //        Console.Write($"{y[i, j]} ");
            //    }
            //    Console.WriteLine();
            //}
            #endregion


            #region Ques. 28
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            Console.WriteLine("Original Array:");
            foreach (int num in arr)
            {
                Console.Write($"{num} ");
            }
            Console.WriteLine();

            Console.WriteLine("Array Reversed:");
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                Console.Write($"{arr[i]} ");
            }
            Console.WriteLine();
            #endregion
        }
        static bool isPrime(int n)
        {
            if (n <= 1) return false;
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0)
                    return false;
            }
            return true;
        }
    }
}
