namespace MainProject
{
    internal class Program
    {

        static void Main(string[] args)
        {
            // Debug();
            var page = new Page(new ClassDB("dbLight.txt"));
            while (true) {page.FrontMenu();}
        }
        public static void Debug()
        {
        }
    }

}