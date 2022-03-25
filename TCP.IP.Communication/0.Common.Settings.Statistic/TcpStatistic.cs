using System;

namespace TCP
{
    /// <summary>
    /// TCP statistics.
    /// </summary>
    public class TcpStatistic
    {
        /// <summary>
        /// The time at which the client or server was started.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
        }

        /// <summary>
        /// The amount of time which the client or server has been up.
        /// </summary>
        public TimeSpan UpTime
        {
            get
            {
                return DateTime.Now.ToUniversalTime() - _startTime;
            }
        }

        /// <summary>
        /// The number of bytes received.
        /// </summary>
        public long ReceivedBytes
        {
            get
            {
                return _receivedBytes;
            }
            internal set
            {
                _receivedBytes = value;
            }
        }
         
        /// <summary>
        /// The number of bytes sent.
        /// </summary>
        public long SentBytes
        {
            get
            {
                return _sentBytes;
            }
            internal set
            {
                _sentBytes = value;
            }
        }

        private DateTime _startTime = DateTime.Now.ToUniversalTime();
        private long _receivedBytes = 0; 
        private long _sentBytes = 0; 

        /// <summary>
        /// Initialize the statistics object.
        /// </summary>
        public TcpStatistic()
        {
        }

        /// <summary>
        /// Return human-readable version of the object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ret =
                "--- Statistics ---" + Environment.NewLine +
                "    Started        : " + _startTime.ToString() + Environment.NewLine +
                "    Uptime         : " + UpTime.ToString() + Environment.NewLine +
                "    Received bytes : " + ReceivedBytes + Environment.NewLine +
                "    Sent bytes     : " + SentBytes + Environment.NewLine;
            return ret;
        }

        /// <summary>
        /// Reset statistics other than StartTime and UpTime.
        /// </summary>
        public void Reset()
        {
            _receivedBytes = 0; 
            _sentBytes = 0; 
        }
    }
}
