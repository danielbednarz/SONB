namespace SONB
{
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
