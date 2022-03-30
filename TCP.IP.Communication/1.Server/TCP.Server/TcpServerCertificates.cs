using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        private bool AcceptCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // return true; // Allow untrusted certificates.
            return _settings.AcceptInvalidCertificates;
        }
    }
}
