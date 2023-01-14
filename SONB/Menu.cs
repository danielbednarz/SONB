using System.Collections.Concurrent;
using System.Threading;

namespace SONB
{
    public class Menu
    {
        public static bool DisplayMenu(BlockingCollection<string> collection)
        {
            Console.WriteLine("1. Wyslij prawidłową wiadomosc do serwerów");
            Console.WriteLine("2. Wyslij nieprawidłową wiadomosc z jednym bitem do losowego serwera");
            Console.WriteLine("3. Wyslij mniejszą liczbę wiadomości");
            Console.WriteLine("4. Wyslij null");
            Console.WriteLine("5. Wyslij nieprawidłową wiadomosc z dwoma bitami do losowego serwera");
            Console.WriteLine("0. Koniec");

            switch (Console.ReadLine())
            {
                case "1":
                    Server.StartMasterServer(collection, ExceptionType.NoException);
                    return true;
                case "2":
                    Server.StartMasterServer(collection, ExceptionType.IncorrectMessage);
                    return true;
                case "3":
                    Server.StartMasterServer(collection, ExceptionType.EmptyMessage);
                    return true;
                case "4":
                    Server.StartMasterServer(collection, ExceptionType.NullMessage);
                    return true;
                case "5":
                    Server.StartMasterServer(collection, ExceptionType.IncorrectMessageTwoBit);
                    return true;
                case "0":
                    return false;
                default:
                    return false;
            }
        }
    }
}
