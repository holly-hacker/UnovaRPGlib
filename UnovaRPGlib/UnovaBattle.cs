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

        public int? PokeId { get; private set; }
        public int? Level { get; private set; }
        private int? _mapId;

        private string _type;
        private long? _tid;
        private string _token;
        private string _runToken;
        private UnovaSession _sess;
        
        internal static UnovaBattle FromHtml(UnovaSession sess, string html, int? pokeId = null, int? level = null, int? mapId = null, long? tid = null)
        {
            var ub = new UnovaBattle();

            ub._sess = sess;
            ub.PokeId = pokeId;
            ub.Level = level;
            ub._mapId = mapId;

            if (tid != null)
                ub._tid = tid;

            //get tokens
            string fParams = RegexToken.Match(html).Groups[1].Value;
            ub._type = fParams.Split(',')[2].Trim('\'', ' ');
            ub._token = fParams.Split(',').Last().Trim('\'', ' ');
            ub._runToken = RegexRunToken.Match(html).Groups[1].Value;

            return ub;
        }

        public void Auth() => SendCommand();

        public string Attack(Move move) => Attack((int)move);
        public string Attack(int attackId)
        {
            //TODO: return results properly
            var cmd = SendCommand(moveId: attackId.ToString()).First(a => a.Attributes["id"] == "battleEvents");

            return cmd.Value.Text.Replace("<br />", "\n");
        }

        public void Run()
        {
            _sess.Web.DownloadString(Urls.UrlMap + $"?map={_mapId}&p={_runToken}");
        }

        private XajaxCommand[] SendCommand(string banchNumber = "0", string moveId = "", string type = null,
            string idCapturedPokemon = "", string hpLeft = "", string useItem = "undefined", 
            string keepBanchPokemon = "undefined")
        {
            type = type ?? _type;

            string url = Urls.UrlBattle + "?type=" + type;

            if (_tid != null)
                url += "&tid=" + _tid;

            return _sess.Web.Xajax(url , "setBattle", 
                banchNumber, moveId, type, idCapturedPokemon, PokeId, Level, _mapId, hpLeft, useItem, keepBanchPokemon, _token)
                .ToArray();
        }
    }
}
