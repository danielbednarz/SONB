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
            btnSend.Enabled = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect();
                btnSend.Enabled = true;
                btnConnect.Enabled = false;
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
                txtInfo.Text += $"Roz³¹czono z serwerem. {Environment.NewLine}";
            });
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                var message = Encoding.UTF8.GetString(e.Data);
                txtInfo.Text += $"{e.IpPort} Otrzymano zakodowan¹ wiadomoœæ: {message}{Environment.NewLine}";
                var encoded = Helpers.prettyStringToBoolArray(message);
                int calculatedErrorPosition = SONB.Hamming.ErrorSyndrome(encoded);
                if (calculatedErrorPosition != 0)
                {
                    txtInfo.Text += $"! {Thread.CurrentThread.Name} - B³¹d na pozycji: {calculatedErrorPosition}{Environment.NewLine}";
                    encoded[calculatedErrorPosition - 1] = !encoded[calculatedErrorPosition - 1];
                    Console.WriteLine($"! {Thread.CurrentThread.Name} - B³¹d naprawiony");
                }

                var decoded = SONB.Hamming.Decode( encoded );
            txtInfo.Text += $"{Thread.CurrentThread.Name} - Wiadomoœæ po odkodowaniu: {Helpers.boolArrayToPrettyString(decoded)}{Environment.NewLine}";

            txtInfo.Text += Enumerable.SequenceEqual(Helpers.prettyStringToBoolArray(SONB.Helpers.codeString), decoded) ? $"{Thread.CurrentThread.Name} - Wiadomoœci s¹ takie same!" : $"{Thread.CurrentThread.Name} - Wiadomoœci s¹ ró¿ne!{Environment.NewLine}";
            });
        }

        private void Events_Connected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"Po³¹czono z serwerem. {Environment.NewLine}";
            });
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if(client.IsConnected)
            { 
                if(!string.IsNullOrEmpty(txtMessage.Text))
                {
                    client.Send(txtMessage.Text);
                    txtInfo.Text += $"Moja wiadomoœæ: {txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }
    }
}