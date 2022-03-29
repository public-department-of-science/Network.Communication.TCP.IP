using System;
using System.Text;
using System.Windows.Forms;
using TCP.EventArguments;
using TCP.Server;

namespace TCP.IP.Server
{
    public partial class Server : Form
    {
        TcpServer tcpServer;

        public Server()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            tcpServer.Start();
            txtbInfo.Text += $"Server {txtBox_serverInfo.Text} Started {Environment.NewLine}";

            btnStopServer.Enabled = true;
            btnStartServerListening.Enabled = false;
            btnSend.Enabled = true;
            btnAbortNewConnections.Enabled = true;
        }
        private void btnAbortNewConnections_Click(object sender, EventArgs e)
        {
            if (tcpServer.IsListening)
            {
                tcpServer.AbortNewConnections();
                txtbInfo.Text += $"Server {txtBox_serverInfo.Text} stopped {Environment.NewLine}";

                btnAbortNewConnections.Enabled = false;
                btnStopServer.Enabled = false;
                btnStartServerListening.Enabled = true;
                btnSend.Enabled = false;
            }
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            if (tcpServer.IsListening)
            {
                tcpServer.AbortNewConnections();
                tcpServer.DisconnectAllClients();
                tcpServer.Dispose();

                btnStopServer.Enabled = false;
                btnAbortNewConnections.Enabled = false;
                btnSend.Enabled = false;
                btnStartServerListening.Enabled = true;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tcpServer.IsListening)
            {
                if (!string.IsNullOrWhiteSpace(txtMessage.Text) && lstBoxClients.SelectedItem != null)
                {
                    tcpServer.Send(lstBoxClients.SelectedItem.ToString(), txtMessage.Text);
                    txtbInfo.Text += $"Server: {txtMessage.Text} {Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
                if (lstBoxClients.SelectedItem == null)
                {
                    MessageBox.Show("Client wasn't selected to send message!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void Server_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            btnAbortNewConnections.Enabled = false;
            btnStopServer.Enabled = false;

            tcpServer = new TcpServer(txtBox_serverInfo.Text);

            tcpServer.Events.ClientConnected += Events_ClientConnected;
            tcpServer.Events.ClientDisconnected += Events_ClientDisconnected;
            tcpServer.Events.DataReceived += Events_DataReceived;
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtbInfo.Text += $"{e.IpPort}: {Encoding.UTF8.GetString(e.Data)} {Environment.NewLine}";
            });
        }

        private void Events_ClientDisconnected(object sender, DisconnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtbInfo.Text += $"{e.IpPort} disconnected. Reason: {e.Reason}. {Environment.NewLine}";
                lstBoxClients.Items.Remove(e.IpPort);
            });
        }

        private void Events_ClientConnected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtbInfo.Text += $"{e.IpPort} connected. Reason: {e.Reason}. {Environment.NewLine}";
                lstBoxClients.Items.Add(e.IpPort);
            });
        }

        private void disconnectClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                var selectedClientsIPs = lstBoxClients.CheckedItems;
                if (selectedClientsIPs.Count == 0)
                {
                    return;
                }
                else
                {
                    do
                    {
                        tcpServer.DisconnectClient(selectedClientsIPs[0].ToString());
                        txtbInfo.Text += $"{selectedClientsIPs[0].ToString()} disconnected. {Environment.NewLine}";
                        lstBoxClients.Items.Remove(selectedClientsIPs[0]);
                    }
                    while (selectedClientsIPs.Count != 0);
                }
            });
        }
    }
}
