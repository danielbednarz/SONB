namespace HammingAttempt
{
    class Program
    {
        public const bool t = true;
        public const bool f = false;
        public const int startWith = 2;
        static int length;

        static bool[] Encode(bool[] code)
        {
            var encoded = new bool[length];

            int i = startWith, j = 0;
            while (i < length)
            {
                if (i == 3 || i == 7 || i == 15) i++;
                encoded[i] = code[j];

                i++;
                j++;
            }

            encoded[0] = Helpers.doXoringForPosition(encoded, length, 1);
            encoded[1] = Helpers.doXoringForPosition(encoded, length, 2);
            encoded[3] = Helpers.doXoringForPosition(encoded, length, 4);
            if (length > 7)
                encoded[7] = Helpers.doXoringForPosition(encoded, length, 8);
            if (length > 15)
                encoded[15] = Helpers.doXoringForPosition(encoded, length, 16);

            return encoded;
        }

        static bool[] Decode(bool[] encoded)
        {
            var decoded = new bool[16];

            int i = startWith, j = 0;
            while (i < length)
            {
                if (i == 3 || i == 7 || i == 15) i++;
                decoded[j] = encoded[i];

                i++;
                j++;
            }

            return decoded;
        }

        static int ErrorSyndrome(bool[] encoded)
        {
            int syndrome =
                (Convert.ToInt32(Helpers.doXoringForPosition(encoded, length, 1) ^ encoded[0])) +
                (Convert.ToInt32(Helpers.doXoringForPosition(encoded, length, 2) ^ encoded[1]) << 1) +
                (Convert.ToInt32(Helpers.doXoringForPosition(encoded, length, 4) ^ encoded[3]) << 2);
            if (length > 7) syndrome +=
               (Convert.ToInt32(Helpers.doXoringForPosition(encoded, length, 8) ^ encoded[7]) << 3);
            if (length > 15) syndrome +=
               (Convert.ToInt32(Helpers.doXoringForPosition(encoded, length, 16) ^ encoded[15]) << 4);

            return syndrome;
        }

        static void MixinSingleError(bool[] encoded, int pos)
        {
            encoded[pos - 1] = !encoded[pos - 1];
        }

        static void Main(string[] args)
        {
            length = 21;
            int errorPosition = 18;
            string codeString = "1111111111111111";

            var code = Helpers.prettyStringToBoolArray(codeString);
            var encoded = Encode(code);

            Console.WriteLine($"Message to encode: {Helpers.boolArrayToPrettyString(code)}");
            Console.WriteLine($"Encoded message: {Helpers.boolArrayToPrettyString(encoded)}");

            MixinSingleError(encoded, errorPosition);
            Console.WriteLine($"Message with error: {Helpers.boolArrayToPrettyString(encoded)}");

            int calculatedErrorPosition = ErrorSyndrome(encoded);
            Console.WriteLine($"Error position: {calculatedErrorPosition}");
            encoded[calculatedErrorPosition-1] = !encoded[calculatedErrorPosition-1];

            var decoded = Decode(encoded);
            Console.WriteLine($"Message after decoding: {Helpers.boolArrayToPrettyString(decoded)}");

            Console.WriteLine(Enumerable.SequenceEqual(code, decoded) ? "Messages are equal!" : "Messages are not equal!");

            Console.WriteLine();

            Console.ReadLine();
        }
    }

    public class Helpers
    {

        public static string boolArrayToPrettyString(bool[] arr)
        {
            return string.Join("", arr.Select(x => Convert.ToInt32(x)));
        }

        public static bool[] prettyStringToBoolArray(string s)
        {
            return s.Select(x => x == '1').ToArray();
        }

        public static bool notPowerOf2(int x)
        {
            return !(x == 1 || x == 2 || x == 4 || x == 8 || x == 16);
        }

        public static int[] getPositionsForXoring(int length, int currentHammingPosition)
        {
            var positions = new List<int>();
            for (int i = 1; i <= length; i++)
            {
                if ((i & currentHammingPosition) > 0 && notPowerOf2(i))
                    positions.Add(i);

            }
            return positions.ToArray();
        }

        public static bool doXoringForPosition(bool[] vector, int length, int currentHammingPosition)
        {
            return getPositionsForXoring(length, currentHammingPosition)
                .Select(x => vector[x - 1])
                .Aggregate((x, y) => x ^ y);
        }
    }
}