using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace CSBibliotekaStudent.EkstraCalkaRozniczka
{
    /// <summary>
    /// Klas abstrakcyjna do ekstrapolacji procesów zadanych w postaci 
    /// funkcji rzeczywistej zmiennej rzeczywistej 
    /// double EkstapolacjaProcesu(double x)
    /// </summary>
    public abstract class EkstrapolacjaAbstr 
    {
        public static string[] Komunikat = 
        { /*0*/ "Brak błędu",
          /*1*/ "Parametr q w metodzie EkstrapolacjaAitkena()musi być większy od 1  ",
          /*2*/ "Krok ekstrapolacji mniejszy od zera komputerowego ",
          /*3*/ "Przekroczono maksymalnie zadaną ilość iteracji w procesie ekstrapolacji",
        };
        public static void PiszKomunikat(int k)
        {
            MessageBox.Show(Komunikat[k]);
        }
        
        //Funkcję EkstapolacjaProcesu należy zdefiniować w obiektach potomnych dla różnych
        //procesów iteracyjnych np.: różniczkowania i całkowania numerycznego
        public abstract double EkstapolacjaProcesu(double x);

        protected bool JestOsiagnietePrzyblizenie;
        protected int maxit;
        protected double F0;    //Wartość ekstrapolowanej wielkości
        protected double q;     //Iloraz kolejnych kroków iteracji 
        protected double h0;    //Wstępnie ustalony krok w procesie iteracji
        protected double p;     //Potęga występująca we wzorach na błąd obcięcia np 2, 4
        protected double eps;   //Dokładność iteracji
        protected double epsm;  // błąd komputera dla danej dokładności obliczeń Typ

        public EkstrapolacjaAbstr() { }

        /// <summary>
        /// Konstruktor 
        /// </summary>
        /// <param name="h0">Wstępnie ustalony krok w procesie iteracji</param>
        /// <param name="q">Iloraz kolejnych kroków iteracji</param>
        /// <param name="eps">Dokładność iteracji</param>
        /// <param name="maxit"></param>
        public EkstrapolacjaAbstr(double h0, double q, double eps, int maxit)
        {
            this.h0 = h0; this.q = q; this.eps = eps; this.maxit = maxit;
            JestOsiagnietePrzyblizenie = false;
            //Wyznacz błąd maszynowy
            double x; epsm = 1.0;
            do
            {
                epsm = epsm / 2;
                x = epsm + 1;
            }
            while (x > 1);
        }//Koniec konstruktora EkstrapolacjaAbstr
        //-------------------------------------------------------------------

  
        //Metoda Aitkena do ekstrapolacji procesu F(h) dla długości kroku h
        //zmierzającego do zera dla znanej struktury wzoru na bład obcięcia
        //F(h)-F(0) = b1*(h)^p+ O(h^r) ; r>p  gdzie potega p oraz współczynik
        //b1  wyznacza się w krokach iteracji Aitkena  wzór(3.12)

        protected int EkstrapolacjaAitkena()
        {
            int m;
            double F1, F2, F3, F21, h1, h2, h3, epsx;
            bool CzyZaMalyKrok;
            //double[] A = new double[maxit + 1]; 
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
                        //if (JestOsiagnietePrzyblizenie) F0 = A[m];
                        return 0;
                    }
            }
            else return 1;
        }

  
    }

    /// <summary>
    /// Klasa abstrakcyjna do ekstrapolacji procesów zwracająca wartość 
    /// w postaci wektora double[] EkstapolacjaProcesuWektor(double x);
    /// </summary>
    public abstract class EkstrapolacjaWektorAbstr
    {
        public static string[] Komunikat = 
        { /*0*/ "Brak błędu",
          /*1*/ "Parametr q w metodzie EkstrapolacjaAitkena()musi być większy od 1  ",
          /*2*/ "Krok ekstrapolacji mniejszy od zera komputerowego ",
          /*3*/ "Przekroczono maksymalnie zadaną ilość iteracji w procesie ekstrapolacji",
        };
        public static void PiszKomunikat(int k)
        {
            MessageBox.Show(Komunikat[k]);
        }

        //Funkcję EkstapolacjaProcesu należy zdefiniować w obiektach potomnych dla różnych
        //procesów iteracyjnych np.: różniczkowania i całkowania numerycznego
        public abstract double[] EkstapolacjaProcesuWektor(double x);

        protected bool JestOsiagnietePrzyblizenie;
        protected int maxit;
        protected int M;
        protected double[] W0;    //Wartość ekstrapolowanej wielkości wektorowej
        protected double q;     //Iloraz kolejnych kroków iteracji 
        protected double h0;    //Wstępnie ustalony krok w procesie iteracji
        protected double p;     //Potęga występująca we wzorach na błąd obcięcia np 2, 4
        protected double eps;   //Dokładność iteracji
        protected double epsm;  // błąd komputera dla danej dokładności obliczeń Typ

        public EkstrapolacjaWektorAbstr() { }

        /// <summary>
        /// Konstruktor 
        /// </summary>
        /// <param name="h0">Wstępnie ustalony krok w procesie iteracji</param>
        /// <param name="q">Iloraz kolejnych kroków iteracji</param>
        /// <param name="eps">Dokładność iteracji</param>
        /// <param name="M">Wymiar wektora ekstrapolowanego </param>
        /// <param name="maxit">Maksymalna ilość iteracji</param>
        public EkstrapolacjaWektorAbstr(double h0, double q, double eps, int maxit,int M)
        {
            this.h0 = h0; this.q = q; this.eps = eps; this.maxit = maxit; this.M = M;
            JestOsiagnietePrzyblizenie = false;
            W0=new double[M+1];
            //Wyznacz błąd maszynowy
            double x; epsm = 1.0;
            do
            {
                epsm = epsm / 2;
                x = epsm + 1;
            }
            while (x > 1);
        }//Koniec konstruktora EkstrapolacjaAbstr
        //-------------------------------------------------------------------


        //Metoda Aitkena do ekstrapolacji procesu F(h) dla długości kroku h
        //zmierzającego do zera dla znanej struktury wzoru na bład obcięcia
        //F(h)-F(0) = b1*(h)^p+ O(h^r) ; r>p  gdzie potega p oraz współczynik
        //b1  wyznacza się w krokach iteracji Aitkena  wzór(3.12)
        protected int EkstrapolacjaAitkenaWektor()
        {
            int m;
            double[] F1, F2, F3;
            double F123,F21, h1, h2, h3, epsx;
            bool CzyZaMalyKrok;
            m = 0; h1 = h0;
            h2 = q * h1; h3 = q * h2;
            double epsy ;//= 11.0;
            if (q > 1)
            {
                do
                {
                    m++;
                    epsy = 0;
                    F1 = EkstapolacjaProcesuWektor(h1);
                    F2 = EkstapolacjaProcesuWektor(h2);
                    F3 = EkstapolacjaProcesuWektor(h3);
                    for (int i = 1; i < F1.Length; i++)
                    {
                        F21 = F2[i] - F1[i];
                        F123=F1[i] - 2 * F2[i] + F3[i];
                        if (Math.Abs(F123) > 0)
                        {
                            epsx = F21 * F21 / F123;
                            W0[i] = F1[i] - epsx;
                            if (Math.Abs(epsx) > epsy) epsy = Math.Abs(epsx);
                        }
                        else W0[i] = F1[i];
                    }
                    h1 /= q;
                    h2 = q * h1; h3 = q * h2;
                    JestOsiagnietePrzyblizenie = epsy < eps;
                    CzyZaMalyKrok = h1 < epsm;
                }
                while (!(CzyZaMalyKrok || JestOsiagnietePrzyblizenie || (m > maxit)));
                if (CzyZaMalyKrok) return 2;
                else
                    if (m > maxit) return 3;
                    else
                    {
                        //if (JestOsiagnietePrzyblizenie) F0 = A[m];
                        return 0;
                    }
            }
            else return 1;
        }

    }

}
