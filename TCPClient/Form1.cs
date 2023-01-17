using SONB;
using SuperSimpleTcp;
using System.Text;

namespace TCPClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpClient client;

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new(txtIP.Text);
            client.Events.Connected += Events_Connected;
            client.Events.DataReceived += Events_DataReceived;
            client.Events.Disconnected += Events_Disconnected;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect();
                btnConnect.Enabled = false;
                textBoxIpClient.Text = client.LocalEndpoint.Port.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Events_Disconnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $" --- Roz��czono z serwerem. ---{Environment.NewLine}";
            });
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                var port = client.LocalEndpoint.Port;

                if (e.Data == null)
                {
                    txtInfo.Text += $" ! B��d, warto�� nie mo�e by� nullem!{Environment.NewLine}";
                    txtInfo.Text += $" - Prosz� serwer o ponowne nades�anie wiadomo�ci.{Environment.NewLine}";
                    client.Send(port.ToString());
                    return;
                }

                if (e.Data.Count <= 1)
                {
                    txtInfo.Text += $" ! B��d, wiadomo�� jest pusta!{Environment.NewLine}";
                    txtInfo.Text += $" - Prosz� serwer o ponowne nades�anie wiadomo�ci.{Environment.NewLine}";
                    client.Send(port.ToString());
                    return;
                }

                if (e.Data.Count < 21)
                {
                    txtInfo.Text += $" ! B��d, wiadomo�� nie jest kodem Hamminga!{Environment.NewLine}";
                    txtInfo.Text += $" - Prosz� serwer o ponowne nades�anie wiadomo�ci.{Environment.NewLine}";
                    client.Send(port.ToString());
                    return;
                }

                var message = Encoding.UTF8.GetString(e.Data);
                txtInfo.Text += $"{e.IpPort} Otrzymano zakodowan� wiadomo��: {message}{Environment.NewLine}";

                var encoded = Helpers.ConvertStringToBoolArray(message);
                int calculatedErrorPosition = Hamming.ErrorSyndrome(encoded);
                if (calculatedErrorPosition != 0)
                {
                    txtInfo.Text += $" ! B��d na pozycji: {calculatedErrorPosition}{Environment.NewLine}";
                    encoded[calculatedErrorPosition - 1] = !encoded[calculatedErrorPosition - 1];
                    txtInfo.Text += $" - B��d naprawiony";
                }

                var decoded = Hamming.Decode(encoded);
                txtInfo.Text += $" - Wiadomo�� po odkodowaniu: {Helpers.ConvertBoolArrayToString(decoded)}{Environment.NewLine}";
                if (!Enumerable.SequenceEqual(Helpers.ConvertStringToBoolArray(Helpers.codeString), decoded))
                {
                    client.Send(port.ToString());
                }
                txtInfo.Text += Enumerable.SequenceEqual(Helpers.ConvertStringToBoolArray(Helpers.codeString), decoded) ? $"{Thread.CurrentThread.Name} - Wiadomo�ci s� takie same!{Environment.NewLine}" : $"{Thread.CurrentThread.Name} ! B��d wiadomo�ci s� r�ne, prosz� serwer o nades�anie poprawnej wiadomo�ci!{Environment.NewLine}";
            });
        }

        private void Events_Connected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $" --- Po��czono z serwerem ---{Environment.NewLine}";
            });
        }

    }
}