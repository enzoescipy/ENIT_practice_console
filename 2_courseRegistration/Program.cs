namespace MainProject
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Debug();
            // while (true) {MenuPage.FrontMenu();}
        }
        public static void Debug()
        {
            var debug = new ClassDB("dbLight.txt");
        }
    }

}