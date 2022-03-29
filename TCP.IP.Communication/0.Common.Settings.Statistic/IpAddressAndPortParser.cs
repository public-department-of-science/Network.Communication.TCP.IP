using System;

namespace TCP.Common
{
    internal static class IpAddressAndPortParser
    {
        internal static void ParseIpPort(string ipPort, out string ip, out int port)
        {
            if (string.IsNullOrWhiteSpace(ipPort))
            {
                throw new ArgumentNullException(nameof(ipPort));
            }

            ip = string.Empty;
            port = -1;

            int addressSeparatorIndex = ipPort.LastIndexOf(':');
            if (addressSeparatorIndex != -1)
            {
                ip = ipPort.Substring(0, addressSeparatorIndex);
                var isPortParseOk = int.TryParse(ipPort.Substring(addressSeparatorIndex + 1), out port);
            }
        }
    }
}
