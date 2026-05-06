using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSBibliotekaStudent.EkstraCalkaRozniczka
{
    public abstract class TDifferentialAbstr : EkstrapolacjaAbstr
    {

     
         protected double X0;      //Współrzędne obliczenia pochodnej
         protected int TypPochodnej;
         
         protected void EkstrapolacjaRozniczkowa()
         {
             int blad;
             q = 2;
             blad = EkstrapolacjaAitkena();
             if (blad != 0)
             {
                //F0 = 0;
                //throw new Exception("Algorytm EkstrapolacjaAitkena() zgłasza błąd w metodzie EkstrapolacjaRozniczkowa");
                PiszKomunikat(blad);
             }
         }

         // Funkcja poddawana operacji różniczkowania
         public abstract double FunRealReal(double x);

         //Funkcja FunRealReal rzeczywista zmiennej rzeczywistej powinna 
         //implementować wzory różniczkowania
         public override double EkstapolacjaProcesu(double h)
         {
            double f1,f2,f3,f4,f5,f6,h2,h3;
            switch (TypPochodnej)
            {
              case 1: //rzędu drugiego - pierwsza pochodna wzór (3.36)
                 {
                   f1=FunRealReal(X0+h);  f2=FunRealReal(X0-h);
                   return 0.5*(f1-f2)/h;
                }
              case 2: //rzędu drugiego - druga pochodna   wzór (3.40)
                {
                   f1=FunRealReal(X0+h);  f2=FunRealReal(X0-h);  f3=FunRealReal(X0);
                   return (f1-2*f3+f2)/(h*h);
                }
              case 3: //rzędu drugiego - trzecia pochodna   wzór (3.42)
                {
                   h2=2*h;
                   f1=FunRealReal(X0+h2);  f2=FunRealReal(X0+h);  f3=FunRealReal(X0-h);
                   f4=FunRealReal(X0-h2);
                   return 0.5*(f1-2*f2+2*f3-f4)/(h*h);
                }
              case 4: //rzędu czwartego - pierwsza pochodna   wzór (3.44)
                {
                   h2=2*h;
                   f1=FunRealReal(X0+h2);  f2=FunRealReal(X0+h);  
                   f3=FunRealReal(X0-h);  f4=FunRealReal(X0-h2);
                   return (8*f2-8*f3-f1+f4)/(12*h);
                }
              case 5://rzędu czwartego - druga pochodna      wzór (3.45)
                {
                   h2=2*h;
                   f1=FunRealReal(X0+h2);  f2=FunRealReal(X0+h);  f3=FunRealReal(X0-h);
                   f4=FunRealReal(X0-h2);  f5=FunRealReal(X0);
                   return (-f1+16*f2-30*f5+16*f3-f4)/(12*h*h);
                }
              case 6://rzędu czwartego - trzecia pochodna   wzór (3.46)
                {
                   h2=2*h;   h3=3*h;
                   f1=FunRealReal(X0+h2);  f2=FunRealReal(X0+h);
                   f3=FunRealReal(X0-h);  f4=FunRealReal(X0-h2);
                   f5=FunRealReal(X0+h3);  f6=FunRealReal(X0-h3);
                   return (-f5+8*f1-13*f2+13*f3-8*f4+f6)/(8*h*h*h);
                }
              default: return 0;
          }
         }
         //----------------------------------------------------------------
         public TDifferentialAbstr():base() { }

         //Konstruktor
         //x0 - punkt, w którym obliczamy pochodną ;
         //h0 - wstępnie ustalony krok różniczkowania
         //q - iloraz próbnych kroków różniczkowania ; q>1
         //eps - dokładność iteracji różniczkowania
         public TDifferentialAbstr(double x0, double h0,double q, double eps, int maxit):base(h0,q,eps,maxit)
         {
             X0=x0;
             TypPochodnej=0;
         }

         //Pierwsza pochodna dla rzędu błędu obcięcia 2  ;wzór (3.36)
         public double PierwszPochodnaR2()
         {
             TypPochodnej = 1;
             EkstrapolacjaRozniczkowa();
             return F0;
         }
         //Druga pochodna dla rzędu błędu obcięcia 2   ;wzór (3.40)
         public double DrugaPochodnaR2()
         {
             TypPochodnej = 2;
             EkstrapolacjaRozniczkowa();
             return F0;
         }
         ////Trzecia pochodna dla rzędu błędu obcięcia 2  ;wzór (3.42)
         public double TrzeciaPochodnaR2()
         {
             TypPochodnej = 3;
             EkstrapolacjaRozniczkowa();
             return F0;
         }
         ////Pierwsza pochodna dla rzędu błędu obcięcia 4  ;wzór (3.44)
         public double PierwszPochodnaR4()
         {
             TypPochodnej = 4;
             EkstrapolacjaRozniczkowa();
             return F0;
         }
         ////Druga pochodna dla rzędu błędu obcięcia 4   ;wzór (3.45)
         public double DrugaPochodnaR4()
         {
             TypPochodnej = 5;
             EkstrapolacjaRozniczkowa();
             return F0;
         }
         ////Trzecia pochodna dla rzędu błędu obcięcia 4  ;wzór (3.46)
         public double TrzeciaPochodnaR4()
         {
             TypPochodnej = 6;
             EkstrapolacjaRozniczkowa();
             return F0;
         }
    }

    //-----------------------------------------------
    public class TDifferential : TDifferentialAbstr
    {
        
        public FunkcjaRealeReale FunReRe;
        public TDifferential(FunkcjaRealeReale FRR ,double x0, double h0, double q, double eps, int maxit)
            : base(x0, h0, q, eps, maxit)
        { 
            FunReRe = FRR; 
        }
        public override double FunRealReal(double x)
        {
            return FunReRe(x); 
        }


    }//
    //---------------------------------------------------------

}
