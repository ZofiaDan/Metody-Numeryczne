using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BIBLIOTEKA_NR1.CalkiPochodne
{
    public abstract class Ekstrapolacja
    {
        public static string[] Komunikat =
        { /*0*/ "Brak błędu",
       /*1*/ "Parametr q w metodzie EkstrapolacjaAitkena()musi"+
             " być większy od 1  ",
       /*2*/ "Krok ekstrapolacji mniejszy od zera komputerowego ",
       /*3*/ "Przekroczono maksymalnie zadaną ilość iteracji"+
              " w procesie ekstrapolacji",
     };

        /// <summary>
        /// Funkcja publiczna do wyświetlania komunikatów błędów
        /// w procesie kompilacji
        /// </summary>
        /// <param name="k">Numer błędu od 0 do 4</param>
        public static void PiszKomunikat(int k)
        {
            MessageBox.Show(Komunikat[k]);
        }

        /// <summary>
        ///Funkcję EkstapolacjaProcesu należy zdefiniować w obiektach 
        ///potomnych dla różnych procesów iteracyjnych 
        ///np.: różniczkowania i całkowania numerycznego
        /// </summary>
        /// <param name="x">Argument funkcji</param>
        /// <returns>Zwracana wartość funkcji</returns>
        public abstract double EkstapolacjaProcesu(double x);

        protected bool JestOsiagnietePrzyblizenie;
        protected int maxit;  //Maksymalna ilość iteracji procesu ekstrapolacji
        protected double F0;  //Wartość ekstrapolowanej wielkości
        protected double q;   //Iloraz kolejnych kroków iteracji 
        protected double h0;  //Wstępnie ustalony krok w procesie iteracji
        protected double p;   //Potęga występująca we wzorach na błąd obcięcia np 2, 4
        protected double eps; //Dokładność iteracji
protected double epsm;//Błąd komputera dla danej dokładności obliczeń

     /// <summary>
     /// Konstruktor klasy EkstrapolacjaAbstr bez argumentów
     /// </summary>
     public Ekstrapolacja() { }

        /// <summary>
        /// Konstruktor klasy EkstrapolacjaAbstr z argumentami
        /// </summary>
        /// <param name="h0">Wstępnie ustalony krok w procesie iteracji</param>
        /// <param name="q">Iloraz kolejnych kroków iteracji</param>
        /// <param name="eps">Dokładność iteracji</param>
        /// <param name="maxit">Maksymalna ilość iteracji ekstrapolacji</param>
        public Ekstrapolacja(double h0, double q, double eps, int maxit)
        {
            this.h0 = h0; this.q = q; this.eps = eps; this.maxit = maxit;
            JestOsiagnietePrzyblizenie = false;
            //Wyznacz błąd maszynowy
            double x;
            epsm = 1.0;
            do
            {
                epsm = epsm / 2;
                x = epsm + 1;
            }
            while (x > 1);
        }//Koniec konstruktora EkstrapolacjaAbstr

        /// <summary>
        /// Metoda Aitkena do ekstrapolacji procesu F(h) dla długości kroku h
        /// zmierzającego do zera dla znanej struktury wzoru na bład obcięcia
        /// F(h)-F(0) = b1*(h)^p+ O(h^r) ; r>p  gdzie potega p oraz współczynik
        /// b1  wyznacza się w krokach iteracji Aitkena  wzór(3.12)
        /// </summary>
        /// <returns>Zwraca numer błędu, 0 brak błędu</returns>
        protected int EkstrapolacjaAitkena()
        {
            int m;
            double F1, F2, F3, F21, h1, h2, h3, epsx, p;
            bool CzyZaMalyKrok;
            m = 0; h1 = h0;
            h2 = q * h1; h3 = q * h2; F0 = 0;
            if (q > 1)
            {
                do
                {
                    m++;
                    F1 = EkstapolacjaProcesu(h1);
                    F2 = EkstapolacjaProcesu(h2);
                    F3 = EkstapolacjaProcesu(h3);
                    F21 = F2 - F1;
                    epsx = F21 * F21 / (F1 - 2 * F2 + F3);
                    p = Math.Log((F3 - F2) / (F2 - F1)) / Math.Log(q);
                    F0 = F1 - epsx;
                    h1 /= q;
                    h2 = q * h1; h3 = q * h2;
                    JestOsiagnietePrzyblizenie = Math.Abs(epsx) < eps;
                    CzyZaMalyKrok = h1 < epsm;
                }
                while (!(CzyZaMalyKrok || JestOsiagnietePrzyblizenie || (m > maxit)));
                if (CzyZaMalyKrok) return 2;
                else
                    if (m > maxit) return 3;
                    else
                    {
                        return 0;
                    }
            }
            else return 1;
        }
    }

}
