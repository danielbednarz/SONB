using System.Collections.Concurrent;

namespace SONB
{
    public class Helpers
    {
        public static readonly string codeString = "1011001010110010";
        public static string ConvertBoolArrayToString(bool[] arr)
        {
            return string.Join("", arr.Select(x => Convert.ToInt32(x)));
        }

        public static bool[] ConvertStringToBoolArray(string s)
        {
            return s.Select(x => x == '1').ToArray();
        }

        public static bool IsNotPowerOf2(int x)
        {
            return !(x == 1 || x == 2 || x == 4 || x == 8 || x == 16);
        }

        public static int[] GetPositionsForXoring(int length, int currentHammingPosition)
        {
            var positions = new List<int>();
            for (int i = 1; i <= length; i++)
            {
                if ((i & currentHammingPosition) > 0 && IsNotPowerOf2(i))
                    positions.Add(i);

            }
            return positions.ToArray();
        }

        public static bool DoXoringForPosition(bool[] vector, int length, int currentHammingPosition)
        {
            return GetPositionsForXoring(length, currentHammingPosition)
                .Select(x => vector[x - 1])
                .Aggregate((x, y) => x ^ y);
        }

        public static void ClearCollection(BlockingCollection<string> collection)
        {
            while (collection.TryTake(out _)) { }
        }

        public static bool[] GetEncodedCodeToSend(string codeString)
        {
            var code = ConvertStringToBoolArray(codeString);
            var encoded = Hamming.Encode(code);

            Console.WriteLine($"Serwer nadzorujacy - Wiadomość do zakodowania: {ConvertBoolArrayToString(code)}");
            Console.WriteLine($"Serwer nadzorujacy - Zakodowana wiadomość: {ConvertBoolArrayToString(encoded)}\n");

            return encoded;
        }
        public static bool IsPowerOfTwo(int n)
        {
            if (n == 0)
                return false;

            return (int)(Math.Ceiling(
            (Math.Log(n) / Math.Log(2))))
            == (int)(Math.Floor(
            ((Math.Log(n) / Math.Log(2)))));
        }

    }
}
