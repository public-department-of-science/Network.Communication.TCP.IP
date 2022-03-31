using System;
using System.Text;
using System.Windows.Forms;
using TCP.EventArguments;
using TCP.IP.Communication.Client;

namespace TCP.IP.Client.Server
{
    public partial class Client : Form
    {
        TcpClient tcpClient;

        public Client()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tcpClient.IsConnected)
            {
                if (!string.IsNullOrWhiteSpace(txtbMessage.Text))
                {
                    tcpClient.Send(txtbMessage.Text);
                    txtbInfo.Text += $"Me: {txtbMessage.Text} {Environment.NewLine}";
                    txtbMessage.Text = string.Empty;
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                tcpClient.Connect();
                btnSend.Enabled = true;
                btnDisconnect.Enabled = true;
                btnConnect.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient.IsConnected)
                {
                    tcpClient.Disconnect();
                    btnSend.Enabled = false;
                    btnDisconnect.Enabled = false;
                    btnConnect.Enabled = true;
                }
                else
                {
                    btnConnect.Enabled = true;
                    btnDisconnect.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Client_Load(object sender, EventArgs e)
        {
            tcpClient = new TcpClient(txtbServer.Text);
            tcpClient.Events.Connected += Events_Connected;
            tcpClient.Events.DataReceived += Events_DataReceived;
            tcpClient.Events.Disconnected += Events_Disconnected;
            btnSend.Enabled = false;
            btnDisconnect.Enabled = false;
        }

        private void Events_Disconnected(object sender, DisconnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtbInfo.Text += $"Client {tcpClient.LocalEndpoint.ToString()} was disconnected from the server {txtbServer.Text}. {Environment.NewLine}";
            });
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtbInfo.Text += $"Server: {Encoding.UTF8.GetString(e.Data)} {Environment.NewLine}";
            });
        }

        private void Events_Connected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtbInfo.Text += $"Client connected to the {txtbServer.Text} server. {Environment.NewLine}";
            });
        }

        private void connectionStatusTimer_Tick(object sender, EventArgs e)
        {
            if (tcpClient.IsConnected)
            {
                btnDisconnect.Enabled = true;
                btnConnect.Enabled = false;
                btnSend.Enabled = true;

                connectionStatusLbl.Text = "Connected";
            }
            else
            {
                btnDisconnect.Enabled = false;
                btnConnect.Enabled = true;
                btnSend.Enabled = false;
                
                connectionStatusLbl.Text = "Disconnected";
            }
        }
    }
}
