using SONB;
using SuperSimpleTcp;
using System;
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
                txtInfo.Text += $"Klient prosi o ponown¹ wiadomoœæ: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
                if (e.IpPort != null)
                {
                    txtInfo.Text += $"Serwer - wiadomoœæ do zakodowania: {SONB.Helpers.codeString}{Environment.NewLine}";
                    var encoded = SONB.Helpers.GetEncodedCodeToSend(SONB.Helpers.codeString);
                    server.Send(e.IpPort, Helpers.boolArrayToPrettyString(encoded));
                    txtInfo.Text += $"Sewer - zakodowana wiadomoœæ: {Helpers.boolArrayToPrettyString(encoded)}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
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
                lstClientIP.Items.Add(e.IpPort+" - eee#1");
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
                    foreach (var item in lstClientIP.SelectedItems)
                    {
                        txtInfo.Text += $"Serwer - wiadomoœæ do zakodowania: {SONB.Helpers.codeString}{Environment.NewLine}";
                        var encoded = SONB.Helpers.GetEncodedCodeToSend(SONB.Helpers.codeString);
                        server.Send(item.ToString(), Helpers.boolArrayToPrettyString(encoded));
                        txtInfo.Text += $"Sewer - zakodowana wiadomoœæ: {Helpers.boolArrayToPrettyString(encoded)}{Environment.NewLine}";
                        txtMessage.Text = string.Empty;
                    }
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnSendAll_Click(object sender, EventArgs e)
        {
            if (server.IsListening)
            {
                if (lstClientIP.Items != null)
                {
                    foreach (var item in lstClientIP.Items)
                    {
                        
                        //String it  = item.ToString().Substring(mystring.Length - 4);
                        txtInfo.Text += $"Serwer - wiadomoœæ do zakodowania: {SONB.Helpers.codeString}{Environment.NewLine}";
                        var encoded = SONB.Helpers.GetEncodedCodeToSend(SONB.Helpers.codeString);
                        server.Send(item.ToString(), Helpers.boolArrayToPrettyString(encoded));
                        txtInfo.Text += $"Sewer - zakodowana wiadomoœæ: {Helpers.boolArrayToPrettyString(encoded)}{Environment.NewLine}";
                        txtMessage.Text = string.Empty;
                    }
                }
            }
        }

        private void btnCorrect_Click(object sender, EventArgs e)
        {
            if (lstClientIP.SelectedItems != null)
            {
                String text ="";
                int index = lstClientIP.SelectedItem.ToString().IndexOf("-");
                if (index >= 0)
                    text = lstClientIP.Text.Substring(0, index);
                else
                    return;
                lstClientIP.Items.Remove(lstClientIP.SelectedItem.ToString());
                lstClientIP.Items.Add(text + " - poprawna#1");
                
            }
        }

        private void btnError1_Click(object sender, EventArgs e)
        {
            if (lstClientIP.SelectedItems != null)
            {
                String text = "";
                int index = lstClientIP.SelectedItem.ToString().IndexOf("-");
                if (index >= 0)
                    text = lstClientIP.Text.Substring(0, index);
                else
                    return;
                lstClientIP.Items.Remove(lstClientIP.SelectedItem.ToString());
                lstClientIP.Items.Add(text + " - blad1bit#2");

            }
        }

        private void btnError2_Click(object sender, EventArgs e)
        {
            if (lstClientIP.SelectedItems != null)
            {
                String text = "";
                int index = lstClientIP.SelectedItem.ToString().IndexOf("-");
                if (index >= 0)
                    text = lstClientIP.Text.Substring(0, index);
                else
                    return;
                lstClientIP.Items.Remove(lstClientIP.SelectedItem.ToString());
                lstClientIP.Items.Add(text + " - blad2bit#3");

            }
        }

        private void btnNull_Click(object sender, EventArgs e)
        {
            if (lstClientIP.SelectedItems != null)
            {
                String text = "";
                int index = lstClientIP.SelectedItem.ToString().IndexOf("-");
                if (index >= 0)
                    text = lstClientIP.Text.Substring(0, index);
                else
                    return;
                lstClientIP.Items.Remove(lstClientIP.SelectedItem.ToString());
                lstClientIP.Items.Add(text + " - pusta#4");

            }
        }
    }
}