using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSBibliotekaStudent.EkstraCalkaRozniczka
{
    public abstract class GradientFunSkalarWektorAbstr : EkstrapolacjaAbstr
    {
        public double[] X;      //Punkt obliczenia gradientu
        private double[] X0;
        public double[] gradFun;
        protected int TypPochodnej;
        public abstract double FunSkalarWektor(double[] X);
        private int k=0;

        //----------------------------------------------------------------
         public GradientFunSkalarWektorAbstr():base() { }

         //Konstruktor
         //X - punkt, w którym obliczamy pochodną ;
         //h0 - wstępnie ustalony krok różniczkowania
         //q - iloraz próbnych kroków różniczkowania ; q>1
         //eps - dokładność iteracji różniczkowania
         public GradientFunSkalarWektorAbstr(double[] X, double h0, double q, double eps, int maxit)
             : base(h0, q, eps, maxit)
         {
             this.X=X;
             TypPochodnej=0;
         }

         //Funkcja FunSkalarWektor rzeczywista zmiennej wektorowej powinna 
        //implementować obliczanie gradientu
        public override double EkstapolacjaProcesu(double h)
        {
            double f1, f2, f3, f4;
            X0 = X;
            switch (TypPochodnej)
            {
                case 1: //rzędu drugiego - pierwsza pochodna wzór (3.36)
                    {
                        X0[k] += h;
                        f1 = FunSkalarWektor(X0);
                        X0[k] -= 2.0*h;
                        f2 = FunSkalarWektor(X0);
                        return 0.5 * (f1 - f2) / h;
                    }
                case 2: //rzędu czwartego - pierwsza pochodna   wzór (3.44)
                    {
                        //h2 = 2 * h;
                        X0[k] += 2.0 * h;
                        f1 = FunSkalarWektor(X0);//X0 + h2
                        X0[k] -= h;
                        f2 = FunSkalarWektor(X0);//X0 + h
                        X0[k] -= 2.0 * h;
                        f3 = FunSkalarWektor(X0);//X0 - h
                        X0[k] -= h;
                        f4 = FunSkalarWektor(X0);//X0 - h2
                        return (8 * f2 - 8 * f3 - f1 + f4) / (12 * h);
                    }
                 default: return 0;
            }
        }

        protected void EkstrapolacjaGradientu()
        {
            int blad;
            gradFun = new double[X.Length];
            for (k = 1; k <= X.Length - 1; k++)
            {
                blad = EkstrapolacjaAitkena();
                gradFun[k] = F0;
                if (blad != 0)
                {
                    //F0 = 0;
                    //throw new Exception("Algorytm EkstrapolacjaAitkena() zgłasza błąd w metodzie EkstrapolacjaRozniczkowa");
                    PiszKomunikat(blad);
                }
            }
        }

        //Gradient obliczany ilorazem różniczkowym cetralnym drugiego rzędu 
        public double[] GradientFunSkalWekR2()
        {
            TypPochodnej = 1;
            EkstrapolacjaGradientu();
            return gradFun;
        }

        //Gradient obliczany ilorazem różniczkowym cetralnym czwartego rzędu 
        public double[] GradientFunSkalWekR4()
        {
            TypPochodnej = 2;
            EkstrapolacjaGradientu();
            return gradFun;
        }
    }

    //-----------------------------------------------
    public class TGradientFunSkalarWektor : GradientFunSkalarWektorAbstr
    {

        public FunWektorSkalarDelegate FunSkalWek;
        public TGradientFunSkalarWektor(FunWektorSkalarDelegate FSW, double[] X, double h0, double q, double eps, int maxit)
            : base(X, h0, q, eps, maxit)
        {
            FunSkalWek = FSW;
        }
        public override double FunSkalarWektor(double[] X)
        {
            return FunSkalWek(X);
        }

    }//TGradientFunSkalarWektor


}
