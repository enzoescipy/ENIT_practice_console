namespace MainProject
{
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
        /// public static int GetInt(string description)
        /// get integer that are parsed by Int32.TryParse
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static int GetInt(string description)
        {
            while (true)
            {
                string result = Get(description);
                int resultParsed;
                bool validate = Int32.TryParse(result, out resultParsed);
                if (validate == true) {return resultParsed;} else {continue;}
            }
        }

        /// <summary>
        /// public static int GetInt(string description, Func<int, bool> formula)
        /// get integer that are parsed by Int32.TryParse.
        /// but, if integer did not satisfied the formula, then reject to return.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static int GetInt(string description, Func<int, bool> formula)
        {
            while (true)
            {
                string result = Get(description);
                int resultParsed;
                bool validate = Int32.TryParse(result, out resultParsed);
                if (validate == true && formula(resultParsed) == true) {return resultParsed;} else {continue;}
            }
        }

    }
}