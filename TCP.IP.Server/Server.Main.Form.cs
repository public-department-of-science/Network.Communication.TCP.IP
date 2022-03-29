﻿using System;
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
            btnStart.Enabled = false;
            btnSend.Enabled = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tcpServer.IsListening)
            {
                if (!string.IsNullOrWhiteSpace(txtMessage.Text) && lstClients.SelectedItem != null)
                {
                    tcpServer.Send(lstClients.SelectedItem.ToString(), txtMessage.Text);
                    txtbInfo.Text += $"Server: {txtMessage.Text} {Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }

        private void Server_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
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
                lstClients.Items.Remove(e.IpPort);
            });
        }

        private void Events_ClientConnected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtbInfo.Text += $"{e.IpPort} connected. Reason: {e.Reason}. {Environment.NewLine}";
                lstClients.Items.Add(e.IpPort);
            });
        }
    }
}
