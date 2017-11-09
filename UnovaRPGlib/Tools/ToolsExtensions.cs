using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnovaRPGlib.Tools
{
    public static class ToolsExtensions
    {
        /// <summary>
        /// This method will attempt to get as much moves as possible by iterating through pokemon ID's.
        /// </summary>
        /// <param name="sess"></param>
        public static Dictionary<int, string> GetMoves(this UnovaSession sess, long startId, int count = 100)
        {
            var r = new Regex(@"#(?<id>\d+) - <strong>(?<name>[^<]+)");
            var d = new Dictionary<int, string>();

            for (int i = 0; i < count; i++) {
                Console.WriteLine("Request " + (i + 1));
                string str = sess.Web.XajaxString(Urls.UrlSetupTeam, "launchMoveTutor", startId - i, "Mr. McDickface", "001");

                foreach (Match m in r.Matches(str))
                    d[int.Parse(m.Groups["id"].Value)] = m.Groups["name"].Value;
            }

            return d;
        }
    }
}
