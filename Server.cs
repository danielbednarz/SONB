using System.Collections.Concurrent;

namespace SONB
{
    public class Server
    {
        public static void StartMasterServerWithCorrectMessage(BlockingCollection<int[]> collection)
        {
            Console.WriteLine("Serwer nadzorujący wystartował...");

            Console.WriteLine($"Wysyłam informację");
            for (int i = 0; i < 6; i++)
            {
                int[] correctInfo = { 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0 };
                collection.Add(correctInfo);
            }
        }

        public static void StartMasterServerWithNullMessage(BlockingCollection<int[]> collection)
        {
            Console.WriteLine("Serwer nadzorujący wystartował...");

            Console.WriteLine($"Wysyłam informację");
            for (int i = 0; i < 6; i++)
            {
                int[] correctInfo = { 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0 };
                collection.Add(correctInfo);
            }
            // dodanie wiadomosci null
            collection.Add(null);
        }

        public static void StartServers(BlockingCollection<int[]> collection)
        {
            List<Thread> watki = new List<Thread>();

            for (int i = 0; i < 7; i++)
            {
                Thread watek = new(() =>
                {
                    Thread.Sleep(1000);

                    if (collection.TryTake(out int[] res))
                    {
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
        }
    }
}
