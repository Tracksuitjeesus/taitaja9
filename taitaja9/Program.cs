using System;
using System.Collections.Generic;
using System.IO;

namespace taitaja9
{
    internal class Program
    {
        static List<Tulos> tulokset = new List<Tulos>();
        static string kansio = @"C:\Users\Esra\OneDrive - Kouvolan Ammattiopisto Oy, Eduko\Tiedostot\GitHub\taitaja9b"; // Muista vaihtaa omalle polulle
        static string tiedosto = Path.Combine(kansio, "tulokset.txt");

        static void Main(string[] args)
        {
            LuoKansioJosEiOle();
            Console.WriteLine($"Tallennetaan tiedosto: {tiedosto}");
            LataaTulokset();

            bool kaynnissa = true;
            while (kaynnissa)
            {
                NaytaValikko();
                string valinta = Console.ReadLine();

                switch (valinta)
                {
                    case "1": LisaaTulos(); break;
                    case "2": NaytaTulokset(); break;
                    case "3": HaeTuloksia(); break;
                    case "4": NaytaTuloksetSuodatettuna(); break;
                    case "5": kaynnissa = false; break;
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

        static void LuoKansioJosEiOle()
        {
            if (!Directory.Exists(kansio))
            {
                Directory.CreateDirectory(kansio);
                Console.WriteLine($"Kansio luotu: {kansio}");
            }
        }

        static void NaytaValikko()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- Taitaja 9 Tulostaulukko ---");
            Console.ResetColor();
            Console.WriteLine("1) Lisää tulos");
            Console.WriteLine("2) Näytä kaikki tulokset");
            Console.WriteLine("3) Hae tulos nimen perusteella");
            Console.WriteLine("4) Näytä tulokset suodatettuna/järjestettynä");
            Console.WriteLine("5) Lopeta");
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

        static void NaytaTuloksetSuodatettuna()
        {
            if (tulokset.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ei tuloksia näytettäväksi.");
                Console.ResetColor();
                return;
            }

            // Suodatus
            Console.WriteLine("\nHaluatko suodattaa tuloksia? (k/e)");
            string suodatusValinta = Console.ReadLine().ToLower();
            List<Tulos> naytettavat = new List<Tulos>(tulokset);

            if (suodatusValinta == "k")
            {
                Console.Write("Anna laji suodatettavaksi (tyhjä = kaikki): ");
                string lajiSuodatus = Console.ReadLine().ToLower();
                if (!string.IsNullOrEmpty(lajiSuodatus))
                {
                    naytettavat = naytettavat.FindAll(t => t.Laji.ToLower().Contains(lajiSuodatus));
                }
            }

            // Järjestys
            Console.WriteLine("\nHaluatko järjestää tulokset? (1 = Nimi, 2 = Laji, 3 = Tulos, muu = ei järjestystä)");
            string jarjestys = Console.ReadLine();

            switch (jarjestys)
            {
                case "1":
                    naytettavat.Sort((x, y) => x.Nimi.CompareTo(y.Nimi));
                    break;
                case "2":
                    naytettavat.Sort((x, y) => x.Laji.CompareTo(y.Laji));
                    break;
                case "3":
                    naytettavat.Sort((x, y) =>
                    {
                        if (double.TryParse(x.TulosArvo, out double tx) && double.TryParse(y.TulosArvo, out double ty))
                            return tx.CompareTo(ty);
                        return 0;
                    });
                    break;
            }

            if (naytettavat.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n{0,-20} {1,-15} {2,-10}", "Nimi", "Laji", "Tulos");
                Console.WriteLine(new string('-', 50));
                Console.ResetColor();

                foreach (var t in naytettavat)
                {
                    Console.WriteLine("{0,-20} {1,-15} {2,-10}", t.Nimi, t.Laji, t.TulosArvo);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ei tuloksia valituilla suodatuskriteereillä.");
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
                Console.WriteLine($"{tulokset.Count} tulosta ladattu tiedostosta.");
            }
        }

        static void TallennaTulokset()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(tiedosto))
                {
                    foreach (var t in tulokset)
                    {
                        sw.WriteLine($"{t.Nimi};{t.Laji};{t.TulosArvo}");
                    }
                }
                Console.WriteLine("Tiedosto tallennettu onnistuneesti!");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Virhe tallennettaessa tiedostoa: " + ex.Message);
                Console.ResetColor();
            }
        }
    }

    class Tulos
    {
        public string Nimi { get; set; }
        public string Laji { get; set; }
        public string TulosArvo { get; set; }

        public Tulos(string nimi, string laji, string tulos)
        {
            Nimi = nimi;
            Laji = laji;
            TulosArvo = tulos;
        }
    }
}
