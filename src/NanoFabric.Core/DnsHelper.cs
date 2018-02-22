using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;
using DnsClient;

namespace NanoFabric.Core
{
    public static class DnsHelper
    {
        /// <summary>
        /// 获取本地的IP地址
        /// </summary>
        /// <param name="ipv4">是否ipv4</param>
        /// <returns></returns>
        public static async Task<string> GetIpAddressAsync(bool ipv4 = true)
        {
            var client = new LookupClient();            
            var hostEntry = await client.GetHostEntryAsync(Dns.GetHostName());
            IPAddress ipaddress = null;
            if (ipv4)
            {
                ipaddress = (from ip in hostEntry.AddressList where 
                             (!IPAddress.IsLoopback(ip) && ip.AddressFamily == AddressFamily.InterNetwork)
                             select ip)
                             .FirstOrDefault() ;
            }
            else
            {
                ipaddress = (from ip in hostEntry.AddressList where
                             (!IPAddress.IsLoopback(ip) && ip.AddressFamily == AddressFamily.InterNetworkV6)
                             select ip)
                             .FirstOrDefault();
            }
            if(ipaddress != null)
            {
                return ipaddress.ToString();
            }         

            return string.Empty;
        }
    }
}
