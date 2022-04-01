using System;

namespace TCP.Statistic
{
    /// <summary>
    /// TCP statistics.
    /// </summary>
    public class TcpStatistic
    {
        private DateTime utcTime = DateTime.UtcNow;
        private long receivedBytes = 0;
        private long sentBytes = 0;

        /// <summary>
        /// The time at which the client or server was started.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return utcTime;
            }
        }

        /// <summary>
        /// The amount of time which the client or server has been up.
        /// </summary>
        public TimeSpan UpTime
        {
            get
            {
                return DateTime.UtcNow - utcTime;
            }
        }

        /// <summary>
        /// The number of bytes received.
        /// </summary>
        public long ReceivedBytes
        {
            get
            {
                return receivedBytes;
            }
            internal set
            {
                receivedBytes = value;
            }
        }

        /// <summary>
        /// The number of bytes sent.
        /// </summary>
        public long SentBytes
        {
            get
            {
                return sentBytes;
            }
            internal set
            {
                sentBytes = value;
            }
        }

        /// <summary>
        /// Return human-readable version of the object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ret =
                "--- Statistics ---" + Environment.NewLine +
                "    Started        : " + utcTime.ToString() + Environment.NewLine +
                "    Uptime         : " + UpTime.ToString() + Environment.NewLine +
                "    Received bytes : " + ReceivedBytes + Environment.NewLine +
                "    Sent bytes     : " + SentBytes + Environment.NewLine;
            return ret;
        }

        public void ResetDataCounter()
        {
            receivedBytes = 0;
            sentBytes = 0;
        }
    }
}
