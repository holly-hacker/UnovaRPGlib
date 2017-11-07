using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnovaRPGlib
{
    public class UnovaMap
    {
        public int MapId;
        public int ZoneId;
        public string Name;
        public string Description;

        /// <summary> Amount of players in this map. </summary>
        public int Players;
    }
}
