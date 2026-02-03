namespace SESSION_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Question 1
            Console.Write("Enter a number: ");
            int X; X = int.Parse(Console.ReadLine());
            if (X%3 == 0 && X%4 == 0)
            {
                Console.WriteLine("YES");
            }
            else
            {
                Console.WriteLine("NO");
            }
            #endregion

            #region Question 2
            Console.Write("Enter a number: ");
            int Y; Y = int.Parse(Console.ReadLine());
            //Console.WriteLine(Y);
            if (Y < 0)
            {
                Console.WriteLine("negative");
            }
            else if (Y == 0)
            {
                Console.WriteLine("ZERO");
            }
            else
            {
                Console.WriteLine("positive");
            }
            #endregion

            #region Question 3
            int A,B,C;
            Console.Write("Enter first number: ");
            A = int.Parse(Console.ReadLine());
            Console.Write("Enter second number: ");
            B = int.Parse(Console.ReadLine());
            Console.Write("Enter third number: ");
            C = int.Parse(Console.ReadLine());

            if (A >= B)
            {
                if (A >= C)
                {
                    Console.WriteLine($"max element = {A}");
                    Console.WriteLine($"min element = {((B>=C)? C:B)}");

                }
                else
                {
                    Console.WriteLine($"max element = {C}");
                    Console.WriteLine($"min element = {B}");
                }
        }
        else
        {
            if(B >= C)
            {
                Console.WriteLine($"max element = {B}");
                Console.WriteLine($"min element = {((A>=C)? C:A)}");
            }
            else
            {
                Console.WriteLine($"max element = {C}");
                Console.WriteLine($"min element = {A}");
            }
        }
            #endregion

        }
    }
}
