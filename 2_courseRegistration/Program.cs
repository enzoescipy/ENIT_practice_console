namespace MainProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;
            // Debug();
            var page = new Page(new ClassDB("dbLight.txt"), new List<int>(), new List<int>());
            while (true) {page.FrontMenu();}
        }
        public static void Debug()
        {
        }
    }

}