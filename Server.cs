using System.Collections.Concurrent;
using System.Text;
using static SONB.Hamming;

namespace SONB
{
    public class Server
    {

        static string codeString = "1011001010110010";
        static int errorPosition = 18;
        public static void StartMasterServer(BlockingCollection<string> collection, ExceptionType exceptionType)
        {
            Console.WriteLine("Serwer nadzorujący wystartował...");
            Console.WriteLine($"Wysyłam informację");
            Console.Clear();

            switch (exceptionType)
            {
                case ExceptionType.NoException:
                    SendCorrectMessageToAllServer(collection);
                    break;
                case ExceptionType.IncorrectMessage:
                    SendErrorMessageToRandomServer(collection);
                    break;
                case ExceptionType.EmptyMessage:
                    AddMissingMessage(collection);
                    break;
                default:
                    return;
            };

            StartServers(collection);
        }

        private static void SendCorrectMessageToAllServer(BlockingCollection<string> collection)
        {
            var code = Helpers.prettyStringToBoolArray(codeString);
            var encoded = Hamming.Encode(code);

            Console.WriteLine($"Serwer nadzorujacy - Message to encode: {Helpers.boolArrayToPrettyString(code)}");
            Console.WriteLine($"Serwer nadzorujacy - Encoded message: {Helpers.boolArrayToPrettyString(encoded)}");

            for (int i = 0; i < 7; i++)
            {
                collection.Add(Helpers.boolArrayToPrettyString(encoded));
            }
        }

        private static void SendErrorMessageToRandomServer(BlockingCollection<string> collection)
        {
            var code = Helpers.prettyStringToBoolArray(codeString);
            var encoded = Hamming.Encode(code);
            var encodedError = Hamming.Encode(code);

            Console.WriteLine($"Serwer nadzorujacy - Message to encode: {Helpers.boolArrayToPrettyString(code)}");
            Console.WriteLine($"Serwer nadzorujacy - Encoded message: {Helpers.boolArrayToPrettyString(encoded)}");
            MixinSingleError(encodedError, errorPosition);
            Console.WriteLine($"Serwer nadzorujacy - Message with error: {Helpers.boolArrayToPrettyString(encoded)}");



            Random rnd = new Random();
            int loss = rnd.Next(7);
            Console.WriteLine($"Serwer nadzorujacy - Błąd zostanie wysłany do serwera numer: {loss}");


            for (int i = 0; i < 7; i++)
            {
                if(loss==i)
                    collection.Add(Helpers.boolArrayToPrettyString(encodedError));
                collection.Add(Helpers.boolArrayToPrettyString(encoded));
            }
        }

        //private static void AddNullMessage(BlockingCollection<string[]> collection)
        //{
        //    for (int i = 0; i < 6; i++)
        //    {
        //        int[] correctInfo = { 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0 };
        //        collection.Add(correctInfo);
        //    }
        //    // dodanie wiadomosci null
        //    collection.Add(null);
        //}

        private static void AddMissingMessage(BlockingCollection<string> collection)
        {
            var code = Helpers.prettyStringToBoolArray(codeString);
            var encoded = Hamming.Encode(code);

            Console.WriteLine($"Serwer nadzorujacy - Message to encode: {Helpers.boolArrayToPrettyString(code)}");
            Console.WriteLine($"Serwer nadzorujacy - Encoded message: {Helpers.boolArrayToPrettyString(encoded)}");

            for (int i = 0; i < 6; i++)
            {
                collection.Add(Helpers.boolArrayToPrettyString(encoded));
            }
        }

        private static void StartServers(BlockingCollection<string> collection)
        {
            List<Thread> watki = new List<Thread>();

            for (int i = 0; i < 7; i++)
            {
                Thread watek = new(() =>
                {
                    Thread.Sleep(1000);

                    if (collection.TryTake(out string res))
                    {

                        if (res == null)
                        {
                            Console.WriteLine($"{Thread.CurrentThread.Name} - Blad, wartosc nie moze byc nullem");
                            return;
                        }

                        var encoded = Helpers.prettyStringToBoolArray(res);

                        Console.WriteLine($"{Thread.CurrentThread.Name} - Received encoded message: {Helpers.boolArrayToPrettyString(encoded)}");

                        int calculatedErrorPosition = ErrorSyndrome(encoded);
                        Console.WriteLine($"{Thread.CurrentThread.Name} - Error position: {calculatedErrorPosition}");

                        if (calculatedErrorPosition != 0)
                            encoded[calculatedErrorPosition - 1] = !encoded[calculatedErrorPosition - 1];
                        var decoded = Decode(encoded);
                        Console.WriteLine($"{Thread.CurrentThread.Name} - Message after decoding: {Helpers.boolArrayToPrettyString(decoded)}");

                        Console.WriteLine(Enumerable.SequenceEqual(Helpers.prettyStringToBoolArray(codeString), decoded) ? $"{Thread.CurrentThread.Name} - Messages are equal!" : $"{Thread.CurrentThread.Name} - Messages are not equal!");

                    }
                    else
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name}. Brak wiadomości do pobrania");
                    }

                });
                watek.Name = "Watek"+i;
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
