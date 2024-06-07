using feleves_beadando;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace feleves_beadando
{
    public class Jatek
    {

        public static void Fut()
        {
            Random rnd = new Random();

            // valos input -> penz, mezok_szama, mezok_erteke 
            /*TODO fejlesztesek :
             * DONE map_gen -- még a playereket rá kell rakni
             * DONE HUD (jatekosok penzmennyisegevel esetleg mezojevel) 
             *  filebol beolvasas es megfelelo ertek adas
            */

            //input benyelese
            

            //file eleresi utvonal
            string link = Path.Combine(Environment.CurrentDirectory, "input.txt");
            
            string[] beolvasott = File.ReadAllLines(link, Encoding.Default);
            int penz = int.Parse(beolvasott[0]);
            int[] mezok_erteke = new int[beolvasott.Length-2];
            for (int i = 0; i < mezok_erteke.Length; i++)
            {
                mezok_erteke[i] = int.Parse(beolvasott[i+2]);
            }
            

            



            //input ertekek atadasa jatekos oszt-nak
            Jatekos_gen(penz);

            //jatekosok letrehozasa
            Jatekos[] jatekosok = Jatekos_gen(penz);

            //mezok generalasa 
            Mezo[] mezok = new Mezo[mezok_erteke.Length + 1];
            mezok[0] = new Mezo(0, 0); // START mezo
            for (int i = 1; i < mezok.Length; i++)
            {
                mezok[i] = new Mezo(i, mezok_erteke[i - 1]);

            }


            //maga a jatek
            bool megy_a_jatek = (jatekba_van(jatekosok).Length > 2);
            int idx = 0;
            while (megy_a_jatek)
            {
                jatekosok = jatekba_van(jatekosok);
                tabla_gen(jatekosok, mezok);
                szinezes(jatekosok[idx % jatekosok.Length]);


                Jatekos_lep(jatekosok[idx % jatekosok.Length], ref rnd, mezok);


                Console.ResetColor();


                idx++;
                if (jatekosok.Length == idx)
                    idx = 0;
                megy_a_jatek = (jatekba_van(jatekosok).Length > 2);
                Console.WriteLine("Enter -> tovább\n");
                Console.ReadKey();
                Console.Clear();

            }

            
            Jatekos[] nyerok = nyertesek(jatekosok);
            Console.WriteLine("\nA nyertesek:");
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine(nyerok[i].szin);
            }
            Console.ReadKey();
        }


        //nem fog kelleni 
        static int[] Input_gen(ref Random rnd) // mezokerteke
        {
            //felso mezo db hatar:71 
            //also mezo db hatar: 1(de miért akarna bárki 1 mezőt??)
            int[] T = new int[1];//paratlannak kell lennie

            for (int i = 0; i < T.Length; i++)
            {
                T[i] = rnd.Next(10, 50);
            }
            return T;
        }


        static Jatekos[] Jatekos_gen(int penz)
        {

            Jatekos Piros = new Jatekos(penz, "Piros");
            Jatekos Kék = new Jatekos(penz, "Kék");
            Jatekos Zöld = new Jatekos(penz, "Zöld");
            Jatekos Sárga = new Jatekos(penz, "Sárga");

            Jatekos[] jatekosok = { Piros, Kék, Zöld, Sárga };
            return jatekosok;
        }

        static void tabla_gen(Jatekos[] p, Mezo[] m) //kell: jatekos pozi, mezo db, mezo ertek,
                                                     //jatekos tulajdonjai

        //HUD gen ebbe johet
        {
            void Hud_gen() // kell jatekos pozi, jatekos penz, 
            {//szin,penz, mezo

                for (int j = 0; j < p.Length; j++)
                {
                    szinezes(p[j]);
                    Console.SetCursorPosition(j * 20, 25);
                    Console.WriteLine($"{p[j].szin} Játékos");
                    Console.SetCursorPosition(j * 20, 26);
                    Console.WriteLine($"Pénz:{p[j].penz}");
                    Console.SetCursorPosition(j * 20, 27);
                    Console.WriteLine($"Mező: {p[j].mezo}");
                    Console.ResetColor();

                }

            }

            int[] Tbol_oldalak(int T)
            {
                int a = T / 4; //magassag
                int b = a + (T % 4) / 2; //szelesseg
                int[] x = { a, b };
                return x;
            }

            int[] x = Tbol_oldalak(m.Length);
            //Console.WriteLine(string.Join(",",Tbol_oldalak(m.Length)));
            //x0 magassag
            //x1 szelesseg

            int idx = 0;
            int X = 50; //consolon kordinatak
            int Y = 0;
            Jatekos p_szin = p[0];

            bool p_plot(ref Jatekos p_szin)
            {
                for (int i = 0; i < p.Length; i++)
                {
                    if (p[i].mezo == idx)
                    {
                        p_szin = p[i];
                        return true;
                    }
                }
                return false;
            }

            bool mezo_van__tulaj(ref Mezo m)
            {
                if (m.tulaj != null)
                {
                    szinezes(m.tulaj);
                    return true;

                }
                return false;
            }

            Hud_gen();
            for (int i = 0; i < x[1]; i++)

            {
                Console.SetCursorPosition(X + (i * 4), Y);
                if (p_plot(ref p_szin))
                {
                    szinezes(p_szin);
                    Console.Write("#");
                    Console.ResetColor();
                }
                else if (mezo_van__tulaj(ref m[idx]))
                {
                    szinezes(m[idx].tulaj);
                    Console.Write(m[idx].ertek.ToString());
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(m[idx].ertek.ToString());
                }
                idx++;
            }//felso oldal
            for (int i = 0; i < x[0]; i++)
            {
                Console.SetCursorPosition(X + (x[1] * 4 - 4), Y + (i + 1));
                if (p_plot(ref p_szin))
                {
                    szinezes(p_szin);
                    Console.Write("#");
                    Console.ResetColor();
                }
                else if (mezo_van__tulaj(ref m[idx]))
                {
                    szinezes(m[idx].tulaj);
                    Console.Write(m[idx].ertek.ToString());
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(m[idx].ertek.ToString());
                }

                idx++;
            }// jobb oldal
            for (int i = x[1] - 1; i > 0; i--)
            {
                Console.SetCursorPosition(X + (i * 4), Y + x[0] + 1);
                if (p_plot(ref p_szin))
                {
                    szinezes(p_szin);
                    Console.Write("#");
                    Console.ResetColor();
                }
                else
                if (mezo_van__tulaj(ref m[idx]))
                {
                    szinezes(m[idx].tulaj);
                    Console.Write(m[idx].ertek.ToString());
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(m[idx].ertek.ToString());
                }
                idx++;
            }//also oldal
            for (int i = x[0] + 1; i > 0; i--)
            {
                Console.SetCursorPosition(X, Y + (i));
                if (p_plot(ref p_szin))
                {
                    szinezes(p_szin);
                    Console.Write("#");
                    Console.ResetColor();
                }
                else
                if (mezo_van__tulaj(ref m[idx]))
                {
                    szinezes(m[idx].tulaj);
                    Console.Write(m[idx].ertek.ToString());
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(m[idx].ertek.ToString());
                }
                idx++;
            }// bal oldal 


            Hud_gen();
            Console.SetCursorPosition(0, 0);

        }


        static Jatekos[] jatekba_van(Jatekos[] p_list) // kigyujteni akik jatszanak meg
        {
            int p_db = 0;
            for (int i = 0; i < p_list.Length; i++)
            {
                if (p_list[i].penz > 0)
                    p_db++;
            }
            Jatekos[] output = new Jatekos[p_db];
            int idx = 0;
            for (int i = 0; i < p_list.Length; i++)
            {
                if (p_list[i].penz > 0)
                {
                    output[idx] = p_list[i];
                    idx++;
                }
                else
                    Console.WriteLine($"{p_list[i].szin} adósságba esett:((");
            }
            return output;
        }

        static Jatekos[] nyertesek(Jatekos[] p) // return 2 nyertes tombbe
        
        {
            Jatekos[] nyertesek = new Jatekos[2];
            int idx = 0;
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i].penz > 0)
                {
                    nyertesek[idx] = p[i];
                    idx++;
                }
            }
            return nyertesek;
        }


        static void szinezes(Jatekos p)
        {
            if (p.szin == "Piros")
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (p.szin == "Kék")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (p.szin == "Zöld")
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (p.szin == "Sárga")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }


        }


        static void Jatekos_lep(Jatekos p, ref Random rnd, Mezo[] m)
        {
            void athalad_a_starton(Jatekos p, int dobas, Mezo[] m)
            {
                int min_mezo()

                {
                    int min_ertek = 99999;
                    bool valtozott = false;
                    for (int i = 0; i < m.Length - 1; i++)
                    {
                        if (m[i].ertek < min_ertek && m[i].tulaj == p)
                        {
                            min_ertek = m[i].ertek;
                            valtozott = true;
                        }
                    }
                    if (valtozott)
                        return min_ertek;
                    else
                        return 0;
                }//legkisebb mezo ertekenek megkeresese

                if (p.mezo + dobas > m.Length - 1) //ha athaladna a starton
                {
                    p.mezo = (p.mezo + dobas) % m.Length;
                    int startos_penz = min_mezo();
                    p.penz += startos_penz;
                    Console.WriteLine($"\nÁthaladtál a starton:DD Jutalmad: {startos_penz}");
                }
                else
                {
                    p.mezo += dobas;
                }

            }

            Console.WriteLine($"A {p.szin} játékos következik");


            int dobas = p.dobas(rnd);
            Console.WriteLine($"Eldobja a kockát: {dobas} \n");
            athalad_a_starton(p, dobas, m);
            Console.WriteLine($"Lépett a {p.mezo} mezőre");


            Console.WriteLine($"A mező értéke {m[p.mezo].ertek}"); //kifut az indexbol //TODO:javitani
            Console.WriteLine($"{p.szin} pénzösszege: {p.penz} \n");
            // itt vizsgalja hogy van e a mezonek tulaja 
            // aztan elagazas
            if (m[p.mezo].Van_e_tulaj())//ha van tulaj
            {
                if (m[p.mezo].tulaj != p) //ha NEM onmaga a tulaj
                {
                    Console.Write("\nUH OH ez a mező a ");
                    szinezes(m[p.mezo].tulaj);
                    Console.Write($"{m[p.mezo].tulaj.szin} játékosé\n");
                    p.penz -= m[p.mezo].ertek;
                    m[p.mezo].tulaj.penz += m[p.mezo].ertek;


                    szinezes(p);
                    Console.WriteLine("Vám megfizetve:(");
                }
                else
                {
                    Console.WriteLine("\nEz a mező már a tiéd");
                }


            }
            else if (m[p.mezo].ID != 0)//ha nincs tulaj és nem start 
            {
                Console.WriteLine("Ennek a mezőnek még nincs tulaja!\nMegveszed?(y/n)");
                string valasz = Console.ReadLine().ToLower();
                if (valasz == "y")
                {
                    m[p.mezo].Megvesz(p);
                }


            }
            Console.WriteLine($"{p.szin} új pénzösszege: {p.penz} \n");

        }

    }

}