using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UnovaRPGlib
{
    internal static class Extensions
    {
        public static string Xajax(this WebClient wc, string url, string function)
        {
            long time = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            byte[] resp = wc.UploadValues(url, new NameValueCollection {
                {"xjxfun", function},
                {"xjxr", time.ToString()}
            });

            return Encoding.UTF8.GetString(resp);
        }
    }
}
