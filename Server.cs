using System.Collections.Concurrent;

namespace SONB
{
    public class Server
    {
        public static void StartMasterServer(BlockingCollection<int[]> collection, ExceptionType exceptionType)
        {
            Console.WriteLine("Serwer nadzorujący wystartował...");
            Console.WriteLine($"Wysyłam informację");
            Console.Clear();

            switch (exceptionType)
            {
                case ExceptionType.NoException:
                    AddCorrectMessage(collection);
                    break;
                case ExceptionType.IncorrectMessage:
                    AddNullMessage(collection);
                    break;
                case ExceptionType.EmptyMessage:
                    AddMissingMessage(collection);
                    break;
                default:
                    return;
            };

            StartServers(collection);
        }

        private static void AddCorrectMessage(BlockingCollection<int[]> collection)
        {
            for (int i = 0; i < 7; i++)
            {
                int[] correctInfo = { 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0 };
                collection.Add(correctInfo);
            }
        }

        private static void AddNullMessage(BlockingCollection<int[]> collection)
        {
            for (int i = 0; i < 6; i++)
            {
                int[] correctInfo = { 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0 };
                collection.Add(correctInfo);
            }
            // dodanie wiadomosci null
            collection.Add(null);
        }

        private static void AddMissingMessage(BlockingCollection<int[]> collection)
        {
            for (int i = 0; i < 6; i++)
            {
                int[] correctInfo = { 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0 };
                collection.Add(correctInfo);
            }
        }

        private static void StartServers(BlockingCollection<int[]> collection)
        {
            List<Thread> watki = new List<Thread>();

            for (int i = 0; i < 7; i++)
            {
                Thread watek = new(() =>
                {
                    Thread.Sleep(1000);

                    if (collection.TryTake(out int[] res))
                    {
                        if (res == null)
                        {
                            Console.WriteLine("Blad, wartosc nie moze byc nullem");
                            return;
                        }
                        Console.WriteLine($"Odbieram {string.Join("", res)} ");
                    }
                    else
                    {
                        Console.WriteLine("Brak wiadomości do pobrania");
                    }

                });
                watki.Add(watek);
                watek.Start();
            }
            foreach (var watek in watki)
            {
                watek.Join();
            }

            Console.WriteLine("Naciśnij klawisz, żeby kontynuować...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
