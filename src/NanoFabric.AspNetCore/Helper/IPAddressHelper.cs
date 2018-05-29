using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using NanoFabric.Core;
using System.Linq;

namespace NanoFabric.AspNetCore.Helper
{
    public class IPAddressHelper
    {
        public static string GetRequestIP(HttpContext context, bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = GetHeaderValueAs<string>(context, "X-Forwarded-For").SplitCsv().FirstOrDefault();

            if (ip.IsNullOrWhitespace() && context?.Connection?.RemoteIpAddress != null)
                ip = context.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>(context, "REMOTE_ADDR");

            if (ip.IsNullOrWhitespace())
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        public static T GetHeaderValueAs<T>(HttpContext context, string headerName)
        {
            StringValues values;

            if (context.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrEmpty(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }
    }
}
