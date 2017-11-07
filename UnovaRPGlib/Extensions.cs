using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using UnovaRPGlib.Xajax;

namespace UnovaRPGlib
{
    internal static class Extensions
    {
        public static IEnumerable<XajaxCommand> Xajax(this WebClient wc, string url, string function, params object[] args) 
            => XajaxParser.Parse(XajaxString(wc, url, function, args));
        
        public static string XajaxString(this WebClient wc, string url, string function, params object[] args)
        {
            long time = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var nvc = new NameValueCollection {
                {"xjxfun", function},
                {"xjxr", time.ToString()}
            };

            foreach (XajaxValue arg in args.Select(a => new XajaxValue(a))) {
                nvc.Add("xjxargs[]", arg.ToString());
            }

            byte[] resp = wc.UploadValues(url, nvc);
            return Encoding.UTF8.GetString(resp);
        }
    }
}
