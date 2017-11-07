using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using UnovaRPGlib.Utils;

namespace UnovaRPGlib
{
    public class UnovaSession
    {
        internal readonly CookieAwareWebClient Web = new CookieAwareWebClient();

        private string CSRFToken => Web.Cookies.GetCookies(new Uri(Web.LastPage))["unovarpg"]?.Value;

        private UnovaSession() { }

        public static UnovaSession Create(string username, string password)
        {
            var us = new UnovaSession();

            //initialize cookies
            //_web.DownloadString(Urls.UrlBase);
            us.Web.DownloadString(Urls.UrlLogin);

            //submit login request
            byte[] resp = us.Web.UploadValues(Urls.UrlLoginAction, new NameValueCollection {
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
            Web.XajaxString(Urls.UrlPokemonCenter, "recoverMyPokemon");
        }

        public UnovaZone GetZoneById(int id)
        {
            //TODO: unsafe, add error checking
            var cmd = Web.Xajax(Urls.UrlMap, "loadZone", 1).First(a => a.Command == "as");
            
            string html = cmd.Value.Text;
            
            if (html.Contains("leader-list"))
                throw new NotImplementedException("Elite 4 and Battle Frontier are not supported yet");

            //extract from html
            return UnovaZone.FromHtml(html);
        }

        public UnovaPokemon[] GetBattleTeam()
        {
            //TODO: unsafe, add error checking
            var cmd1 = Web.Xajax(Urls.UrlMap + "?map=1&zone=1", "getBattleTeam", "", true).ToArray();
            var cmd = cmd1.First(a => a.Command == "as");

            string html = cmd.Value.Text;
            
            //extract from html
            return UnovaPokemon.FromHtml(html);
        }

        public UnovaBattle StartWildBattle(int pokeId, int level, int mapId, int x, int y)
        {
            string shiny = "";

            byte[] resp = Web.UploadValues(Urls.UrlBattleWild, new NameValueCollection {
                {"token_pokemon", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{pokeId}|{shiny}|{level}|{mapId}"))},
                {"id_pokemon", pokeId.ToString()},
                {"level", level.ToString()},
                {"id_map", mapId.ToString()},
                {"x", x.ToString()},
                {"y", y.ToString()}
            });

            //TODO: sanity check, this can go wrong easily

            return UnovaBattle.FromHtml(this, Encoding.UTF8.GetString(resp), pokeId, level, mapId);
        }
    }
}
