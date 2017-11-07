using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnovaRPGlib;

namespace TestProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] creds = File.ReadAllLines("creds.txt");

            Console.WriteLine("Logging in...");

            var sess = UnovaSession.Create(creds[0], creds[1]);
            if (sess == null) {
                Console.WriteLine("Bad login :(");
                Debugger.Break();
                return;
            }

            Console.WriteLine($"\n\nLogged in, welcome {creds[0]}.");

            //sess.Heal();

            //var z1 = sess.GetZoneById(1);
            //var m1 = z1.Maps[0];    //route 101
            //var team = sess.GetBattleTeam();

            while (true) {
                var b = sess.StartWildBattle(165, 10, 1, 20, 12);

                Console.WriteLine("Press space to battle");
                b.Auth();

                string str;
                while (!(str = b.Attack(427)).Contains("span"))
                {
                    Console.Write(".");
                }
                Console.WriteLine();
                Console.WriteLine(str);

                //b.Run();
                //Console.WriteLine("Ran");

                sess.Heal();
                Console.WriteLine("Healed!");

                //we have to (re)load the team before battle, or we'll get an error
                sess.GetBattleTeam();
                Console.WriteLine("Reloaded team");

                //sess.BuildMap(1);
                //Console.WriteLine("rebuilt map");

                //Thread.Sleep(2500);
            }
        }
    }
}
