using System.Net;
using TCP.Client;
using TCP.Settings;
using TCP.Statistic;

namespace TCP.IP.Communication.Client
{
    public partial class TcpClient
    {
        /// <summary>
        /// Indicates whether or not the client is connected to the server.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            private set
            {
                _isConnected = value;
            }
        }

        /// <summary>
        /// Client IPEndPoint if connected.
        /// </summary>
        public IPEndPoint LocalEndpoint
        {
            get
            {
                if (_client != null && _isConnected)
                {
                    return (IPEndPoint)_client.Client.LocalEndPoint;
                }

                return null;
            }
        }

        /// <summary>
        /// Tcp client settings.
        /// </summary>
        public TcpClientSettings Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                if (value == null)
                {
                    _settings = new TcpClientSettings();
                }
                else
                {
                    _settings = value;
                }
            }
        }

        /// <summary>
        /// Tcp client events.
        /// </summary>
        public TcpClientEventsHandler Events
        {
            get
            {
                return _events;
            }
            set
            {
                if (value == null)
                {
                    _events = new TcpClientEventsHandler();
                }
                else
                {
                    _events = value;
                }
            }
        }

        /// <summary>
        /// Tcp statistics.
        /// </summary>
        public TcpStatistic Statistics
        {
            get
            {
                return _statistics;
            }
        }

        /// <summary>
        /// Tcp keepalive settings.
        /// </summary>
        public TcpKeepAliveSettings Keepalive
        {
            get
            {
                return _keepalive;
            }
            set
            {
                if (value == null)
                {
                    _keepalive = new TcpKeepAliveSettings();
                }
                else
                {
                    _keepalive = value;
                }
            }
        }

        /// <summary>
        /// The IP:port of the server to which this client is mapped.
        /// </summary>
        public string ServerIpPort
        {
            get
            {
                return $"{_serverIp}:{_serverPort}";
            }
        }
    }
}
