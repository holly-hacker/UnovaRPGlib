using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
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
            
            var dic = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("xjxfun", function),
                new KeyValuePair<string, string>("xjxr", time.ToString())
            };

            foreach (XajaxValue arg in args.Select(a => new XajaxValue(a))) {
                dic.Add(new KeyValuePair<string, string>("xjxargs[]", arg.ToString()));
            }

            string query = string.Join("&", dic.Select(a => $"{HttpUtility.UrlEncode(a.Key)}={HttpUtility.UrlEncode(a.Value)}"));
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            return wc.UploadString(url, query);
        }
    }
}
