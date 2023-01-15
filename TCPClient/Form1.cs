using SONB;
using SuperSimpleTcp;
using System.Drawing.Imaging;
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Message",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Events_Disconnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $" --- Roz³¹czono z serwerem. ---{Environment.NewLine}";
            });
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                var port = client.LocalEndpoint.Port;

                if (e.Data == null)
                {
                    txtInfo.Text += $" ! B³¹d, wartoœæ nie mo¿e byæ nullem!{Environment.NewLine}";
                    txtInfo.Text += $" - Proszê serwer o ponowne nades³anie wiadomoœci.{Environment.NewLine}";
                    client.Send(port.ToString());
                    return;
                }

                if (e.Data.Count <= 1)
                {
                    txtInfo.Text += $" ! B³¹d, wiadomoœæ jest pusta!{Environment.NewLine}";
                    txtInfo.Text += $" - Proszê serwer o ponowne nades³anie wiadomoœci.{Environment.NewLine}";
                    client.Send(port.ToString());
                    return;
                }

                if (e.Data.Count < 21)
                {
                    txtInfo.Text += $" ! B³¹d, wiadomoœæ nie jest kodem Hamminga!{Environment.NewLine}";
                    txtInfo.Text += $" - Proszê serwer o ponowne nades³anie wiadomoœci.{Environment.NewLine}";
                    client.Send(port.ToString());
                    return;
                }


                
                var message = Encoding.UTF8.GetString(e.Data);
                txtInfo.Text += $"{e.IpPort} Otrzymano zakodowan¹ wiadomoœæ: {message}{Environment.NewLine}";
                var encoded = Helpers.prettyStringToBoolArray(message);
                int calculatedErrorPosition = SONB.Hamming.ErrorSyndrome(encoded);
                if (calculatedErrorPosition != 0)
                {
                    txtInfo.Text += $" ! B³¹d na pozycji: {calculatedErrorPosition}{Environment.NewLine}";
                    encoded[calculatedErrorPosition - 1] = !encoded[calculatedErrorPosition - 1];
                    txtInfo.Text += $" - B³¹d naprawiony";
                }

                var decoded = SONB.Hamming.Decode( encoded );
                txtInfo.Text += $" - Wiadomoœæ po odkodowaniu: {Helpers.boolArrayToPrettyString(decoded)}{Environment.NewLine}";
                if(!Enumerable.SequenceEqual(Helpers.prettyStringToBoolArray(SONB.Helpers.codeString), decoded))
                {
                    client.Send(port.ToString());
                }
                txtInfo.Text += Enumerable.SequenceEqual(Helpers.prettyStringToBoolArray(SONB.Helpers.codeString), decoded) ? $"{Thread.CurrentThread.Name} - Wiadomoœci s¹ takie same!{Environment.NewLine}" : $"{Thread.CurrentThread.Name} ! B³¹d wiadomoœci s¹ ró¿ne, proszê serwer o nades³anie poprawnej wiadomoœci!{Environment.NewLine}";
            });
        }

        private void Events_Connected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $" --- Po³¹czono z serwerem ---{Environment.NewLine}";
            });
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}