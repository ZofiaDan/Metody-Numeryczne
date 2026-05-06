using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSBibliotekaStudent.DifferentialEquation
{
    public abstract class DifferentialEquationW
    {
      protected bool FJestRozwiazanie; //Przyjmuje wartość true,
        //jeżeli wywołano metodę Obliczaj

        public System.Windows.Forms.ProgressBar FPG;  //Wskażnik całkowania
        protected bool FWywolanoKontynuuj;
        protected bool FWywolanoZerujIKontynuuj;

        //Iloczyn wektora przez liczbę

        //FMS-tablica dynamiczna do zapamiętania rozwiązania
        //FMS[NrIteracji,NrZmiennej] - element wektora stanu NrZmiennej
        //                             iteracji numer NrIteracji
        public List<List<double>> FMS;    //Do zapisu zmiennych stanu

        public int FN;    //FN-liczba równań
        public int FLW;   //FLW-liczba wektorow stanu
        public double FTp;  //FTp-czas początkowy
        public double FTk;  //FTk-czas końcowy
        public bool FStKr;  //FStKr-true - stały krok całkowania
        //                         -false- automatyczny dobór kroku całkowania
        public double FH0;      //FH0- wartość stałego kroku całkowania
        public double FEpsW;    //FEpsW - tolerancja błędu względnego
        public double FEpsWM;   //FEpsWM- Minimalna tolerancja błędu względnego
        public double FEpsA;    //FEpsA - błąd bezwzględny
        public double Fepsm;    // błąd komputera dla danej dokładności obliczeń
        public double FA26;     // FA26=Fepsm*26

        public int FMaxWs;//Maksimum wskażnika całkowania

        
        //Norma wektora stanu E
        public static double NormaX(double[] E)
        {
            int i;
            int N = E.Length;
            double S, W;
            S = 0;
            for (i = 1; i <= N - 1; i++)
            {
                W = Math.Abs(E[i]);
                if (W > S) S = W;
            }
            return S;
        }

        //Metoda prywatna klasy dopisująca wektor stanu w danej
        //chwili całkowania.
        protected void Dopisz(double[] X)
        {
            FMS.Add(new List<double>((int)FN + 1));
            for (int j = 0; j <= FN; j++)
            {
                FMS[FLW].Add(X[j]);
            }
            FLW++;
        }

       
        //Metoda prywatna klasy TDifferentialEquationAbstract zerująca tablicę dynamiczną
        //do zapisywania wektorów stanu , ustalająca licznik wektorów
        //stanu na zerze FLW=0 oraz zapisująca do tej tablicy warunek
        //początkowy
        protected void Zeruj()
        {
            double[] X= new double[FN+1];
            for (int l = 0; l <= FN; l++) X[l] = FMS[0][l];
            FMS.Clear();
            FLW = 0;
            Dopisz(X);
            FJestRozwiazanie = false;
        }

        protected void BladMaszynowy()
        {
            double x; Fepsm = 1;
            do
            {
                Fepsm = Fepsm / 2.0;
                x = Fepsm + 1.0;
            }
            while (x > 1);
        }

        //Metodę Obliczaj należy zdefiniować w obiektach potomnych
        protected abstract int Obliczaj();


        //public TWskaznikPostepuAbstr *FPG;  //Wskażnik całkowania


        //Metoda prywatna dla właściwości WarunekPoczatkowy do pobierania
        //z zewnątrz warunku początkowego
        public List<double> PobierzWP()
        {
           return FMS[0];
        }
        //Metoda prywatna dla właściwości WarunekPoczatkowy do zadawania
        //z zewnątrz warunku początkowego
        public void UstawWP(double[] WP)
        {
            if (FMS != null) FMS.Clear();
            FLW = 0;
            Dopisz(WP);
            FJestRozwiazanie = false;
        }

        public DifferentialEquationW()
        {
            FWywolanoKontynuuj = false;
            FWywolanoZerujIKontynuuj = false;
        }
        // Inicjacja obiektu
        //N    - liczba równań
        //Tp   - czas początkowy obliczeń
        //Tk   - czas końcowy obliczeń
        //X0   - wektor zawierający warunek początkowy
        public DifferentialEquationW(int N, double Tp, double Tk, double[] X0,
                System.Windows.Forms.ProgressBar PG)
        {
            FN = N; FTp = Tp; FTk = Tk; FLW = 0;
            FStKr = false; FH0 = 1e-3; FEpsW = 1e-6;
            FEpsWM = 1e-9; FEpsA = 1e-6;
            FJestRozwiazanie = false;
            FWywolanoKontynuuj = false;
            FWywolanoZerujIKontynuuj = false;
            FMS = new List<List<double>>();
            FMS.Add(new List<double>((int)FN + 1));
            for (int j = 0; j <= FN; j++)
            {
              FMS[0].Add(X0[j]);
            }

            //Zapisywanie warunku początkowego
            //Wy znaczanie błędu maszynowego Fepsm dla wybranej precyzji obliczeń
            double x; Fepsm = 1;
            do
            {
              Fepsm = Fepsm / 2.0; 
              x = Fepsm + 1.0;
            } 
            while (x > 1);
            FA26 = 26.0 * Fepsm;
            FPG = PG;
            if (FPG != null)
            {
               FPG.Minimum=0;
               FMaxWs = FPG.Maximum;
            }
            else FMaxWs = 0;
        }

        //Metoda prywatna sprawdzająca poprawność podstawienia względnej
        //dokładności obliczeń (wg wzoru (5.27)) i zadanego przedziału
        //całkowania (wzór (5.26) oraz inicjująca przedefiniowaną metodę
        //obliczeń
        public int Rozwiaz()
        {
            int Blad; double dt;
            Zeruj();
            BladMaszynowy();
            if (FEpsW < 2 * Fepsm + FEpsWM) FEpsW = 2 * Fepsm + FEpsWM;
            dt = FTk - FTp;
            dt = Math.Abs(dt);
            if (dt < FA26) return 6;
            else
            {
                Blad = Obliczaj();
                if (Blad == 0)
                {
                    FTp = Czas(0);
                    FTk = Czas(FLW - 1);
                    FJestRozwiazanie = true;
                }
                return Blad;
            }
        }

        // Kontynuacja obliczeń przez czas DeltaT. Dotychczasowe wektory stanu są zachowane
        // Metodę Kontynuuj można wywołać po metodzie Rozwiaz lub zamiast niej
        public int Kontynuuj(double DeltaT)
        {
            int Blad;
            //FJestRozwiazanie=false;
            FWywolanoKontynuuj = true;
            //FWywolanoZerujIKontynuuj=false;
            if (FEpsW < 2 * Fepsm + FEpsWM)
                FEpsW = 2 * Fepsm + FEpsWM;
            if (Math.Abs(FTk - FTp) < FA26) return 6;
            else
            {
                FTp = Czas(0);
                FTk = Czas(FLW - 1);
                FTk += DeltaT;
                Blad = Obliczaj();
                FTp = Czas(0);
                FTk = Czas(FLW - 1);
                FJestRozwiazanie = true;
                return Blad;
            }
        }

        // Kontynuacja obliczeń przez czas DeltaT. Dotychczasowe wektory stanu są wyzerowane,
        // przy czym ostatni z nich staje sie warunkiem początkowycm
        // Metodę ZerujIKontynuuj można wywołać po metodzie Rozwiaz lub zamiast niej
        public virtual int ZerujIKontynuuj(double DeltaT)
        {
            int Blad;
            double[] X1 = new double[FN + 1];
            for (int i = 0; i <= FN; i++) X1[i] = FMS[FLW - 1][i];
            X1[0] = 0;
            FWywolanoZerujIKontynuuj = true;
            FMS.Clear();
            FLW = 0;
            Dopisz(X1);
            FTp = 0;
            FTk = DeltaT;
            Blad = Obliczaj();
            FTp = Czas(0);
            FTk = Czas(FLW - 1);
            return Blad;
        }

        //Czas - funkcja publiczna klasy do pobrania chwili czasowej
        //       wektora stanu o numerze NrWek
        public double Czas(int NrWek)
        {
            return FMS[NrWek][0];
        }

        //X - funkcja publiczna klasy do pobrania składowej Nr
        //    wektora stanu o numerze NrWek
        public double X(int NrWek, int Nr)
        {
            if (Nr > 0 && Nr <= FN) return FMS[NrWek][Nr];
            else
            {
                 return FMS[0][0];
            }
        }

        //WektorStanu - metoda publiczna klasy do pobrania wektora
        //             stanu o numerze NrWek
        public List<double> WektorStanu(int NrWek)
        {
           return FMS[NrWek];
        }

    }

    public abstract class TNonLinearDifferentialEquationAbstractW : DifferentialEquationW
    {
        public int FRzad;   //FRzad -rząd metody
        public int FNetap;  //Ilość etapów

        public TNonLinearDifferentialEquationAbstractW()
            : base()
        { }

        public TNonLinearDifferentialEquationAbstractW(int N, double Tp1, double Tk1, double[] X0, System.Windows.Forms.ProgressBar PG)
            : base(N, Tp1, Tk1, X0,PG)
        { }


        public abstract void FunNonLinearDiffEquation(double[] F, double[] X, double t);

        protected double[] K1, K2, K3, K4, K5, K6, K7, K8, K9, K10, K11, K12, K13, X1, KK;

        //c- wektor odpowiadający pierwszej kolumnie tablicy 1 Butchera
        //w1- wektor odpowiadający ostatniemu wierszowi tablicy 1 Butchera
        //e - wektor błędów w metodach włożonych
        //KX - wektor (12) do realizacji obliczeń w metodach Rungego-Kutta
        protected double[] c,c2, w1, w2, e;
        protected double[,] a, b;
        protected double[,] KX;
        public void MetodaRungeKuttaPairs(int Netap, double h, double t0,
                double[] X0, double[] X, double[] E,
            double[,] a, double[] c, double[] w, double[] e)
        {
            int i, j, l, k;
            double t;
            FunNonLinearDiffEquation(KK, X0, t0);
            for (k = 1; k <= FN; k++) KX[1,k] = KK[k] * h;
            for (i = 2; i <= Netap; i++)
            {
                t = t0 + c[i] * h;
                for (l = 1; l <= FN; l++) X1[l] = X0[l];
                for (j = 1; j <= i - 1; j++)
                    for (k = 1; k <= FN; k++) X1[k] += KX[j,k] * a[i,j];
                FunNonLinearDiffEquation(KK, X1, t);
                for (k = 1; k <= FN; k++) KX[i,k] = KK[k] * h;
            }
            for (i = 1; i <= FN; i++) E[i] = 0;
            for (i = 1; i <= Netap; i++)
                for (k = 1; k <= FN; k++) E[k] += KX[i,k] * e[i];
            for (l = 1; l <= FN; l++) X[l] = X0[l];
            for (j = 1; j <= Netap; j++)
                for (k = 1; k <= FN; k++) X[k] += KX[j,k] * w[j];
        }

        protected int DaneFehlberg(int K)
        {
            //K- rząd metody
            switch (K)
            {
                case 1:
                    // para metod włożonych 1. i 2. rzędu - wzory (2.37)
                    a = new double[4, 4];
                    c = new double[4];
                    w1 = new double[4];
                    w2 = new double[4];
                    e = new double[4];
                    c[1] = 0; a[1,1] = 0;
                    c[2] = 0.5; a[2,1] = 1.0 / 2.0;
                    c[3] = 1.0; a[3,1] = 1.0 / 256.0; a[3,2] = 255.0 / 256.0;
                    w2[1] = 1.0 / 512.0; w2[2] = 510.0 / 512.0; w2[3] = 1.0 / 512.0;
                    w1[1] = 1.0 / 256.0; w1[2] = 255.0 / 256.0; w1[3] = 0;
                    e[1] = -1.0 / 512.0; e[2] = 0; e[3] = 1.0 / 512.0;
                    KX = new double[4, FN + 1];
                    return 0;
                case 2:
                    //para metod włożonych 2. i 3. rzędu - wzory (2.38)
                    a = new double[5,5];
                    c = new double[5];
                    w1 = new double[5];
                    w2 = new double[5];
                    e = new double[5];
                    c[1] = 0; a[1,1] = 0;
                    c[2] = 1.0 / 4.0; a[2,1] = 1.0 / 4.0;
                    c[3] = 27.0 / 40.0; a[3,1] = -189.0 / 800.0; a[3,2] = 729.0 / 800.0;
                    c[4] = 1.0; a[4,1] = 214.0 / 891.0; a[4,2] = 1.0 / 33.0; a[4,3] = 650.0 / 891.0;
                    w2[1] = 533.0 / 2106.0; w2[2] = 0; w2[3] = 800.0 / 1053.0; w2[4] = -1.0 / 78.0;
                    w1[1] = 214.0 / 891.0; w1[2] = 1.0 / 33.0; w1[3] = 650.0 / 891.0; w1[4] = 0;
                    e[1] = 23.0 / 1782.0; e[2] = -1.0 / 33.0; e[3] = 350.0 / 11583.0; e[4] = -1.0 / 78.0;
                    KX = new double[5, FN + 1];
                    return 0;
                case 3:
                    //para metod włożonych 3 i 4 rzędu - wzory (2.39)
                    a = new double[6,6];
                    c = new double[6];
                    w1 = new double[6];
                    w2 = new double[6];
                    e = new double[6];
                    c[1] = 0; a[1,1] = 0;
                    c[2] = 2.0 / 7.0; a[2,1] = 2.0 / 7.0;
                    c[3] = 7.0 / 15.0; a[3,1] = 77.0 / 900.0; a[3,2] = 343.0 / 900.0;
                    c[4] = 35.0 / 38.0; a[4,1] = 805.0 / 1444.0; a[4,2] = -77175.0 / 54872.0;
                    a[4,3] = 97125.0 / 54872.0;
                    c[5] = 1.0; a[5,1] = 79.0 / 490.0; a[5,2] = 0;
                    a[5,3] = 2175.0 / 3626.0; a[5,4] = 2166.0 / 9065.0;
                    w2[1] = 229.0 / 1470.0; w2[2] = 0; w2[3] = 1125.0 / 1813.0;
                    w2[4] = 13718.0 / 81585.0; w2[5] = 1.0 / 18.0;
                    e[1] = 4.0 / 735.0; e[2] = 0; e[3] = -75.0 / 3626.0; e[4] = 5776.0 / 81585.0;
                    e[5] = -1.0 / 18.0;
                    for (int i = 1; i <= 5; i++) w1[i] = w2[i] - e[i];   //?? sprawdzić
                    KX = new double[6, FN + 1];
                    return 0;
                case 4:
                    //para metod włożonych 4. i 5. rzędu - wzory (2.40)
                    a = new double[7,7];
                    c = new double[7];
                    w1 = new double[7];
                    w2 = new double[7];
                    e = new double[7];
                    c[1] = 0; a[1,1] = 0;
                    c[2] = 0.25; a[2,1] = 1.0 / 4.0;
                    c[3] = 3.0 / 8.0; a[3,1] = 3.0 / 32.0; a[3,2] = 9.0 / 32.0;
                    c[4] = 12.0 / 13.0; a[4,1] = 1932.0 / 2197.0; a[4,2] = -7200.0 / 2197.0;
                    a[4,3] = 7296.0 / 2197.0;
                    c[5] = 1.0; a[5,1] = 439.0 / 216.0; a[5,2] = -8.0;
                    a[5,3] = 3680.0 / 513.0; a[5,4] = -845.0 / 4104.0;
                    c[6] = 0.5; a[6,1] = -8.0 / 27.0; a[6,2] = 2.0;
                    a[6,3] = -3544.0 / 2565.0; a[6,4] = 1859.0 / 4104.0; a[6,5] = -11.0 / 40.0;
                    w2[1] = 16.0 / 135.0; w2[2] = 0; w2[3] = 6656.0 / 12825.0;
                    w2[4] = 28561.0 / 56430.0; w2[5] = -9.0 / 50.0; w2[6] = 2.0 / 55.0;
                    w1[1] = 25.0 / 216.0; w1[2] = 0; w1[3] = 1408.0 / 2565.0;
                    w1[4] = 2197.0 / 4104.0; w1[5] = -1.0 / 5.0; w1[6] = 0;
                    for (int i = 1; i <= 6; i++) e[i] = w2[i] - w1[i];
                    KX = new double[7, FN + 1];
                    return 0;

                case 5:
                    //para metod włożonych 5. i 6. rzędu 8 etapowa- wzory (2.41)
                    a = new double[9,9];
                    c = new double[9];
                    w1 = new double[9];
                    w2 = new double[9];
                    e = new double[9];
                    c[1] = 0; a[1,1] = 0;
                    c[2] = 1.0 / 6.0; a[2,1] = 1.0 / 6.0;
                    c[3] = 4.0 / 15.0; a[3,1] = 4.0 / 75.0; a[3,2] = 16.0 / 75.0;
                    c[4] = 2.0 / 3.0; a[4,1] = 5.0 / 6.0; a[4,2] = -8.0 / 3.0; a[4,3] = 5.0 / 2.0;
                    c[5] = 4.0 / 5.0; a[5,1] = -8.0 / 5.0; a[5,2] = 144.0 / 25.0;
                    a[5,3] = -4.0; a[5,4] = 16.0 / 25.0;
                    c[6] = 1.0; a[6,1] = 722.0 / 640.0; a[6,2] = -2304.0 / 640.0;
                    a[6,3] = 2035.0 / 640.0; a[6,4] = -88.0 / 640.0; a[6,5] = 275.0 / 640.0;
                    c[7] = 0; a[7,1] = -11.0 / 640.0; a[7,2] = 0;
                    a[7,3] = 11.0 / 256.0; a[7,4] = -11.0 / 160.0; a[7,5] = 11.0 / 256.0;
                    a[7,6] = 0;
                    c[8] = 1.0; a[8,1] = 93.0 / 640.0; a[8,2] = -18.0 / 5.0;
                    a[8,3] = 803.0 / 256.0; a[8,4] = -11.0 / 160.0; a[8,5] = 99.0 / 256.0;
                    a[8,6] = 0; a[8,7] = 1.0;
                    w2[1] = 7.0 / 1408.0; w2[2] = 0; w2[3] = 1125.0 / 2816.0; w2[4] = 9.0 / 32.0;
                    w2[5] = 125.0 / 768.0; w2[6] = 0; w2[7] = 5.0 / 66.0; w2[8] = 5.0 / 66.0;

                    w1[1] = 31.0 / 384.0; w1[2] = 0; w1[3] = 1125.0 / 2816.0;
                    w1[4] = 9.0 / 32.0; w1[5] = 125.0 / 768.0; w1[6] = 5.0 / 66.0;
                    w1[7] = 0; w1[8] = 0;
                    e[1] = 5.0 / 66.0; e[2] = 0; e[3] = 0; e[4] = 0; e[5] = 0;
                    e[6] = 5.0 / 66.0; e[7] = -5.0 / 66.0; e[8] = -5.0 / 66.0;
                    KX = new double[9, FN + 1];
                    return 0;
                default: return 2;
            }
        }//DaneFehlberg

        public int Fehlberg(int K, int N, double h, double t0,
                 double[] X0, double[] X, double[] E)
        {
            //K - ilość etapów metody
            MetodaRungeKuttaPairs(K, h, t0, X0, X, E, a, c, w2, e);
            return 0;
        }
    }

    //Klasa potomna klasy TNonLinearDifferentialEquationAbstract do rozwiązywania układów równań
    //różniczkowych metodą Fehlberga
    public abstract class TFehlbergAbstractW : TNonLinearDifferentialEquationAbstractW
    {

        //Metoda prywatna zawierająca konstrukcję algorytmu
        //Fehlberga przedefiniowana zgodnie z deklaracją
        //w prototypie  TDifferentialEquationAbstract jako abstract
        protected override int Obliczaj()
        {
            double alfa, t, e, xt, mxt, H, Hmin, ee, Hph, h1,Skala=0;
            int[] NN = new int[8] { 0, 3, 4, 5, 6, 8, 0, 13 };
            int KKx, i, j, Blad;
            double[] Er, XX1, Y;
            Er =new double[FN+1];
            Y = new double[FN + 1];
            X1 = new double[FN + 1];
            XX1 = new double[FN + 1];
            KK = new double[FN + 1];
            DaneFehlberg(FRzad);
            Blad = 0;
            KKx = 0;
            if (FPG != null)
            { Skala = FMaxWs / (FTk - FTp); FPG.Minimum=0; }
            for (j = 0; j <= FN; j++) Y[j] = FMS[FLW - 1][j];
            t = Y[0];
            if (FStKr) H = FH0;
            else H = (FTk - t) / 1000;
            if (FRzad >= 1 && FRzad <= 5)
                do
                {
                    Blad = Fehlberg(NN[FRzad], FN, H, t, Y, XX1, Er);
                    ee = 0;
                    mxt = 0;
                    for (j = 1; j <= FN; j++)
                    {
                        e = Math.Abs(Er[j]);
                        if (ee < e) ee = e;
                        xt = (Math.Abs(Y[j]) + Math.Abs(XX1[j])) / 2;
                        if (mxt < xt) mxt = xt;
                    }
                    ee = ee / (mxt * FEpsW + FEpsA);
                    if (ee == 0) Hph = 0.9;
                    else Hph = Math.Exp(Math.Log(ee) / (FRzad + 1));
                    h1 = H / Hph;
                    alfa = 0.9 / Hph;
                    Hmin = FA26 * Math.Abs(t);
                    if (Math.Abs(h1) < Hmin && !FStKr) Blad = 3;
                    else
                    {
                        if (Hph <= 1)
                        {
                            for (i = 0; i <= FN; i++) Y[i] = XX1[i];
                            t = t + H;
                            Y[0] = t; XX1[0] = t;
                            Dopisz(Y);
                            if (FPG != null)
                            { FPG.Value=(int)Math.Floor(Skala * (t - FTp)); }
                            if (KKx == 0 && !FStKr)
                            {
                                if (alfa < 5 && alfa >= 0.9) H *= alfa;
                                else if (alfa >= 5) H *= 5.0;
                            }
                            else if ((KKx > 0 && alfa < 1) && !FStKr) H *= alfa;
                            KKx = 0;
                        }
                        else
                        {
                            KKx++;
                            if (!FStKr) H *= alfa; else Blad = 4;
                        }
                        if (Blad == 0)
                        {
                            h1 = FTk - t;
                            if ((h1 < H && H > 0) && !FStKr) H = h1;
                            else if ((h1 > H && H < 0) && !FStKr) H = h1;
                        }
                    }
                }
                while (!(t >= FTk || Blad != 0));
            else return 2;
            return Blad;

        }

        public TFehlbergAbstractW()
            : base()
        { }

        /// <summary>
        ///Konstruktor obiektu typu TFehlbergAbstract o parametrach: 
        /// </summary>
        /// <param name="N"> liczba równań</param>
        /// <param name="Tp1">czas początkowy całkowania</param>
        /// <param name="Tk1">czas końcowy całkowania</param>
        /// <param name="X0">wektor zawierający warunek początkowy</param>
        public TFehlbergAbstractW(int N, double Tp1, double Tk1, double[] X0, System.Windows.Forms.ProgressBar PG)
            : base(N, Tp1, Tk1, X0,PG)
        { }
    }//Koniec TFehlbergAbstractW

    //Klasa potomna TFehlberg klasy TFehlbergAbstract do rozwiązywania układów równań
    //różniczkowych metodą Fehlberga
    public class TFehlbergW : TFehlbergAbstractW
    {
        private
         FunNonLinearDiffEquationDelegateD FProcRoRoD;
         FunNonLinearDiffEquationDelegateWD FProcRoRoWD;
        int Wybor = -1;
        public
          override void FunNonLinearDiffEquation(double[] F, double[] X, double t)
        {
            if (Wybor == 1) FProcRoRoD(F, X, t);
            else if (Wybor == 2)
            {
                double[] F1 = new double[FN + 1];
                F1 = FProcRoRoWD(X, t);
                for (int i = 0; i <= FN; i++) F[i] = F1[i];
            }
            else F = null;
        }
        public TFehlbergW()
            : base()
        { Wybor = 0; }
        //ProcRR - egzemplarz delegata FunNonLinearDiffEquationDelegateD do zadawania prawej strony równania
        //         różniczkowego w postaci normalnej
        //N    - liczba równań
        //Tp   - czas początkowy obliczeń
        //Tk   - czas końcowy obliczeń
        //X0   - wektor zawierający warunek początkowy
        public TFehlbergW(FunNonLinearDiffEquationDelegateD ProcRR, int N, double Tp1,
               double Tk1, double[] X0, System.Windows.Forms.ProgressBar PG)
            : base(N, Tp1, Tk1, X0,PG)
        {
            FProcRoRoD = ProcRR;
            Wybor = 1;
        }

        public TFehlbergW(FunNonLinearDiffEquationDelegateWD ProcRRWD, int N, double Tp1,
               double Tk1, double[] X0, System.Windows.Forms.ProgressBar PG)
            : base(N, Tp1, Tk1, X0,PG)
        {
            FProcRoRoWD = ProcRRWD;
            Wybor = 2;
        }
       
    }
    //Koniec 


}
