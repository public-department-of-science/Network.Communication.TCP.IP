
namespace TCP.IP.Client.Server
{
    partial class Client
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
            this.lblServer = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbServer = new System.Windows.Forms.TextBox();
            this.txtbMessage = new System.Windows.Forms.TextBox();
            this.txtbInfo = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.connectionStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.connectionStatusStrip = new System.Windows.Forms.StatusStrip();
            this.connectionStatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.connectionStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(66, 30);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(50, 20);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 363);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "label2";
            // 
            // txtbServer
            // 
            this.txtbServer.Location = new System.Drawing.Point(146, 30);
            this.txtbServer.Name = "txtbServer";
            this.txtbServer.Size = new System.Drawing.Size(606, 27);
            this.txtbServer.TabIndex = 2;
            this.txtbServer.Text = "127.0.0.1:9000";
            // 
            // txtbMessage
            // 
            this.txtbMessage.Location = new System.Drawing.Point(146, 363);
            this.txtbMessage.Name = "txtbMessage";
            this.txtbMessage.Size = new System.Drawing.Size(606, 27);
            this.txtbMessage.TabIndex = 3;
            // 
            // txtbInfo
            // 
            this.txtbInfo.Location = new System.Drawing.Point(146, 76);
            this.txtbInfo.Multiline = true;
            this.txtbInfo.Name = "txtbInfo";
            this.txtbInfo.ReadOnly = true;
            this.txtbInfo.Size = new System.Drawing.Size(606, 258);
            this.txtbInfo.TabIndex = 4;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(146, 413);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(125, 56);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(473, 413);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(125, 56);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(627, 413);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(125, 56);
            this.btnDisconnect.TabIndex = 7;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // connectionStatusTimer
            // 
            this.connectionStatusTimer.Enabled = true;
            this.connectionStatusTimer.Tick += new System.EventHandler(this.connectionStatusTimer_Tick);
            // 
            // connectionStatusStrip
            // 
            this.connectionStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.connectionStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionStatusLbl});
            this.connectionStatusStrip.Location = new System.Drawing.Point(0, 480);
            this.connectionStatusStrip.Name = "connectionStatusStrip";
            this.connectionStatusStrip.Size = new System.Drawing.Size(812, 26);
            this.connectionStatusStrip.TabIndex = 8;
            this.connectionStatusStrip.Text = "statusStrip1";
            // 
            // connectionStatusLbl
            // 
            this.connectionStatusLbl.Name = "connectionStatusLbl";
            this.connectionStatusLbl.Size = new System.Drawing.Size(151, 20);
            this.connectionStatusLbl.Text = "toolStripStatusLabel1";
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 506);
            this.Controls.Add(this.connectionStatusStrip);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtbInfo);
            this.Controls.Add(this.txtbMessage);
            this.Controls.Add(this.txtbServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblServer);
            this.MinimizeBox = false;
            this.Name = "Client";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TCP.Client";
            this.Load += new System.EventHandler(this.Client_Load);
            this.connectionStatusStrip.ResumeLayout(false);
            this.connectionStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtbServer;
        private System.Windows.Forms.TextBox txtbMessage;
        private System.Windows.Forms.TextBox txtbInfo;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Timer connectionStatusTimer;
        private System.Windows.Forms.StatusStrip connectionStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel connectionStatusLbl;
    }
}

