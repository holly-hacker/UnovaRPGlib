using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnovaRPGlib.Xajax;

namespace UnovaRPGlib
{
    public class UnovaBattle
    {
        private static readonly Regex RegexToken = new Regex(@"setBattle\((.+')\)");

        private int _pokeId, _level, _mapId;

        private string _token;
        private UnovaSession _sess;

        internal static UnovaBattle FromHtml(UnovaSession sess, string html, int pokeId, int level, int mapId)
        {
            var ub = new UnovaBattle();

            ub._sess = sess;
            ub._pokeId = pokeId;
            ub._level = level;
            ub._mapId = mapId;

            //get token
            string fParams = RegexToken.Match(html).Groups[1].Value;
            ub._token = fParams.Split(',').Last().Trim('\'', ' ');

            return ub;
        }

        public void Auth()
        {
            SendCommand(idPokemon: _pokeId.ToString(), level: _level.ToString(), idMap: _mapId.ToString(), token: _token);
        }

        public void Attack(int attackId)
        {
            //TODO: return results
            SendCommand(form: attackId.ToString(), idPokemon: _pokeId.ToString(), level: _level.ToString(), idMap: _mapId.ToString());

        }

        private void SendCommand(string banchNumber = "0", string form = "", string type = "wild",
            string idCapturedPokemon = "", string idPokemon = "", string level = "", string idMap = "",
            string hpLeft = "", string useItem = "undefined", string keepBanchPokemon = "undefined", 
            string token = "")
        {
            //banchNumber = 0
            //form = moveid (eg. 427)
            //type = wild
            //idCapturedPokemon = ''
            //idPokemon = other pokemon id
            //level = level
            //idMap = map id
            //useItem = itemid, null OR keepBanchPokemon, undefined
            //attackPP, undefined
            //token, empty or token

            var cmd = _sess.Web.Xajax(Urls.UrlBattleWild, "setBattle", 
                banchNumber, form, type, idCapturedPokemon, idPokemon, level, idMap, hpLeft, useItem, keepBanchPokemon, token)
                .ToArray().First(a => a.Attributes["id"] == "battleEvents");

            Console.WriteLine(cmd.Attributes["id"]);
            Console.WriteLine(cmd.Value.Text.Replace("<br />", "\n"));
            Console.WriteLine();
            //TODO
        }
    }
}
