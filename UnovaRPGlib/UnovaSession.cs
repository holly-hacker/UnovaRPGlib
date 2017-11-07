using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using UnovaRPGlib.Utils;

namespace UnovaRPGlib
{
    public class UnovaSession
    {
        private readonly CookieAwareWebClient _web = new CookieAwareWebClient();

        private string CSRFToken => _web.Cookies.GetCookies(new Uri(_web.LastPage))["unovarpg"]?.Value;

        private UnovaSession() { }

        public static UnovaSession Create(string username, string password)
        {
            var us = new UnovaSession();

            //initialize cookies
            //_web.DownloadString(Urls.UrlBase);
            us._web.DownloadString(Urls.UrlLogin);

            //submit login request
            byte[] resp = us._web.UploadValues(Urls.UrlLoginAction, new NameValueCollection {
                {"unovarpg", us.CSRFToken},
                {"username", username},
                {"password", password}
            });
            string respS = Encoding.UTF8.GetString(resp);

            return !respS.Contains("validateLogin") ? us : null;
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
            
            if (html.Contains("leader-list"))
                throw new NotImplementedException("Elite 4 and Battle Frontier are not supported yet");

            //extract from html
            return UnovaZone.FromHtml(html);
        }

        public UnovaPokemon[] GetBattleTeam()
        {
            //TODO: unsafe, add error checking
            var cmd1 = _web.Xajax(Urls.UrlMap + "?map=1&zone=1", "getBattleTeam", "", true).ToArray();
            var cmd = cmd1.First(a => a.Command == "as");

            string html = cmd.Value.Text;
            
            //extract from html
            return UnovaPokemon.FromHtml(html);
        }
    }
}
