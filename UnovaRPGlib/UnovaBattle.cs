using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnovaRPGlib.Xajax;

namespace UnovaRPGlib
{
    public class UnovaBattle
    {
        private static readonly Regex RegexToken = new Regex(@"setBattle\((.+')\)");
        private static readonly Regex RegexRunToken = new Regex(@"map=\d+&amp;p=(?<p>[^=]+=+)");

        public int PokeId { get; private set; }
        public int Level { get; private set; }
        private int _mapId;

        private string _token;
        private string _runToken;
        private UnovaSession _sess;

        internal static UnovaBattle FromHtml(UnovaSession sess, string html, int pokeId, int level, int mapId)
        {
            var ub = new UnovaBattle();

            ub._sess = sess;
            ub.PokeId = pokeId;
            ub.Level = level;
            ub._mapId = mapId;

            //get tokens
            string fParams = RegexToken.Match(html).Groups[1].Value;
            ub._token = fParams.Split(',').Last().Trim('\'', ' ');
            ub._runToken = RegexToken.Match(html).Groups[1].Value;

            return ub;
        }

        public void Auth()
        {
            //TODO: change Attack() parameter to move index (0-based), get moveid[] from this
            SendCommand(idPokemon: PokeId.ToString(), level: Level.ToString(), idMap: _mapId.ToString(), token: _token);
        }

        public string Attack(int attackId)
        {
            //TODO: return results
            var cmd = SendCommand(moveId: attackId.ToString(), idPokemon: PokeId.ToString(), level: Level.ToString(), idMap: _mapId.ToString())
                .First(a => a.Attributes["id"] == "battleEvents");

            return cmd.Value.Text.Replace("<br />", "\n");
        }

        public void Run()
        {
            _sess.Web.DownloadString(Urls.UrlMap + $"?map={_mapId}&p={_runToken}");
        }

        private XajaxCommand[] SendCommand(string banchNumber = "0", string moveId = "", string type = "wild",
            string idCapturedPokemon = "", string idPokemon = "", string level = "", string idMap = "",
            string hpLeft = "", string useItem = "undefined", string keepBanchPokemon = "undefined", 
            string token = "")
        {
            //banchNumber: selected pokemon index?
            //idCapturedPokemon: ?

            return _sess.Web.Xajax(Urls.UrlBattleWild, "setBattle", 
                banchNumber, moveId, type, idCapturedPokemon, idPokemon, level, idMap, hpLeft, useItem, keepBanchPokemon, token)
                .ToArray();
        }
    }
}
