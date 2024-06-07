using System;


namespace feleves_beadando
{
    public class Jatekos
    {
        //public int azonosito;
        public string szin = "";
        public int mezo = 0;
        public int penz;


        public Jatekos(int penz, string szin) //int azonosito,
        {
            //this.azonosito = azonosito;
            this.penz = penz;
            this.szin = szin;

        }

        public int dobas(Random rnd)
        {
            int dobas = rnd.Next(1, 7);
            
            
            return dobas;
        }
    }
}
