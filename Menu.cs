using System.Collections.Concurrent;

namespace SONB
{
    public class Menu
    {
        public static bool DisplayMenu(BlockingCollection<int[]> collection)
        {
            Console.WriteLine("1. Wyslij prawidłową wiadomosc do serwerów");
            Console.WriteLine("2. Wyslij nieprawidlowa wiadomosc do serwerów");
            Console.WriteLine("3. Wyslij pusta wiadomosc");
            Console.WriteLine("0. Koniec");

            switch (Console.ReadLine())
            {
                case "1":
                    Thread serverThread = new(() => Server.StartMasterServerWithCorrectMessage(collection));
                    serverThread.Start();
                    serverThread.Join();
                    Server.StartServers(collection);
                    return true;
                case "0":
                    return false;
                default:
                    return false;
            }
        }
    }
}
