using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using UnovaRPGlib.Utils;
using UnovaRPGlib.Xajax;

namespace UnovaRPGlib
{
    public class UnovaSession
    {
        private CookieAwareWebClient _web = new CookieAwareWebClient();

        private string CSRFToken => _web.Cookies.GetCookies(new Uri(_web.LastPage))["unovarpg"]?.Value;

        public UnovaSession()
        { }

        public bool Login(string username, string password)
        {
            //TODO: clear cookies
            //TODO: or, make this a static method that returns a UnovaSession

            //initialize cookies
            //_web.DownloadString(Urls.UrlBase);
            _web.DownloadString(Urls.UrlLogin);

            //submit login request
            byte[] resp = _web.UploadValues(Urls.UrlLoginAction, new NameValueCollection {
                {"unovarpg", CSRFToken},
                {"username", username},
                {"password", password}  //TODO: check if characters have to be escaped
            });
            string respS = Encoding.UTF8.GetString(resp);
            
            return !respS.Contains("validateLogin");    //this is a js function that only exists on the login page
        }

        public void Heal()
        {
            //we don't use the response (yet)
            _web.XajaxString(Urls.UrlPokemonCenter, "recoverMyPokemon");
        }

        public UnovaZone GetZoneById(int id)
        {
            //TODO: unsafe, add error checking
            var cmd = _web.Xajax(Urls.UrlMap, "loadZone", 1).First(a => a.Command == "as");
            
            string html = cmd.Value.Text;
            Console.WriteLine(html);

            //extract from html
            return UnovaZone.FromHtml(html);
        }
    }
}
