using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSBibliotekaStudent.EkstraCalkaRozniczka
{
    public abstract class JacobianFunWekWekAbstr : EkstrapolacjaWektorAbstr
    {
        public double[] X;      //Punkt obliczenia Jacobianu
        private double[] X0;
        public double[,] Jacobian;
        protected int TypPochodnej;
      
        private int k=0;


        public abstract double[] FunWekWek(double[] X);

        //----------------------------------------------------------------
        public JacobianFunWekWekAbstr() : base() { }

        //Konstruktor
         //X - punkt, w którym obliczamy pochodną ;
         //h0 - wstępnie ustalony krok różniczkowania
         //q - iloraz próbnych kroków różniczkowania ; q>1
         //eps - dokładność iteracji różniczkowania
        public JacobianFunWekWekAbstr(double[] X, double h0, double q, double eps, int maxit,int M)
             : base(h0, q, eps, maxit,M)
         {
             this.X=X;
             TypPochodnej=0;
             X0 = new double[X.Length];
         }

        //Funkcja FunSkalarWektor rzeczywista zmiennej wektorowej powinna 
        //implementować obliczanie gradientu
        public override double[] EkstapolacjaProcesuWektor(double h)
        {
            double[] f1, f2, f3, f4;
            //X0 = X;
            for (int i = 1; i < X.Length; i++) X0[i] = X[i];
            double[] F = new double[M + 1];

            switch (TypPochodnej)
            {
                case 1: //rzędu drugiego - pierwsza pochodna wzór (3.36)
                    {
                        X0[k] += h;
                        f1 = FunWekWek(X0);
                        X0[k] -= 2.0 * h;
                        f2 = FunWekWek(X0);
                        for (int i=1; i<=M; i++) F[i]= 0.5 * (f1[i] - f2[i]) / h;
                        return F;
                    }
                case 2: //rzędu czwartego - pierwsza pochodna   wzór (3.44)
                    {
                        //h2 = 2 * h;
                        X0[k] += 2.0 * h;
                        f1 = FunWekWek(X0);//X0 + h2
                        X0[k] -= h;
                        f2 = FunWekWek(X0);//X0 + h
                        X0[k] -= 2.0 * h;
                        f3 = FunWekWek(X0);//X0 - h
                        X0[k] -= h;
                        f4 = FunWekWek(X0);//X0 - h2
                        for (int i = 1; i <= M; i++)
                            F[i] = (8 * f2[i] - 8 * f3[i] - f1[i] + f4[i]) / (12 * h);
                        return F;
                    }
                default: return null;
            }
        }

        protected void EkstrapolacjaJacobianu()
        {
            int blad;
            Jacobian = new double[M+1, X.Length];
            for (k = 1; k <= X.Length-1; k++)
            {
                blad= EkstrapolacjaAitkenaWektor(); 
                for (int j=1; j<=M; j++) Jacobian[j,k] = W0[j];
                if (blad != 0)
                {
                    //F0 = 0;
                    //throw new Exception("Algorytm EkstrapolacjaAitkena() zgłasza błąd w metodzie EkstrapolacjaRozniczkowa");
                    PiszKomunikat(blad);
                }
            }
        }

        //Gradient obliczany ilorazem różniczkowym cetralnym drugiego rzędu 
        public double[,] JacobianR2()
        {
            TypPochodnej = 1;
            EkstrapolacjaJacobianu();
            return Jacobian;
        }

        //Gradient obliczany ilorazem różniczkowym cetralnym czwartego rzędu 
        public double[,] JacobianR4()
        {
            TypPochodnej = 2;
            EkstrapolacjaJacobianu();
            return Jacobian;
        }
    }

    //-----------------------------------------------
    public class TJacobianFunWekWek : JacobianFunWekWekAbstr
    {

        public FunWektorWektorDelegate FunWW;
        public TJacobianFunWekWek(FunWektorWektorDelegate FWW, double[] X, double h0, double q, double eps, int maxit,int M)
            : base(X, h0, q, eps, maxit,M)
        {
            FunWW = FWW;
        }
        public override double[] FunWekWek(double[] X)
        {
            return FunWW(X);
        }

    }//TJacobianFunWekWek
}
