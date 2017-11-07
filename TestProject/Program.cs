using System;
using System.Diagnostics;
using System.IO;
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

            var z1 = sess.GetZoneById(1);
            var m1 = z1.Maps[0];    //route 101
            var team = sess.GetBattleTeam();

            var b = sess.StartWildBattle(313, 9, 1, 20, 12);

            Console.WriteLine("Press space to battle");
            b.Auth();
            while (Console.ReadKey().Key == ConsoleKey.Spacebar) {
                b.Attack(427);
            }
        }
    }
}
