using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace TCP.IP.Communication.Client
{
    public partial class TcpClient
    {
        private bool AcceptCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return _settings.AcceptInvalidCertificates;
        }
    }
}
