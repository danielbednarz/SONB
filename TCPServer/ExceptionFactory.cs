using SONB;
using System.Windows.Forms;

namespace TCPServer
{
    public class ExceptionFactory
    {
        private static Random rnd = new();

        public static string CreateOneBitErrorMessage(TextBox txtInfo, string ipPort, bool[] encodedMessage)
        {
            var errorPosition = rnd.Next(3, 22);
            while (Helpers.IsPowerOfTwo(errorPosition))
            {
                errorPosition = rnd.Next(3, 22);
            }
            var code = Helpers.ConvertStringToBoolArray(Helpers.codeString);
            var encodedError = Hamming.Encode(code);

            txtInfo.Text += $" - ({ipPort})  zakodowana wiadomość: {Helpers.ConvertBoolArrayToString(encodedMessage)}{Environment.NewLine}";
            Hamming.MixinSingleError(encodedError, errorPosition);
            txtInfo.Text += ($" - ({ipPort}) wiadomość z błędem:   {Helpers.ConvertBoolArrayToString(encodedError)} ({errorPosition}){Environment.NewLine}");
            
            string message = Helpers.ConvertBoolArrayToString(encodedError);

            return message;
        }

        public static string CreateTwoBitErrorMessage(TextBox txtInfo, string ipPort, bool[] encodedMessage)
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
            var code = Helpers.ConvertStringToBoolArray(Helpers.codeString);
            var encodedError = Hamming.Encode(code);
            txtInfo.Text += $" - ({ipPort})  zakodowana wiadomość: {Helpers.ConvertBoolArrayToString(encodedMessage)}{Environment.NewLine}";
            Hamming.MixinDoubleError(encodedError, errorPosition, errorPosition2);
            txtInfo.Text += ($" - ({ipPort})  wiadomość z błędem na 2 bitach: {Helpers.ConvertBoolArrayToString(encodedError)} ({errorPosition})({errorPosition2}){Environment.NewLine}");
            
            string message = Helpers.ConvertBoolArrayToString(encodedError);

            return message;
        }
    }
}
