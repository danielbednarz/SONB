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
                    txtInfo.Text += $"Serwer(klient-{e.IpPort}) - wiadomoœæ do zakodowania: {SONB.Helpers.codeString}{Environment.NewLine}";
                    var encoded = SONB.Helpers.GetEncodedCodeToSend(SONB.Helpers.codeString);
                    server.Send(e.IpPort, Helpers.boolArrayToPrettyString(encoded));
                    txtInfo.Text += $"Sewer(klient-{e.IpPort}) - zakodowana wiadomoœæ: {Helpers.boolArrayToPrettyString(encoded)}{Environment.NewLine}";
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
                lstClientIP.Items.Add(e.IpPort+" - poprawna#1");
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
            //if (server.IsListening) 
            //{
            //    if(lstClientIP.SelectedItems != null)
            //    {
            //        foreach (var item in lstClientIP.SelectedItems)
            //        {
            //            txtInfo.Text += $"Serwer - wiadomoœæ do zakodowania: {SONB.Helpers.codeString}{Environment.NewLine}";
            //            var encoded = SONB.Helpers.GetEncodedCodeToSend(SONB.Helpers.codeString);
            //            server.Send(item.ToString(), Helpers.boolArrayToPrettyString(encoded));
            //            txtInfo.Text += $"Sewer - zakodowana wiadomoœæ: {Helpers.boolArrayToPrettyString(encoded)}{Environment.NewLine}";
            //            txtMessage.Text = string.Empty;
            //        }
            //    }
            //}
            if (server.IsListening)
            {
                if (lstClientIP.SelectedItems != null)
                {
                    foreach (var item in lstClientIP.SelectedItems)
                    {

                        String ipPort = "";
                        String message = " ";
                        int index = item.ToString().IndexOf("-");
                        String it = item.ToString().Substring(item.ToString().Length - 2);
                        if (index >= 0)
                            ipPort = item.ToString().Substring(0, index - 1);

                        txtInfo.Text += $"Serwer(klient-{ipPort}) - wiadomoœæ do zakodowania: {SONB.Helpers.codeString}{Environment.NewLine}";
                        var encoded = SONB.Helpers.GetEncodedCodeToSend(SONB.Helpers.codeString);


                        if (it.Equals("#1"))
                        {
                            message = Helpers.boolArrayToPrettyString(encoded);
                        }
                        if (it.Equals("#2"))
                        {
                            var errorPosition = rnd.Next(3, 22);
                            while (Helpers.isPowerOfTwo(errorPosition))
                            {
                                errorPosition = rnd.Next(3, 22);
                            }
                            var code = Helpers.prettyStringToBoolArray(SONB.Helpers.codeString);
                            var encodedError = Hamming.Encode(code);
                            txtInfo.Text += $"Sewer(klient-{ipPort}) - zakodowana wiadomoœæ: {Helpers.boolArrayToPrettyString(encoded)}{Environment.NewLine}";
                            SONB.Hamming.MixinSingleError(encodedError, errorPosition);
                            txtInfo.Text += ($"Serwer(klient-{ipPort})  - Wiadomoœæ z b³êdem:   {Helpers.boolArrayToPrettyString(encodedError)} ({errorPosition}){Environment.NewLine}");
                            message = Helpers.boolArrayToPrettyString(encodedError);
                        }
                        if (it.Equals("#3"))
                        {
                            var errorPosition = rnd.Next(3, 22);
                            var errorPosition2 = rnd.Next(3, 22);
                            while (Helpers.isPowerOfTwo(errorPosition))
                            {
                                errorPosition = rnd.Next(3, 22);
                            }
                            while (errorPosition == errorPosition2 || (Helpers.isPowerOfTwo(errorPosition2)))
                            {
                                errorPosition2 = rnd.Next(3, 22);
                            }
                            var code = Helpers.prettyStringToBoolArray(SONB.Helpers.codeString);
                            var encodedError = Hamming.Encode(code);
                            txtInfo.Text += $"Sewer(klient-{ipPort}) - zakodowana wiadomoœæ: {Helpers.boolArrayToPrettyString(encoded)}{Environment.NewLine}";
                            SONB.Hamming.MixinDoubleError(encodedError, errorPosition, errorPosition2);
                            txtInfo.Text += ($"Serwer(klient-{ipPort})  - Wiadomoœæ z b³êdemna 2 bitach:   {Helpers.boolArrayToPrettyString(encodedError)} ({errorPosition})({errorPosition2}){Environment.NewLine}");
                            message = Helpers.boolArrayToPrettyString(encodedError);
                        }
                        if (it.Equals("#5"))
                        {
                            message = SONB.Helpers.codeString;
                        }
                        if (it.Equals("#6"))
                        {
                            message = " ";
                        }

                        server.Send(ipPort, message);
                        txtInfo.Text += $"Sewer(klient-{ipPort}) -  wiadomoœæ: {message}{Environment.NewLine}";
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

                        String ipPort = "";
                        String message=" ";
                        int index = item.ToString().IndexOf("-");
                        String it = item.ToString().Substring(item.ToString().Length - 2);
                        if (index >= 0)
                            ipPort = item.ToString().Substring(0, index-1);
                        
                        txtInfo.Text += $"Serwer(klient-{ipPort}) - wiadomoœæ do zakodowania: {SONB.Helpers.codeString}{Environment.NewLine}";
                        var encoded = SONB.Helpers.GetEncodedCodeToSend(SONB.Helpers.codeString);
                        
                        
                        if (it.Equals("#1"))
                        {
                            message = Helpers.boolArrayToPrettyString(encoded);
                        }
                        if (it.Equals("#2"))
                        {
                            var errorPosition = rnd.Next(3, 22);
                            while (Helpers.isPowerOfTwo(errorPosition))
                            {
                                errorPosition = rnd.Next(3, 22);
                            }
                            var code = Helpers.prettyStringToBoolArray(SONB.Helpers.codeString);
                            var encodedError = Hamming.Encode(code);
                            txtInfo.Text += $"Sewer(klient-{ipPort}) - zakodowana wiadomoœæ: {Helpers.boolArrayToPrettyString(encoded)}{Environment.NewLine}";
                            SONB.Hamming.MixinSingleError(encodedError, errorPosition);
                            txtInfo.Text += ($"Serwer(klient-{ipPort})  - Wiadomoœæ z b³êdem:   {Helpers.boolArrayToPrettyString(encodedError)} ({errorPosition}){Environment.NewLine}");
                            message = Helpers.boolArrayToPrettyString(encodedError);
                        }
                        if (it.Equals("#3"))
                        {
                            var errorPosition = rnd.Next(3, 22);
                            var errorPosition2 = rnd.Next(3, 22);
                            while (Helpers.isPowerOfTwo(errorPosition))
                            {
                                errorPosition = rnd.Next(3, 22);
                            }
                            while (errorPosition == errorPosition2 || (Helpers.isPowerOfTwo(errorPosition2)))
                            {
                                errorPosition2 = rnd.Next(3, 22);
                            }
                            var code = Helpers.prettyStringToBoolArray(SONB.Helpers.codeString);
                            var encodedError = Hamming.Encode(code);
                            txtInfo.Text += $"Sewer(klient-{ipPort}) - zakodowana wiadomoœæ: {Helpers.boolArrayToPrettyString(encoded)}{Environment.NewLine}";
                            SONB.Hamming.MixinDoubleError(encodedError, errorPosition, errorPosition2);
                            txtInfo.Text += ($"Serwer(klient-{ipPort})  - Wiadomoœæ z b³êdemna 2 bitach:   {Helpers.boolArrayToPrettyString(encodedError)} ({errorPosition})({errorPosition2}){Environment.NewLine}");
                            message = Helpers.boolArrayToPrettyString(encodedError);
                        }
                        if (it.Equals("#4"))
                        {
                            message = null;
                        }
                        if (it.Equals("#5"))
                        {
                            message = SONB.Helpers.codeString;
                        }
                        if (it.Equals("#6"))
                        {
                            message = " ";
                        }

                        server.Send(ipPort, message);
                        txtInfo.Text += $"Sewer(klient-{ipPort}) -  wiadomoœæ: {message}{Environment.NewLine}";
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
                    text = lstClientIP.Text.Substring(0, index-1);
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
                    text = lstClientIP.Text.Substring(0, index-1);
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
                    text = lstClientIP.Text.Substring(0, index-1);
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
                    text = lstClientIP.Text.Substring(0, index-1);
                else
                    return;
                lstClientIP.Items.Remove(lstClientIP.SelectedItem.ToString());
                lstClientIP.Items.Add(text + " - null#4");

            }
        }

        private void btnNotHamming_Click(object sender, EventArgs e)
        {
            {
                String text = "";
                int index = lstClientIP.SelectedItem.ToString().IndexOf("-");
                if (index >= 0)
                    text = lstClientIP.Text.Substring(0, index-1);
                else
                    return;
                lstClientIP.Items.Remove(lstClientIP.SelectedItem.ToString());
                lstClientIP.Items.Add(text + " - nieHamming#5");

            }
        }

        private void btnEmpty_Click(object sender, EventArgs e)
        {
            {
                String text = "";
                int index = lstClientIP.SelectedItem.ToString().IndexOf("-");
                if (index >= 0)
                    text = lstClientIP.Text.Substring(0, index-1);
                else
                    return;
                lstClientIP.Items.Remove(lstClientIP.SelectedItem.ToString());
                lstClientIP.Items.Add(text + " - pusta#6");

            }
        }
    }
}