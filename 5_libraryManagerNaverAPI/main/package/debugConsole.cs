namespace MainProject
{
    public static class DebugConsole
    {
        
        public static void Debug(string description)
        {
            Console.WriteLine("//deubg//  " + description);
        }

        public static void Debug(dynamic desceiption)
        {
            Debug(desceiption.ToString());
        }
        public static void D1List(dynamic list)
        {
            Func<dynamic, dynamic> func = (s) => {return s;};
            D1List(list, func);
        }
        public static void D1List(dynamic list, Func<dynamic, dynamic> func)
        {
            Console.Write("//debug//  ");
            try 
            {
                Console.Write("{");
                foreach (dynamic d in list)
                {
                    Console.Write(func(d).ToString());
                    Console.Write(',');
                }
                Console.Write('}');
            }
            catch (Exception e)
            {
                Console.Write("unexpected exception happens.");
                Console.Write(e);
            }
            Console.WriteLine();
        }

        public static void D2List(dynamic list)
        {
            Console.Write("//debug//  ");
            try 
            {
                Console.Write("{");
                foreach (dynamic d in list)
                {
                    D1List(d);
                    Console.Write(',');
                }
                Console.Write('}');
            }
            catch (Exception e)
            {
                Console.Write("unexpected exception happens.");
                Console.Write(e);
            }
            Console.WriteLine();
        }
    }
}