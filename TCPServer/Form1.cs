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
                txtInfo.Text += $"Klient ({Encoding.UTF8.GetString(e.Data)}) prosi o ponown¹ wiadomoœæ.{Environment.NewLine}";
                if (e.IpPort != null)
                {
                    txtInfo.Text += $" - ({e.IpPort}) wiadomoœæ do zakodowania: {SONB.Helpers.codeString}{Environment.NewLine}";
                    var encoded = SONB.Helpers.GetEncodedCodeToSend(SONB.Helpers.codeString);
                    server.Send(e.IpPort, Helpers.ConvertBoolArrayToString(encoded));
                    txtInfo.Text += $" - ({e.IpPort}) zakodowana wiadomoœæ: {Helpers.ConvertBoolArrayToString(encoded)}{Environment.NewLine}";
                }
            });
        }

        private void Events_ClientDisconnected(object? sender, ConnectionEventArgs e)
        {

            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $" ### {e.IpPort} roz³¹czono ###{Environment.NewLine}";
                lstClientIP.Items.Remove(e.IpPort);
            });
        }

        private void Events_ClientConnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $" ### {e.IpPort} po³¹czono ###{Environment.NewLine}";
                lstClientIP.Items.Add(e.IpPort + " - poprawna #1");
            });
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            txtInfo.Text += $" --- Serwer wystartowa³ ---{Environment.NewLine}";
            btnStart.Enabled = false;
            btnSend.Enabled = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (server.IsListening)
            {
                if (lstClientIP.SelectedItem != null)
                {
                    foreach (var item in lstClientIP.SelectedItems)
                    {
                        string ipPort = "";
                        string message = " ";
                        int index = item.ToString().IndexOf("-");
                        int problemTypeChoice = int.Parse(item.ToString().Substring(item.ToString().Length - 1));
                        if (index >= 0)
                            ipPort = item.ToString().Substring(0, index - 1);

                        txtInfo.Text += $" - ({ipPort}) wiadomoœæ do zakodowania: {Helpers.codeString}{Environment.NewLine}";
                        var encoded = Helpers.GetEncodedCodeToSend(Helpers.codeString);

                        if (problemTypeChoice == (int)ExceptionType.NoException)
                        {
                            message = Helpers.ConvertBoolArrayToString(encoded);
                        }
                        if (problemTypeChoice == (int)ExceptionType.OneBit)
                        {
                            message = ExceptionFactory.CreateOneBitErrorMessage(txtInfo, ipPort, encoded);
                        }
                        if (problemTypeChoice == (int)ExceptionType.TwoBit)
                        {
                            message = ExceptionFactory.CreateTwoBitErrorMessage(txtInfo, ipPort, encoded);
                        }
                        if (problemTypeChoice == (int)ExceptionType.EmptyMessage)
                        {
                            message = " ";
                        }
                        if (problemTypeChoice == (int)ExceptionType.NotHammingMessage)
                        {
                            message = Helpers.GetRandomErrorMessage();
                            txtInfo.Text += $" - ({ipPort}) wiadomoœæ, która nie jest z kodem Hamminga: {message}{Environment.NewLine}";
                        }

                        server.Send(ipPort, message);
                        txtInfo.Text += $" - ({ipPort}) zostanie wys³ana wiadomoœæ: {message}{Environment.NewLine}";
                    }
                }
                else
                {
                    showMessage();
                }
            }
        }

        private void btnSendAll_Click(object sender, EventArgs e)
        {
            if (server.IsListening)
            {
                if (lstClientIP.Items != null)
                {
                    foreach (var item in lstClientIP.Items)
                    {
                        string ipPort = "";
                        string message = " ";
                        int index = item.ToString().IndexOf("-");
                        int problemTypeChoice = int.Parse(item.ToString().Substring(item.ToString().Length - 1));
                        if (index >= 0)
                            ipPort = item.ToString().Substring(0, index - 1);

                        txtInfo.Text += $" - ({ipPort}) wiadomoœæ do zakodowania: {Helpers.codeString}{Environment.NewLine}";
                        var encoded = Helpers.GetEncodedCodeToSend(Helpers.codeString);

                        if (problemTypeChoice == (int)ExceptionType.NoException)
                        {
                            message = Helpers.ConvertBoolArrayToString(encoded);
                        }
                        if (problemTypeChoice == (int)ExceptionType.OneBit)
                        {
                            message = ExceptionFactory.CreateOneBitErrorMessage(txtInfo, ipPort, encoded);
                        }
                        if (problemTypeChoice == (int)ExceptionType.TwoBit)
                        {
                            message = ExceptionFactory.CreateTwoBitErrorMessage(txtInfo, ipPort, encoded);
                        }
                        if (problemTypeChoice == (int)ExceptionType.EmptyMessage)
                        {
                            message = " ";
                        }
                        if (problemTypeChoice == (int)ExceptionType.NotHammingMessage)
                        {
                            message = Helpers.GetRandomErrorMessage();
                            txtInfo.Text += $" - ({ipPort}) wiadomoœæ, która nie jest z kodem Hamminga: {message}{Environment.NewLine}";
                        }

                        server.Send(ipPort, message);
                        txtInfo.Text += $" - ({ipPort}) zostanie wys³ana wiadomoœæ: {message}{Environment.NewLine}";
                    }
                }
            }
        }

        private void showMessage()
        {
            string message = "W list box musi byæ zaznaczony klient!";
            MessageBox.Show(message);
        }

        private void btnCorrect_Click(object sender, EventArgs e)
        {
            if (lstClientIP.SelectedItem != null)
            {
                string text = "";
                int index = lstClientIP.SelectedItem.ToString().IndexOf("-");
                if (index >= 0)
                    text = lstClientIP.Text.Substring(0, index - 1);
                else
                    return;
                lstClientIP.Items.Remove(lstClientIP.SelectedItem.ToString());
                lstClientIP.Items.Add(text + " - poprawna #1");

            }
            else
            {
                showMessage();
            }
        }

        private void btnError1_Click(object sender, EventArgs e)
        {
            if (lstClientIP.SelectedItem != null)
            {
                string text = "";
                int index = lstClientIP.SelectedItem.ToString().IndexOf("-");
                if (index >= 0)
                    text = lstClientIP.Text.Substring(0, index - 1);
                else
                    return;
                lstClientIP.Items.Remove(lstClientIP.SelectedItem.ToString());
                lstClientIP.Items.Add(text + " - b³¹d 1 bit #2");

            }
            else
            {
                showMessage();
            }
        }

        private void btnError2_Click(object sender, EventArgs e)
        {
            if (lstClientIP.SelectedItem != null)
            {
                string text = "";
                int index = lstClientIP.SelectedItem.ToString().IndexOf("-");
                if (index >= 0)
                    text = lstClientIP.Text.Substring(0, index - 1);
                else
                    return;
                lstClientIP.Items.Remove(lstClientIP.SelectedItem.ToString());
                lstClientIP.Items.Add(text + " - b³¹d 2 bit #3");

            }
            else
            {
                showMessage();
            }
        }

        private void btnEmpty_Click(object sender, EventArgs e)
        {
            if (lstClientIP.SelectedItem != null)
            {
                string text = "";
                int index = lstClientIP.SelectedItem.ToString().IndexOf("-");
                if (index >= 0)
                    text = lstClientIP.Text.Substring(0, index - 1);
                else
                    return;
                lstClientIP.Items.Remove(lstClientIP.SelectedItem.ToString());
                lstClientIP.Items.Add(text + " - pusta #4");

            }
            else
            {
                showMessage();
            }
        }

        private void btnNotHamming_Click(object sender, EventArgs e)
        {
            if (lstClientIP.SelectedItem != null)
            {
                string text = "";
                int index = lstClientIP.SelectedItem.ToString().IndexOf("-");
                if (index >= 0)
                    text = lstClientIP.Text.Substring(0, index - 1);
                else
                    return;
                lstClientIP.Items.Remove(lstClientIP.SelectedItem.ToString());
                lstClientIP.Items.Add(text + " - nie kod Hamminga #5");

            }
            else
            {
                showMessage();
            }
        }

    }
}