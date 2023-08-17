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
            // Console.WriteLine(UserInput.GetInt("오직양수 : ", true, true)); // debug
            MainWindow();
        }

        static void MainWindow()
        {
            Console.WriteLine("PRINTING STAR SIMULATOR");
            Console.WriteLine("1. pyramid");
            Console.WriteLine("2. reversed-pyramid");
            Console.WriteLine("3. hourglass");
            Console.WriteLine("4. diamond");
            Console.WriteLine("5. exit");

            var mainMenuConnectionList = new List<Action> {WindowPyramid, WindowRevPyramid, WindowHourglass, WindowDiamond, WindowExit};

            int intMenu = Int32.Parse(UserInput.GetSpecific("select the menu that you want to enter  (1 ~ 5) : ", "12345", 1));
            intMenu += -1;

            mainMenuConnectionList[intMenu]();

        }

        static void WindowPyramid()
        {
            Console.WriteLine("I am pyramid.");
            int height = UserInput.GetInt("please enter the height of the pyramid : ", true, false);
            int emptyCount = height - 1;
            int starCount = 1;
            var starStringList = new List<string> ();
            for (int i=0; i < height; i++)
            {
                // make the one string line
                string starLine = "";
                string emptyString = String.Concat(Enumerable.Repeat(" ", emptyCount));
                string starString = String.Concat(Enumerable.Repeat("*", starCount));
                starLine = emptyString + starString + emptyString;
                starStringList.Add(starLine);

                // adjust the varables
                emptyCount -= 1;
                starCount += 2;
            }

            foreach(string s in starStringList)
            {
                Console.WriteLine(s);
            }

            UserInput.Get("press enter to return back ...  (any):");
            MainWindow();
        }

        static void WindowRevPyramid()
        {
            Console.WriteLine("I am Revpyramid.");
            int height = UserInput.GetInt("please enter the half height of the pyramid : ", true, false);
            int emptyCount = height - 1;
            int starCount = 1;
            var starStringList = new List<string> ();
            for (int i=0; i < height; i++)
            {
                // make the one string line
                string starLine = "";
                string emptyString = String.Concat(Enumerable.Repeat(" ", emptyCount));
                string starString = String.Concat(Enumerable.Repeat("*", starCount));
                starLine = emptyString + starString + emptyString;
                starStringList.Add(starLine);

                // adjust the varables
                emptyCount -= 1;
                starCount += 2;
            }
            
            // print them the reversed order.
            starStringList.Reverse();
            foreach(string s in starStringList)
            {
                Console.WriteLine(s);
            }

            UserInput.Get("press enter to return back ...  (any):");
            MainWindow();
        }

        static void WindowHourglass()
        {
            Console.WriteLine("I am Hourglass.");
            int height = UserInput.GetInt("please enter the half height of the Hourglass : ", true, false);
            int emptyCount = height - 1;
            int starCount = 1;
            var starStringList = new List<string> ();
            for (int i=0; i < height; i++)
            {
                // make the one string line
                string starLine = "";
                string emptyString = String.Concat(Enumerable.Repeat(" ", emptyCount));
                string starString = String.Concat(Enumerable.Repeat("*", starCount));
                starLine = emptyString + starString + emptyString;
                starStringList.Add(starLine);

                // adjust the varables
                emptyCount -= 1;
                starCount += 2;
            }
            
            starStringList.Reverse(); // reverse the stored lines first.
            // print the stored lines
            foreach(string s in starStringList)
            {
                Console.WriteLine(s);
            }

            // then print them the reversed order.
            starStringList.Reverse();
            foreach(string s in starStringList)
            {
                Console.WriteLine(s);
            }

            UserInput.Get("press enter to return back ...  (any):");
            MainWindow();
            UserInput.Get("press enter to exit...  (any):");
        }

        static void WindowDiamond()
        {
            Console.WriteLine("I am Diamond.");
            int height = UserInput.GetInt("please enter the half height of the pyramid : ", true, false);
            int emptyCount = height - 1;
            int starCount = 1;
            var starStringList = new List<string> ();
            for (int i=0; i < height; i++)
            {
                // make the one string line
                string starLine = "";
                string emptyString = String.Concat(Enumerable.Repeat(" ", emptyCount));
                string starString = String.Concat(Enumerable.Repeat("*", starCount));
                starLine = emptyString + starString + emptyString;
                starStringList.Add(starLine);

                // adjust the varables
                emptyCount -= 1;
                starCount += 2;
            }
            
            // print the stored lines
            foreach(string s in starStringList)
            {
                Console.WriteLine(s);
            }

            // then print them the reversed order. BUT, will not print the first line of reversed list.
            starStringList.Reverse();
            starStringList.RemoveAt(0); // remove the first line
            foreach(string s in starStringList)
            {
                Console.WriteLine(s);
            }

            UserInput.Get("press enter to return back ...  (any):");
            MainWindow();
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
        /// userInput length must be equal to the param length.
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
                if (userInput == null || userInput.Length != length) {continue;}
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
        public static int GetInt(string description)
        {
            string? userInput = "";
            bool isNegative = false;
            while (true)
            {
                Console.Write(description);
                userInput = Console.ReadLine();
                if (userInput == null) {continue;} // null safe
                // negative to positive
                if (String.Equals(userInput[0], '-'))
                {
                    if (userInput.Length == 1) {continue;}// if length is 1 then it is just '-', invalid expression.
                    isNegative = true;
                    userInput = userInput.Substring(1);
                }
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
                    return Int32.Parse(userInput) * (isNegative ? -1 : 1);
                }
            }
        }

        /// <summary>
        /// GetInt(string description, bool isPositive = true, bool isNonZero = false)
        /// zero-splitted boundary specified version of UserInput.GetInt()
        /// </summary>
        /// <param name="description"></param>
        /// <param name="isPositive">true : get only from positive number, false : get only from negative number</param>
        /// <param name="isNonZero">true : value can't be the zero, false : value can be the zero</param>
        /// <returns></returns>
        public static int GetInt(string description, bool isPositive = true, bool isNonZero = false)
        {
            string? userInput = "";
            bool isNegative = false;
            while (true)
            {
                Console.Write(description);
                userInput = Console.ReadLine();
                if (userInput == null) {continue;} // null safe
                // non-zero restriction
                if (String.Equals(userInput[0], '-') || String.Equals(userInput[0], '0'))
                {
                    // if userInput[0] is neither '-' nor '0' then it is not zero.
                    // so, it can be zero.

                    bool isZero = true;
                    if (userInput.Length != 1) // if userInput length over 1 then have to check each digit
                    {
                        foreach (var c in userInput.Substring(1))
                        {
                            if (!String.Equals(c, '0')) {isZero = false;}
                        }
                    } 
                    else
                    {
                        if (String.Equals(userInput[0], '-') == true) {continue;}
                    }
                    
                    // else if userInput length is equal to 1 then it is zero or '-', the wrong expression.

                    if (isNonZero == true && isZero == true)
                    {
                        // if the value is zero but the param isNonZero is true, then re-try again.
                        continue;
                    }
                }

                // negative to positive
                if (String.Equals(userInput[0], '-'))
                {
                    if (isPositive == true) {continue;} // boundary restriction
                    isNegative = true;
                    userInput = userInput.Substring(1);
                }
                if (isPositive == false) {continue;} // boundary restriction
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
                    return Int32.Parse(userInput) * (isNegative ? -1 : 1);
                }
            }
        }
    }
}
