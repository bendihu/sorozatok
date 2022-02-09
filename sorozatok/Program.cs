namespace sorozatok
{
    public class Sorozat
    {
        public string Datum { get; set; }
        public string Cim { get; set; }
        public string EvadEsEpizod { get; set; }
        public int Hossz { get; set; }
        public bool Megnezte { get; set; }
    }

    public class Program
    {
        static List<Sorozat> sorozatok = new List<Sorozat>();
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //1. feladat
            Beolvas();

            //2. feladat
            Feladat2();

            //3. feladat
            Feladat3();

            //4. feladat
            Feladat4();

            //5. feladat
            Feladat5();

            //Hetnapja();

            //7. feladat
            Feladat7();

            //8. feladat
            Feladat8();

            Console.ReadLine();
        }
        private static void Beolvas()
        {
            string[] adatok = File.ReadAllLines("lista.txt");

            for (int i = 0; i < adatok.Length; i += 5)
            {
                Sorozat uj = new Sorozat();
                uj.Datum = adatok[i];
                uj.Cim = adatok[i + 1];
                uj.EvadEsEpizod = adatok[i + 2];
                uj.Hossz = int.Parse(adatok[i + 3]);
                uj.Megnezte = adatok[i + 4] == "1" ? true : false;
                sorozatok.Add(uj);
            }
        }
        private static void Feladat2()
        {
            Console.WriteLine("2. feladat");

            int count = sorozatok.Where(x => !x.Datum.Equals("NI")).Count();

            Console.WriteLine("A listában {0} db vetítési dátummal rendelkező epizód van", count);
            Console.WriteLine();
        }
        private static void Feladat3()
        {
            Console.WriteLine("3. feladat");

            int megnezte = sorozatok.Where(x => x.Megnezte).Count();
            decimal szazalek = (decimal)megnezte / sorozatok.Count * 100;

            Console.WriteLine($"A listában lévő epizódok {Math.Round(szazalek, 2)}%-át látta. ");
            Console.WriteLine();
        }
        private static void Feladat4()
        {
            Console.WriteLine("3. feladat");

            int osszegPercben = sorozatok.Where(x => x.Megnezte).Sum(x => x.Hossz);
            int nap = osszegPercben / 60 / 24;
            int ora = osszegPercben / 60 - (nap * 24);
            int perc = osszegPercben - (nap * 24 * 60) - (ora * 60);

            Console.WriteLine($"Sorozatnézéssel {nap} napot {ora} órát és {perc} percet töltött.");
            Console.WriteLine();
        }
        private static void Feladat5()
        {
            Console.WriteLine("5. feladat");
            Console.Write("Adjon meg egy dátumot! Dátum = ");
            string bekertDatum = Console.ReadLine();

            foreach (var item in sorozatok.Where(x => !x.Megnezte && x.Datum != "NI" && bekertDatum.CompareTo(x.Datum) >= 0))
            {
                Console.WriteLine($"{item.EvadEsEpizod}\t{item.Cim}");
            }
            Console.WriteLine();
        }
        private static string Hetnapja(int ev, int ho, int nap)
        {
            string[] napok = { "v", "h", "k", "sze", "cs", "p", "szo" };
            int[] honapok = { 0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4 };

            if (ho < 3) ev--;
            return napok[(ev + ev / 4 - ev / 100 + ev / 400 + honapok[ho - 1] + nap) % 7];
        }
        private static void Feladat7()
        {
            Console.WriteLine("7. feladat");
            Console.Write("Adja meg a hét egy napját(például cs)! Nap = ");
            string bekertNap = Console.ReadLine();

            var lista = new List<Sorozat>();

            foreach (var item in sorozatok.Where(x => !x.Datum.Equals("NI")))
            {
                int ev = Convert.ToInt32(item.Datum.Substring(0, 4));
                int ho = Convert.ToInt32(item.Datum.Substring(5, 2));
                int nap = Convert.ToInt32(item.Datum.Substring(8, 2));

                if (Hetnapja(ev, ho, nap).Equals(bekertNap)) lista.Add(item);
            }

            if (lista.Count == 0) Console.WriteLine("Az adott napon nem kerül adásba sorozat.");

            foreach (var group in lista.GroupBy(x => x.Cim))
            {
                Console.WriteLine(group.Key);
            }
            Console.WriteLine();
        }
        private static void Feladat8()
        {
            StreamWriter sw = new StreamWriter(@"summa.txt");

            var csoport = from x in sorozatok
                          group x by x.Cim into g
                          select new { Cim = g.Key, ÖsszPerc = g.Sum(g => g.Hossz), Sorozatok = g.Count() };

            foreach (var group in csoport)
            {
                sw.WriteLine($"{group.Cim} {group.ÖsszPerc} {group.Sorozatok}");
            }

            sw.Close();
        }
    }
}