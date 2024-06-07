using System;

namespace feleves_beadando
{
    class Mezo
    {
        public int ID;
        public int ertek;
        public Jatekos tulaj;
        public Mezo(int ID, int ertek)
        {
            this.ID = ID;
            this.ertek = ertek;

        }

        public bool Van_e_tulaj()
        {
            return (tulaj != null);
        }

        public void Megvesz(Jatekos p)
        {
            tulaj = p;
            p.penz -= ertek;
        }

        

    }
}
