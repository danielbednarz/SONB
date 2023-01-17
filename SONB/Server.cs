using System.Collections.Concurrent;
using static SONB.Hamming;

namespace SONB
{
    public class Server
    {
        static readonly string codeString = "1011001010110010";
        static readonly Random rnd = new();
        public static void StartMasterServer(BlockingCollection<string> collection, ExceptionType exceptionType)
        {
            Helpers.ClearCollection(collection);

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
                case ExceptionType.IncorrectMessageTwoBit:
                    SendErrorMessagetwoBitToRandomServer(collection);
                    break;
                case ExceptionType.IncorrectMessageNotHamming:
                    SendErrorMessageNotHammingToRandomServer(collection);
                    break;
                case ExceptionType.EmptyMessage:
                    AddMissingMessage(collection);
                    break;
                case ExceptionType.NullMessage:
                    AddNullMessage(collection);
                    break;
                default:
                    return;
            };

            StartServers(collection);
        }

        private static void SendCorrectMessageToAllServer(BlockingCollection<string> collection)
        {
            var encoded = Helpers.GetEncodedCodeToSend(codeString);

            for (int i = 0; i < 7; i++)
            {
                collection.Add(Helpers.ConvertBoolArrayToString(encoded));
            }
        }

        private static void SendErrorMessageToRandomServer(BlockingCollection<string> collection)
        {
            var errorPosition = rnd.Next(3, 22);
            while (Helpers.IsPowerOfTwo(errorPosition))
            {
                errorPosition = rnd.Next(3, 22);
            }
            var code = Helpers.ConvertStringToBoolArray(codeString);
            var encoded = Hamming.Encode(code);
            var encodedError = Hamming.Encode(code);

            Console.WriteLine($"Serwer nadzorujacy - Wiadomość do zakodowania: {Helpers.ConvertBoolArrayToString(code)}");
            Console.WriteLine($"Serwer nadzorujacy - Zakodowana wiadomość: {Helpers.ConvertBoolArrayToString(encoded)}");
            MixinSingleError(encodedError, errorPosition);
            Console.WriteLine($"Serwer nadzorujacy - Wiadomość z błędem:   {Helpers.ConvertBoolArrayToString(encodedError)} ({errorPosition})");

            int loss = rnd.Next(7);
            Console.WriteLine($"Serwer nadzorujacy - Błąd zostanie wysłany do losowego serwera\n");

            for (int i = 0; i < 7; i++)
            {
                if (loss == i)
                    collection.Add(Helpers.ConvertBoolArrayToString(encodedError));
                collection.Add(Helpers.ConvertBoolArrayToString(encoded));
            }
        }


        private static void SendErrorMessageNotHammingToRandomServer(BlockingCollection<string> collection)
        {
 
            var code = Helpers.ConvertStringToBoolArray(codeString);
            var encoded = Hamming.Encode(code);

            int loss = rnd.Next(7);

            for (int i = 0; i < 7; i++)
            {
                if (loss == i)
                    collection.Add(Helpers.ConvertBoolArrayToString(code));
                collection.Add(Helpers.ConvertBoolArrayToString(encoded));
            }
        }

        private static void SendErrorMessagetwoBitToRandomServer(BlockingCollection<string> collection)
        {
            var errorPosition = rnd.Next(3, 22);
            var errorPosition2 = rnd.Next(3, 22);
            while (Helpers.IsPowerOfTwo(errorPosition))
            {
                errorPosition = rnd.Next(3, 22);
            }
            while (errorPosition == errorPosition2 || (Helpers.IsPowerOfTwo(errorPosition2)))
            {
                errorPosition2 = rnd.Next(3, 22);
            }  
            var code = Helpers.ConvertStringToBoolArray(codeString);
            var encoded = Hamming.Encode(code);
            var encodedError = Hamming.Encode(code);

            Console.WriteLine($"Serwer nadzorujacy - Wiadomość do zakodowania: {Helpers.ConvertBoolArrayToString(code)}");
            Console.WriteLine($"Serwer nadzorujacy - Zakodowana wiadomość: {Helpers.ConvertBoolArrayToString(encoded)}");
            MixinDoubleError(encodedError, errorPosition, errorPosition2);
            Console.WriteLine($"Serwer nadzorujacy - Wiadomość z dwoma błedami:   {Helpers.ConvertBoolArrayToString(encodedError)} ({errorPosition})({errorPosition2}) ");

            int loss = rnd.Next(7);
            Console.WriteLine($"Serwer nadzorujacy - Błąd zostanie wysłany do losowego serwera\n");

            for (int i = 0; i < 7; i++)
            {
                if (loss == i)
                    collection.Add(Helpers.ConvertBoolArrayToString(encodedError));
                collection.Add(Helpers.ConvertBoolArrayToString(encoded));
            }
        }

        private static void AddNullMessage(BlockingCollection<string> collection)
        {
            var encoded = Helpers.GetEncodedCodeToSend(codeString);
            var randomError = rnd.Next(7);

            for (int i = 0; i < 7; i++)
            {
                collection.Add(Helpers.ConvertBoolArrayToString(encoded));
                if (i == randomError)
                {
                    // dodanie wiadomosci null
                    collection.Add(null);
                }
            }

        }

        private static void AddMissingMessage(BlockingCollection<string> collection)
        {
            var encoded = Helpers.GetEncodedCodeToSend(codeString);

            for (int i = 0; i < 6; i++)
            {
                collection.Add(Helpers.ConvertBoolArrayToString(encoded));
            }
        }

        private static void StartServers(BlockingCollection<string> collection)
        {
            List<Thread> watki = new();

            for (int i = 0; i < 7; i++)
            {
                Thread watek = new(() =>
                {
                    Thread.Sleep(1000);

                    if (collection.TryTake(out string res))
                    {

                        if (res == null)
                        {
                            Console.WriteLine($"! {Thread.CurrentThread.Name} - Błąd, wartość nie może być nullem");
                            return;
                        }

                        if (res.Length < 21)
                        {
                            Console.WriteLine($"! {Thread.CurrentThread.Name} - Błąd, wiadomośc nie jest kodem Hamminga");
                            return;
                        }

                        var encoded = Helpers.ConvertStringToBoolArray(res);

                        Console.WriteLine($"{Thread.CurrentThread.Name} - Otrzymano zakodowaną wiadomość: {Helpers.ConvertBoolArrayToString(encoded)}");

                        int calculatedErrorPosition = ErrorSyndrome(encoded);
                        if (calculatedErrorPosition != 0)
                        {
                            Console.WriteLine($"! {Thread.CurrentThread.Name} - Błąd na pozycji: {calculatedErrorPosition}");
                            encoded[calculatedErrorPosition - 1] = !encoded[calculatedErrorPosition - 1];
                            Console.WriteLine($"! {Thread.CurrentThread.Name} - Błąd naprawiony");
                        }

                        var decoded = Decode(encoded);
                        Console.WriteLine($"{Thread.CurrentThread.Name} - Wiadomość po odkodowaniu: {Helpers.ConvertBoolArrayToString(decoded)}");

                        Console.WriteLine(Enumerable.SequenceEqual(Helpers.ConvertStringToBoolArray(codeString), decoded) ? $"{Thread.CurrentThread.Name} - Wiadomości są takie same!" : $"{Thread.CurrentThread.Name} - Wiadomości są różne!");

                    }
                    else
                    {
                        Console.WriteLine($"! {Thread.CurrentThread.Name} - Brak wiadomości do pobrania");
                    }

                });

                watek.Name = "Serwer" + i;
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
