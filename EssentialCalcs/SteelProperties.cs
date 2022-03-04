using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EssentialCalcs
{
    public class SteelProperties
    {
        /// Density of steel
        public double d { get; private set; }

        /// Weight of steel
        public double w { get; private set; }

        /// Modulus of elasticity (Young's modulus)
        public double Es { get; private set; }

        /// Shear modulus
        public double G { get; private set; }

        /// Yield strength
        public double fy { get; private set; }

        /// Ultimate strength
        public double fu { get; private set; }

        /// Poissons ratio
        public double v { get; private set; }

        /// Coefficient of lnear thermal expansion
        public double a { get; private set; }
     



        public double Ecm { get; private set; }

        public double Epsilonc1 { get; private set; }

        public double Epsiloncu1 { get; private set; }

        public double Epsilonc2 { get; private set; }

        public double Epsiloncu2 { get; private set; }

        public double n { get; private set; }

        public double Epsilonc3 { get; private set; }

        public double Epsiloncu3 { get; private set; }


        private SteelProperties(
            double d, 
            double w, 
            double Es, 
            double G, 
            double fy, 
            double fu,
            double v,            
            double a)
        {
            this.d = d;
            this.w = w;
            this.Es = Es;
            this.G = G;
            this.fy = fy;
            this.fu = fu;
            this.v = v;
            this.a = a;
            
        }

        public static SteelProperties ByGrade(string charStr)
        {
            double d = double.NaN;
            double w = double.NaN;
            double Es = double.NaN;
            double G = double.NaN;
            double fy = double.NaN;
            double fu = double.NaN;
            double v = double.NaN;
            double a = double.NaN;
            

            switch (charStr)
            {
                case "235":
                    fy = 235; //MPa
                    fu = 360; //MPa
                    break;
                case "275":
                    fy = 275;
                    fu = 430;
                    break;
                case "355":
                    fy = 355;
                    fu = 490;
                    break;
                case "450":
                    fy = 440;
                    fu = 550;
                    break;

                default:
                    fy = double.NaN;
                    fu = double.NaN;
                    break;
            }

            d = 7850;
            w = d * 9.807 / 1000; ;
            Es = 210000;
            v = 0.3;
            G = Es / (2 * (1 + v));
            a = Math.Pow(12, -6);

            return new SteelProperties(d, w, Es, G, fy, fu, v, a);
        }
    }
}
