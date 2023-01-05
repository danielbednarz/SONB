using SONB;
using System.Collections.Concurrent;

namespace AnotherImpl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BlockingCollection<string> collection = new();

            bool showMenu = true;
            while (showMenu)
            {
                showMenu = Menu.DisplayMenu(collection);
            }

            Console.WriteLine("Koniec..");
            Console.ReadLine();
        }

        
    }
}