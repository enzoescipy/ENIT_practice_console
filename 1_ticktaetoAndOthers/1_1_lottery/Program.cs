using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainWindow();
            // Console.WriteLine(UserInput.GetSpecific("정수내놔 : ", ""));
        }

        static void MainWindow()
        {
            Console.WriteLine("WELCOME TO LOTTERY");
            Console.WriteLine("YOU HAVE TO CHOOSE 6 different 1~45 integer.");

            var lotteryUserChoice = new List<int>();
            for (int i=0; i < 6; i++)
            {   
                while (true)
                {
                    var userInput = UserInput.GetSpecificLengthRange("Please choose the number : ", "0123456789", 1, 2);
                    var parsedInput = Int32.Parse(userInput);
                    if (parsedInput <= 0 || parsedInput >= 46)
                    {
                        Console.WriteLine("<<please put the vaild number.>>");
                        continue;
                    }
                    if (lotteryUserChoice.Contains(parsedInput))
                    {
                        Console.WriteLine("<<you already chose that number.>>");
                        continue;
                    }

                    lotteryUserChoice.Add(parsedInput);
                    break;
                }
            }

            // DebugConsole.D1List(lotteryUserChoice);

            WindowExit(lotteryUserChoice);
        }

        static void WindowExit(List<int> lotteryUserChoice)
        {
            var randomIntList = new List<int>();
            var randomClass = new System.Random();
            int answerCount = 0; // count the answer
            for (int i=0; i < 6; i++)
            {
                randomIntList.Add(randomClass.Next(1, 47));
            }

            // print the answer
            for (int i=0; i<6; i++)
            {
                Console.Write(randomIntList[i]);
                Console.Write(" ");

                // print the user choices
                Console.Write(lotteryUserChoice[i]);
                Console.Write(" ");

                // print the check for right or wrong
                bool isUserCorrect = randomIntList[i] == lotteryUserChoice[i];
                Console.Write( isUserCorrect ? "O" : "X");
                Console.WriteLine(" ");

                if (isUserCorrect) {answerCount++;}
            }
            
            Console.Write("you got the answer of : ");
            Console.WriteLine(answerCount.ToString());
            if (answerCount <= 2) {Console.WriteLine("Got no luck today. that's ok! it'll be better than this time.");}
            else if (answerCount == 3) {Console.WriteLine("4th place!");}
            else if (answerCount == 4) {Console.WriteLine("3rd place!");}
            else if (answerCount == 5) {Console.WriteLine("2nd place! Oh, Just a little bit, well... ");}
            else if (answerCount == 6) {Console.WriteLine("the First place! Congratulations!");}

            UserInput.Get("press enter to exit ...  (any):");
        }

    }

    public static class DebugConsole
    {
        public static void D1List(dynamic list)
        {
            try 
            {
                Console.Write("{");
                foreach (dynamic d in list)
                {
                    Console.Write(d.ToString());
                    Console.Write(',');
                }
                Console.Write('}');
            }
            catch (Exception e)
            {
                Console.Write("unexpected exception happens.");
                Console.Write(e);
            }

        }
    }

    public static class UserInput
    {
        /// <summary>
        /// GetUserInput
        /// print the description through console then get the user input string.
        /// </summary>
        /// <returns></returns>
        public static string Get(string description) 
        {
            while (true)
            {
                Console.Write(description);
                var result = Console.ReadLine();
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }
        }
        /// <summary>
        /// GetSpecific
        /// do same as Get but only accept the value that only made of acceptableCollection string members
        /// if other string has found, then re-ask to user again.
        /// </summary>
        /// <param name="description">non-empty string.</param>
        /// <param name="acceptableCollection"></param>
        /// <returns></returns>
        public static string GetSpecific(string description, string acceptableCollection)
        {
            if (acceptableCollection.Length == 0)
            {
                throw new System.ArgumentException("param acceptableCollection must be non-empty string.");
            }
            string? userInput = "";
            while (true)
            {
                Console.Write(description);
                userInput = Console.ReadLine();
                if (userInput == null) {continue;}
                bool isAcceptable = true;
                foreach (var c in userInput) 
                {
                    if (!acceptableCollection.Contains(c))
                    {
                        isAcceptable = false;
                        break;
                    }
                }
                if (isAcceptable == false)
                {
                    continue;
                }
                else 
                {
                    return userInput;
                }
            }
        }
        /// <summary>
        /// GetSpecific(string description, string acceptableCollection, int length)
        /// input length specified version of UserInput.GetSpecific().
        /// </summary>
        /// <param name="description"></param>
        /// <param name="acceptableCollection">non-empty string</param>
        /// <param name="length">positive integer</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static string GetSpecific(string description, string acceptableCollection, int length)
        {
            if (acceptableCollection.Length == 0)
            {
                throw new System.ArgumentException("param acceptableCollection must be non-empty string.");
            }
            if (length < 1)
            {
                throw new System.ArgumentException("param length must be positive integer.");
            }
            string? userInput = "";
            while (true)
            {
                Console.Write(description);
                userInput = Console.ReadLine();
                if (userInput == null  || userInput.Length != length) {continue;}
                bool isAcceptable = true;
                foreach (var c in userInput) 
                {
                    if (!acceptableCollection.Contains(c))
                    {
                        isAcceptable = false;
                        break;
                    }
                }
                if (isAcceptable == false)
                {
                    continue;
                }
                else 
                {
                    return userInput;
                }
            }
        }

        /// <summary>
        /// GetSpecificLengthRange
        /// functions same as UserInput.GetSpecific(), but length param is now roll in the range 
        /// between lower, and the upper boundary.
        /// lowerbound <= userInput.Length <= upperbound
        /// </summary>
        /// <param name="description"></param>
        /// <param name="acceptableCollection">non-empty string</param>
        /// <param name="lowerbound"></param>
        /// <param name="upperbound"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static string GetSpecificLengthRange(string description, string acceptableCollection, int lowerbound, int upperbound)
        {
            if (acceptableCollection.Length == 0)
            {
                throw new System.ArgumentException("param acceptableCollection must be non-empty string.");
            }
            if (lowerbound > upperbound)
            {
                throw new System.ArgumentException("param lowerbound must be smaller then upperbound.");
            }
            if (lowerbound < 0 || upperbound < 1)
            {
                throw new System.ArgumentException("param lowerbound must be positive integer. param upperbound must be bigger than 0.");
            }
            string? userInput = "";
            while (true)
            {
                Console.Write(description);
                userInput = Console.ReadLine();
                if (userInput == null || upperbound < userInput.Length || userInput.Length < lowerbound ) {continue;}
                bool isAcceptable = true;
                foreach (var c in userInput) 
                {
                    if (!acceptableCollection.Contains(c))
                    {
                        isAcceptable = false;
                        break;
                    }
                }
                if (isAcceptable == false)
                {
                    continue;
                }
                else 
                {
                    return userInput;
                }
            }
        }

    }
}