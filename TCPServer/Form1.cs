using SONB;
using SuperSimpleTcp;
using System.Text;

namespace TCPServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpServer server;
        static readonly Random rnd = new();


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            server = new SimpleTcpServer(txtIP.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort}: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
            });
        }

        private void Events_ClientDisconnected(object? sender, ConnectionEventArgs e)
        {

            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} roz³¹czono.{Environment.NewLine}";
                lstClientIP.Items.Remove(e.IpPort);
            });
        }

        private void Events_ClientConnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} po³¹czono.{Environment.NewLine}";
                lstClientIP.Items.Add(e.IpPort);
            });
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            txtInfo.Text += $"Serwer wystartowa³...{Environment.NewLine}";
            btnStart.Enabled = false;
            btnSend.Enabled = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (server.IsListening) 
            { 
                if(lstClientIP.SelectedItems != null)
                {
                    txtInfo.Text += $"Serwer - wiadomoœæ do zakodowania: {SONB.Helpers.codeString}{Environment.NewLine}";
                    var encoded = SONB.Helpers.GetEncodedCodeToSend(SONB.Helpers.codeString);
                    server.Send(lstClientIP.SelectedItem.ToString(), Helpers.boolArrayToPrettyString(encoded));
                    txtInfo.Text += $"Sewer - zakodowana wiadomoœæ: {Helpers.boolArrayToPrettyString(encoded)}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}