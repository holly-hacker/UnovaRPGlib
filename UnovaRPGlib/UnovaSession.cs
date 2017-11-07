using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using UnovaRPGlib.Utils;

namespace UnovaRPGlib
{
    public class UnovaSession
    {
        private CookieAwareWebClient _web = new CookieAwareWebClient();

        private string CSRFToken => _web.Cookies.GetCookies(new Uri(_web.LastPage))?["unovarpg"].Value;

        public UnovaSession()
        { }

        public bool Login(string username, string password)
        {
            //TODO: clear cookies

            //initialize cookies
            //_web.DownloadString(Urls.UrlBase);
            _web.DownloadString(Urls.UrlLogin);

            string token = CSRFToken;

            //submit login request
            byte[] resp = _web.UploadValues(Urls.UrlLoginAction, new NameValueCollection {
                {"unovarpg", CSRFToken},
                {"username", username},
                {"password", password}  //TODO: check if characters have to be escaped
            });
            string respS = Encoding.UTF8.GetString(resp);


            return !respS.Contains("validateLogin");
        }

        public void Heal()
        {
            long dong = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            byte[] resp = _web.UploadValues(Urls.UrlPokemonCenter, new NameValueCollection {
                {"xjxfun", "recoverMyPokemon"},
                {"xjxr", dong.ToString()}
            });
            string respS = Encoding.UTF8.GetString(resp);
            Console.WriteLine(respS);
        }
    }
}
