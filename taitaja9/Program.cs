using System;
using System.Collections.Generic;
using System.IO;

namespace taitaja9
{
    internal class Program
    {
        static List<Tulos> tulokset = new List<Tulos>();
        static string tiedosto = "tulokset.txt";

        static void Main(string[] args)
        {
            LataaTulokset();

            bool kaynnissa = true;
            while (kaynnissa)
            {
                NäytäValikko();
                string valinta = Console.ReadLine();

                switch (valinta)
                {
                    case "1": LisaaTulos(); break;
                    case "2": NaytaTulokset(); break;
                    case "3": HaeTuloksia(); break;
                    case "4": kaynnissa = false; break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Virheellinen valinta! Yritä uudelleen.");
                        Console.ResetColor();
                        break;
                }
            }

            TallennaTulokset();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Kiitos ohjelman käytöstä!");
            Console.ResetColor();
        }

        static void NäytäValikko()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- Taitaja 9 Tulostaulukko ---");
            Console.ResetColor();
            Console.WriteLine("1) Lisää tulos");
            Console.WriteLine("2) Näytä kaikki tulokset");
            Console.WriteLine("3) Hae tulos nimen perusteella");
            Console.WriteLine("4) Lopeta");
            Console.Write("Valitse toiminto: ");
        }

        static void LisaaTulos()
        {
            Console.Write("Anna nimi: ");
            string nimi = Console.ReadLine();
            Console.Write("Anna laji: ");
            string laji = Console.ReadLine();
            Console.Write("Anna tulos: ");
            string tulos = Console.ReadLine();

            tulokset.Add(new Tulos(nimi, laji, tulos));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tulos lisätty!");
            Console.ResetColor();
        }

        static void NaytaTulokset()
        {
            if (tulokset.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ei tuloksia näytettäväksi.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n{0,-20} {1,-15} {2,-10}", "Nimi", "Laji", "Tulos");
            Console.WriteLine(new string('-', 50));
            Console.ResetColor();

            foreach (var t in tulokset)
            {
                Console.WriteLine("{0,-20} {1,-15} {2,-10}", t.Nimi, t.Laji, t.TulosArvo);
            }
        }

        static void HaeTuloksia()
        {
            Console.Write("Anna nimi haettavaksi: ");
            string haku = Console.ReadLine().ToLower();

            var loydetyt = tulokset.FindAll(t => t.Nimi.ToLower().Contains(haku));

            if (loydetyt.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n{0,-20} {1,-15} {2,-10}", "Nimi", "Laji", "Tulos");
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();

                foreach (var t in loydetyt)
                {
                    Console.WriteLine("{0,-20} {1,-15} {2,-10}", t.Nimi, t.Laji, t.TulosArvo);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ei tuloksia annetulla nimellä.");
                Console.ResetColor();
            }
        }

        static void LataaTulokset()
        {
            if (File.Exists(tiedosto))
            {
                foreach (var rivi in File.ReadAllLines(tiedosto))
                {
                    var osat = rivi.Split(';');
                    if (osat.Length == 3)
                    {
                        tulokset.Add(new Tulos(osat[0], osat[1], osat[2]));
                    }
                }
            }
        }

        static void TallennaTulokset()
        {
            using (StreamWriter sw = new StreamWriter(tiedosto))
            {
                foreach (var t in tulokset)
                {
                    sw.WriteLine($"{t.Nimi};{t.Laji};{t.TulosArvo}");
                }
            }
        }
    }

    class Tulos
    {
        public string Nimi { get; set; }
        public string Laji { get; set; }
        public string TulosArvo { get; set; } // EI samaa nimeä kuin luokka

        public Tulos(string nimi, string laji, string tulos)
        {
            Nimi = nimi;
            Laji = laji;
            TulosArvo = tulos;
        }
    }
}
