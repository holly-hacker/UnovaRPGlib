using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnovaRPGlib
{
    public class UnovaPokemon
    {
        private static readonly Regex RegexName = new Regex(@"strong>(?<name>[^<]+)<", RegexOptions.Compiled);
        private static readonly Regex RegexStatus = new Regex(@"Unique #: (?<uid>\d+).+\sExperience: (?<exp>\d+).+\s" + 
                                                              @"Level: (?<lvl>\d+).+\sHP: (?<hpCur>\d+)\/(?<hpMax>\d+).+\s" + 
                                                              @"Status: (?<status>[^<]+)", RegexOptions.Compiled);
        private static readonly Regex RegexMoves = new Regex(@"Att\. \d: (?<name>[^:]+) - PP Left: (?<ppCur>\d+)\/(?<ppMax>\d+)");

        public string Name { get; private set; }
        public long UniqueId { get; private set; }
        public int Experience { get; private set; }
        public int Level { get; private set; }
        public int HpCurrent { get; private set; }
        public int HpMax { get; private set; }
        public string Status { get; private set; }

        public Dictionary<string, int> MovesPP { get; } = new Dictionary<string, int>();

        internal static UnovaPokemon[] FromHtml(string html)
        {
            //split all by closing li
            string[] split = html.Split(new []{ "</li>" }, StringSplitOptions.RemoveEmptyEntries);

            UnovaPokemon[] arr = new UnovaPokemon[split.Length - 1];

            //skip last string, which isn't needed
            for (int i = 0; i < split.Length - 1; i++) {
                string str = split[i];

                var up = new UnovaPokemon();

                //get name
                up.Name = RegexName.Match(str).Groups[1].Value;

                //get status
                Match m = RegexStatus.Match(str);
                up.UniqueId = long.Parse(m.Groups["uid"].Value);
                up.Experience = int.Parse(m.Groups["exp"].Value);
                up.Level = int.Parse(m.Groups["lvl"].Value);
                up.HpCurrent = int.Parse(m.Groups["hpCur"].Value);
                up.HpMax = int.Parse(m.Groups["hpMax"].Value);
                up.Status = m.Groups["status"].Value;

                //get moves
                MatchCollection matches = RegexMoves.Matches(str);
                for (int j = 0; j < matches.Count; j++) {
                    string name = matches[j].Groups["name"].Value;
                    int pp = int.Parse(matches[j].Groups["ppCur"].Value);
                    up.MovesPP[name] = pp;
                }

                arr[i] = up;
            }

            return arr;
        }
    }
}
