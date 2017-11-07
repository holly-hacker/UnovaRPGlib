using System.Net;

namespace UnovaRPGlib.Utils
{
    internal class CookieAwareWebClient : WebClient
    {
        public CookieContainer Cookies = new CookieContainer();
        public string LastPage;

        protected override WebRequest GetWebRequest(System.Uri address)
        {
            WebRequest req = base.GetWebRequest(address);
            if (req is HttpWebRequest request) {
                request.CookieContainer = Cookies;
                if (LastPage != null)
                    request.Referer = LastPage;
            }
            LastPage = address.ToString();
            req.Timeout = 1000;
            return req;
        }
    }
}
