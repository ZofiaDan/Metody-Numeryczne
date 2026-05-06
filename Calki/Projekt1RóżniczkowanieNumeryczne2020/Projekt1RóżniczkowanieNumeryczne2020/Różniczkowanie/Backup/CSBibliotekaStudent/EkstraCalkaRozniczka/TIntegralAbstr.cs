using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSBibliotekaStudent.EkstraCalkaRozniczka
{
    public abstract class TIntegralAbstr : EkstrapolacjaAbstr
    {
       protected double a; //Punkt początkowy całkowania
       protected double  b; //Punkt końcowy całkowania
       protected double  h; //Krok całkowania wstępnie ustalony przez konstruktora
       protected int N; //Liczba kroków całkowania
       protected int M; //Liczba par kroków całkowania w metodzie Simpsona

       protected int TypWzoruCalkowego; //FTypWzoruCalkowego przyjmuje wartość :
                                 //1 - dla metody trapezów
                                 //2 - dla metody Simpsona
                                 //3 - dla metody prostokątów

       // Funkcja poddawana operacji różniczkowania
       public abstract  double FunRealReal(double x);
             
       //Wzory całkowania numerycznego tj. wzory trapezów lub wzory Simpsona
       public override double EkstapolacjaProcesu(double h)
       {
          int i;
          double Suma,Suma1,Suma2,xi,xip,xin,h2;
          switch (TypWzoruCalkowego)
          {
              case 1:   //Wzór trapezów  (3.24)
                  {
                    Suma=0.5*FunRealReal(a)+0.5*FunRealReal(b);
                    N=(int)Math.Round((b-a)/h);     h=(b-a)/N;
                    for (i=1; i<=N-1;i++)
                      { xi=a+i*h;  Suma+=FunRealReal(xi);}
                    return Suma*h;
                  }
               case 2:  //Wzór Simpsona (3.30)
                 {
                     N=(int)Math.Round((b-a)/h);
                     if (N%2!=0) N++;
                     M=(int)Math.Round((float)N/2.0);
                     h=(b-a)/N;
                     Suma1=0;    Suma2=0;
                     for (i=1; i<=M-1; i++)
                       {xip=a+2*i*h;      Suma2+=FunRealReal(xip); };
                     for (i=0; i<=M-1; i++)
                       {xin=a+(2*i+1)*h;  Suma1+=FunRealReal(xin); };
                     return h*(FunRealReal(a)+FunRealReal(b)+4.0*Suma1+2.0*Suma2)/3.0;
                  }
               case 3: //Wzór prostokątów  (3.16)
                 {
                     Suma=0;
                     N=(int)Math.Round((b-a)/h); h=(b-a)/N;  h2=h/2;
                     for (i=1; i<=N; i++)
                     {
                        xi=a+i*h-h2;
                        Suma+=FunRealReal(xi);
                     }
                    return Suma*h ;
                  }
               default: return 0;
            }

       }
       public TIntegralAbstr():base()
       { }
         

         //Konstruktor dla szczególnej ekstrapolacji Richardsona dla  błędu obcięcia
         //F(h)-F(0) = b1*(h)^p
         //Stosowany dla obliczeń niezależnych
         //Fun - funkcja całkowana w przedziale Fa,Fb niezależna
         //a - punkt początkowy całkowania
         //b - punkt końcowy całkowania
         //h0 - krok całkowania wstępnie ustalony
         //q - iloraz próbnych kroków całkowania ,q>1
         //eps - dokładność iteracji
         public TIntegralAbstr(double a,double b,double h0,
                                    double  q,double  eps, int maxit):base(h0,q,eps,maxit)
         {
            this.a=a; this.b=b;
            TypWzoruCalkowego=0;
         }

      

         //Funkcja implementująca wzór prostokątów (3.24) oraz ekstrapolacji Aitkena (3.12)
         public double MetodaAitkenaDlaProstokatow()
         {
             int blad;
             q = 2; TypWzoruCalkowego = 3;
             blad = EkstrapolacjaAitkena();
             if (blad == 0) return F0;
             else
             {
                 //throw new Exception("Algorytm EkstrapolacjaAitkena zgłasza błąd w metodzie MetodaAitkenaDlaProstokatow()");
                 PiszKomunikat(blad);
                 return F0;
             }
         }

        
         //Funkcja implementująca wzór trapezów (3.24) oraz ekstrapolacji Aitkena (3.12)
         public double MetodaAitkenaDlaTrapezow()
         {
             int blad;
             q = 2; TypWzoruCalkowego = 1;
             blad = EkstrapolacjaAitkena();
             if (blad == 0) return F0;
             else
             {
                 //throw new Exception("Algorytm EkstrapolacjaAitkena zgłasza błąd w metodzie MetodaAitkenaDlaTrapezow()");
                 PiszKomunikat(blad);
                 return F0;
             }
         }

         //Funkcja implementująca wzór trapezów (3.30) oraz ekstrapolacji Aitkena (3.12)
         public double MetodaAitkenaSimpsona()
         {
             int blad;
             q = 2; TypWzoruCalkowego = 2;
             blad = EkstrapolacjaAitkena();
             if (blad == 0) return F0;
             else
             {
                 //throw new Exception("Algorytm EkstrapolacjaAitkena zgłasza błąd w metodzie MetodaAitkenaSimpsona()");
                 PiszKomunikat(blad);
                 return F0;
             }
         }

    }

    public class TIntegral : TIntegralAbstr
    {
        public FunkcjaRealeReale FunReRe;
        //double a,double b,double h0, double  q,double  eps, int maxit
        public TIntegral(FunkcjaRealeReale FRR, double a, double b,double h0, double q, double eps, int maxit)
            : base(a,b, h0, q, eps, maxit)
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
