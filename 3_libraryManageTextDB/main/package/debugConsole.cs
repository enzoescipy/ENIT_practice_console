namespace MainProject
{
    public static class DebugConsole
    {
        public static void D1List(dynamic list)
        {
            Func<dynamic, dynamic> func = (s) => {return s;};
            D1List(list, func);
        }
        public static void D1List(dynamic list, Func<dynamic, dynamic> func)
        {
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

        }

        public static void D2List(dynamic list)
        {
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
        }
    }
}