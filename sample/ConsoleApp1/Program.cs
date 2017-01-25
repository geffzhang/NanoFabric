using System;
using System.Net;
using System.Linq;
using System.Net.Sockets;

class Program
{
    static void Main(string[] args)
    {
        bool ipv4 = true;
        string hostName = Dns.GetHostName();
        Console.WriteLine(hostName);
        var hostEntry = Dns.GetHostEntryAsync(Dns.GetHostName()).Result;
        IPAddress ipaddress = null;
        if (ipv4)
        {
            ipaddress = (from ip in hostEntry.AddressList
                         where
(!IPAddress.IsLoopback(ip) && ip.AddressFamily == AddressFamily.InterNetwork)
                         select ip)
                         .FirstOrDefault();
        }
        else
        {
            ipaddress = (from ip in hostEntry.AddressList
                         where
(!IPAddress.IsLoopback(ip) && ip.AddressFamily == AddressFamily.InterNetworkV6)
                         select ip)
                         .FirstOrDefault();
        }
        if (ipaddress != null)
        {
            Console.WriteLine( ipaddress.ToString());
        }
    }
}