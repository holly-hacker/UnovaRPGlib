using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnovaRPGlib;

namespace TestProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] creds = File.ReadAllLines("creds.txt");

            var sess = new UnovaSession();

            Console.WriteLine("Logging in...");
            if (!sess.Login(creds[0], creds[1])) {
                Console.WriteLine("Bad login :(");
                Debugger.Break();
                return;
            }

            Console.WriteLine($"\n\nLogged in, welcome {creds[0]}.");

            //sess.Heal();

            var z1 = sess.GetZoneById(1);
            var m1 = z1.Maps[0];    //route 101
        }
    }
}
