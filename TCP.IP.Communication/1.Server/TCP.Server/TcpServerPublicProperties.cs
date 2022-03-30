using System;
using TCP.Server;
using TCP.Server.EventsHandler;
using TCP.Server.Settings;
using TCP.Settings;
using TCP.Statistic;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        /// <summary>
        /// Indicates if the server is listening for connections.
        /// </summary>
        public bool IsListening
        {
            get
            {
                return _isListening;
            }
        }

        /// <summary>
        /// Tcp server settings.
        /// </summary>
        public TcpServerSettings Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                if (value == null)
                {
                    _settings = new TcpServerSettings();
                }
                else
                {
                    _settings = value;
                }
            }
        }

        /// <summary>
        /// Tcp server events.
        /// </summary>
        public TcpServerEventsHandler Events
        {
            get
            {
                return _events;
            }
            set
            {
                if (value == null)
                {
                    _events = new TcpServerEventsHandler();
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
        /// Determines if a client is connected by its IP:port.
        /// </summary>
        /// <param name="ipPort">The client IP:port string.</param>
        /// <returns>True if connected.</returns>
        public bool IsConnected(string ipPort)
        {
            if (string.IsNullOrWhiteSpace(ipPort))
            {
                throw new ArgumentNullException(nameof(ipPort));
            }

            return (_clients.TryGetValue(ipPort, out _));
        }
    }
}
