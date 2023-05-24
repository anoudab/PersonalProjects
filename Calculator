using System;

namespace ConsoleApp1
{
    public class Calculator
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the first number: ");
            int num1 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter the first number: ");
            int num2 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the number of the operation to do:");
            Console.WriteLine("1 for addition. \n2 for subtraction\n3 for multiplication \n4 for division");
            int input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    {
                        Console.WriteLine(addition(num1, num2));
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine(subtraction(num1, num2));
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine(multiplication(num1, num2));
                        break;
                    }
           ,    case 4:
                    {
                        Console.WriteLine(division(num1, num2));
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid Number");
                        break;
                    }

            }
        }

            public static int addition(int n1, int n2)
            {

                return n1 + n2;

            }
            public static int subtraction(int n1, int n2)
            {

                return n1 - n2;

            }
            public static int multiplication(int n1, int n2)
            {

                return n1 * n2;

            }
            public static int division(int n1, int n2)
            {

                return n1 / n2;

            }

        }
}
