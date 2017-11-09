using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnovaRPGlib;
using UnovaRPGlib.Tools;

namespace TestProject
{
    internal static partial class Program
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

            TestTrainTrainer(sess);
        }

        private static void TestTrainWild(UnovaSession sess)
        {
            sess.Heal();
            sess.GetBattleTeam();

            while (true)
            {
                try
                {
                    UnovaBattle b = sess.StartWildBattle(Pokemon.Blissey, 40, 33, ShinyType.Shiny);

                    b.Auth();

                    string str;
                    while (!(str = b.Attack(Move.FocusPunch)).Contains("span"))
                        Console.Write(".");
                    Console.WriteLine();
                    Console.WriteLine(str);

                    sess.Heal();
                    sess.GetBattleTeam();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void TestTrainTrainer(UnovaSession sess)
        {
            sess.Heal();
            sess.GetBattleTeam();

            while (true)
            {
                try
                {
                    UnovaBattle b = sess.StartTrainerBattle(90941693);

                    b.Auth();

                    string str;
                    while (!(str = b.Attack(Move.FocusPunch)).Contains("span"))
                        Console.Write(".");
                    Console.WriteLine();
                    Console.WriteLine(str);

                    sess.Heal();
                    sess.GetBattleTeam();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void GetMoves(UnovaSession sess, long startId, int count = 100)
        {
            var d = sess.GetMoves(startId, count);

            var sb = new StringBuilder();
            var sb2 = new StringBuilder();
            foreach (int key in d.Keys.OrderBy(i => i))
            {
                sb.AppendLine(key.ToString("D3") + ": " + d[key]);
                sb2.AppendLine(d[key].Replace(" ", "").Replace("-", "") + " = " + key.ToString("D3") + ",");
            }
            File.WriteAllText("moves.txt", sb.ToString());
            File.WriteAllText("movesenum.txt", sb2.ToString());
        }
    }
}
