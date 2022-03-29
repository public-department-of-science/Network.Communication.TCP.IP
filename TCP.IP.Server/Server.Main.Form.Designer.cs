
namespace TCP.IP.Server
{
    partial class Server
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtBox_serverInfo = new System.Windows.Forms.TextBox();
            this.btnStartServerListening = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtbInfo = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.ctxMenuClientsList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.disconnectClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstBoxClients = new System.Windows.Forms.CheckedListBox();
            this.btnAbortNewConnections = new System.Windows.Forms.Button();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.ctxMenuClientsList.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBox_serverInfo
            // 
            this.txtBox_serverInfo.Location = new System.Drawing.Point(126, 31);
            this.txtBox_serverInfo.Name = "txtBox_serverInfo";
            this.txtBox_serverInfo.Size = new System.Drawing.Size(462, 27);
            this.txtBox_serverInfo.TabIndex = 6;
            this.txtBox_serverInfo.Text = "127.0.0.1:9000";
            // 
            // btnStartServerListening
            // 
            this.btnStartServerListening.Location = new System.Drawing.Point(664, 361);
            this.btnStartServerListening.Name = "btnStartServerListening";
            this.btnStartServerListening.Size = new System.Drawing.Size(185, 46);
            this.btnStartServerListening.TabIndex = 0;
            this.btnStartServerListening.Text = "Start listening";
            this.btnStartServerListening.UseVisualStyleBackColor = true;
            this.btnStartServerListening.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(464, 361);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(124, 46);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 314);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Message";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(664, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Client IPs";
            // 
            // txtbInfo
            // 
            this.txtbInfo.Location = new System.Drawing.Point(126, 86);
            this.txtbInfo.Multiline = true;
            this.txtbInfo.Name = "txtbInfo";
            this.txtbInfo.ReadOnly = true;
            this.txtbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtbInfo.Size = new System.Drawing.Size(462, 207);
            this.txtbInfo.TabIndex = 7;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(126, 314);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(462, 27);
            this.txtMessage.TabIndex = 8;
            // 
            // ctxMenuClientsList
            // 
            this.ctxMenuClientsList.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ctxMenuClientsList.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxMenuClientsList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disconnectClientToolStripMenuItem});
            this.ctxMenuClientsList.Name = "contextMenuStrip1";
            this.ctxMenuClientsList.Size = new System.Drawing.Size(152, 28);
            this.ctxMenuClientsList.Text = "Menu";
            // 
            // disconnectClientToolStripMenuItem
            // 
            this.disconnectClientToolStripMenuItem.Name = "disconnectClientToolStripMenuItem";
            this.disconnectClientToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.disconnectClientToolStripMenuItem.Text = "Disconnect";
            this.disconnectClientToolStripMenuItem.Click += new System.EventHandler(this.disconnectClientToolStripMenuItem_Click);
            // 
            // lstBoxClients
            // 
            this.lstBoxClients.ContextMenuStrip = this.ctxMenuClientsList;
            this.lstBoxClients.FormattingEnabled = true;
            this.lstBoxClients.Location = new System.Drawing.Point(664, 66);
            this.lstBoxClients.MultiColumn = true;
            this.lstBoxClients.Name = "lstBoxClients";
            this.lstBoxClients.Size = new System.Drawing.Size(185, 136);
            this.lstBoxClients.TabIndex = 9;
            // 
            // btnAbortNewConnections
            // 
            this.btnAbortNewConnections.Location = new System.Drawing.Point(664, 295);
            this.btnAbortNewConnections.Name = "btnAbortNewConnections";
            this.btnAbortNewConnections.Size = new System.Drawing.Size(185, 46);
            this.btnAbortNewConnections.TabIndex = 10;
            this.btnAbortNewConnections.Text = "Abort new connections";
            this.btnAbortNewConnections.UseVisualStyleBackColor = true;
            this.btnAbortNewConnections.Click += new System.EventHandler(this.btnAbortNewConnections_Click);
            // 
            // btnStopServer
            // 
            this.btnStopServer.Location = new System.Drawing.Point(664, 228);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(185, 46);
            this.btnStopServer.TabIndex = 11;
            this.btnStopServer.Text = "Stop server";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 452);
            this.Controls.Add(this.btnStopServer);
            this.Controls.Add(this.btnAbortNewConnections);
            this.Controls.Add(this.lstBoxClients);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.txtbInfo);
            this.Controls.Add(this.txtBox_serverInfo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnStartServerListening);
            this.MaximizeBox = false;
            this.Name = "Server";
            this.Text = "TCP.Server";
            this.Load += new System.EventHandler(this.Server_Load);
            this.ctxMenuClientsList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartServerListening;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBox_serverInfo;
        private System.Windows.Forms.TextBox txtbInfo;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.ContextMenuStrip ctxMenuClientsList;
        private System.Windows.Forms.ToolStripMenuItem disconnectClientToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox lstBoxClients;
        private System.Windows.Forms.Button btnAbortNewConnections;
        private System.Windows.Forms.Button btnStopServer;
    }
}

