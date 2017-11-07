using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnovaRPGlib
{
    public class UnovaZone
    {
        private static readonly Regex RegexName = new Regex(@"blueStrip"">\s+([^\n]+)", RegexOptions.Compiled);
        private static readonly Regex RegexMaps = new Regex(@"map\.php\?map=(?<mapid>\d+)&amp;zone=(?<zoneid>\d+)" + 
                                                            @"[^h]+<h2>(?<name>.+)<\/h2>\s+<p>(?<desc>.+)\n\s+<br>" + 
                                                            @"\s+(?<players>\d+)", RegexOptions.Compiled);

        public string Name { get; private set; }
        public IReadOnlyList<UnovaMap> Maps => _maps;

        private readonly List<UnovaMap> _maps = new List<UnovaMap>();

        private UnovaZone() { }

        internal static UnovaZone FromHtml(string html)
        {
            var uz = new UnovaZone();
            
            //get name
            uz.Name = RegexName.Match(html).Groups[1].Value;

            //get all maps
            foreach (Match m in RegexMaps.Matches(html)) {
                uz._maps.Add(new UnovaMap {
                    MapId = int.Parse(m.Groups["mapid"].Value),
                    ZoneId = int.Parse(m.Groups["zoneid"].Value),
                    Name = m.Groups["name"].Value,
                    Description = m.Groups["desc"].Value,
                    Players = int.Parse(m.Groups["players"].Value)
                });
            }

            return uz;
        }
    }
}
