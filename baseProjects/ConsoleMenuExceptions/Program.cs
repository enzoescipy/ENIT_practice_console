using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.WriteLine("PRINTING STAR SIMULATOR");
            Console.WriteLine("1. pyramid");
            Console.WriteLine("2. reversed-pyramid");
            Console.WriteLine("3. hourglass");
            Console.WriteLine("4. diamond");
            Console.WriteLine("5. exit");

            var mainMenuConnectionList = new List<Action> {WindowExit};

            int intMenu = Int32.Parse(UserInput.GetSpecific("select the menu that you want to enter  (1 ~ 5) : ", "12345", 1));
            intMenu += -1;

            mainMenuConnectionList[intMenu]();

        }

        static void WindowExit()
        {
            UserInput.Get("press enter to exit...  (any):");
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
        /// GetInt
        /// do same as Get but only accept the (range of) integer. 
        /// if other string has found, then re-ask to user again.
        /// </summary>
        /// <returns></returns>
        public static string GetInt(string description)
        {
            string? userInput = "";
            while (true)
            {
                Console.Write(description);
                userInput = Console.ReadLine();
                if (userInput == null) {continue;}
                string acceptableCollection = "0123456789";
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
