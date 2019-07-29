using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCalcs
{
    public class ConcProperties
    {
        /// <summary>
        /// Characteristic cylinder strength
        /// </summary>
        public double fck { get; private set; }
        /// <summary>
        /// Mean cylinder strength
        /// </summary>
        public double fcm { get; private set; }
        /// <summary>
        /// Secant modulus of elasticity 
        /// </summary>
        public double Ecm { get; private set; }
        /// <summary>
        /// Tensile strength 5% fractile
        /// </summary>
        public double fctk005 { get; private set; }

        private ConcProperties(double fck, double fcm, double Ecm, double fctk005)
        {
            this.fck = fck;
            this.fcm = fcm;
            this.Ecm = Ecm;
            this.fctk005 = fctk005;
        }

        public static ConcProperties ByGrade(string charStr)
        {
            double fcm = 0;
            double fck = 0;
            double Ecm = 0;
            double fctk005 = 0;
            switch (charStr)
            {
                case "30":
                    fck = 30;
                    fcm = 38;
                    Ecm = 33;
                    fctk005 = 2;
                    break;
                case "35":
                    fck = 35;
                    fcm = 43;
                    Ecm = 34;
                    fctk005 = 2.2;
                    break;
                case "40":
                    fck = 40;
                    fcm = 48;
                    Ecm = 35;
                    fctk005 = 2.5;
                    break;
                case "45":
                    fck = 45;
                    fcm = 53;
                    Ecm = 36;
                    fctk005 = 2.7;
                    break;
                case "50":
                    fck = 50;
                    fcm = 58;
                    Ecm = 37;
                    fctk005 = 2.9;
                    break;
                case "55":
                    fck = 55;
                    fcm = 63;
                    Ecm = 38;
                    fctk005 = 3;
                    break;
                case "60":
                    fck = 60;
                    fcm = 68;
                    Ecm = 39;
                    fctk005 = 3.1;
                    break;
                case "70":
                    fck = 70;
                    fcm = 78;
                    Ecm = 41;
                    fctk005 = 3.2;
                    break;
                case "80":
                    fck = 80;
                    fcm = 88;
                    Ecm = 42;
                    fctk005 = 3.4;
                    break;
                case "90":
                    fck = 90;
                    fcm = 98;
                    Ecm = 44;
                    fctk005 = 3.5;
                    break;
                default:
                    fck = double.NaN;
                    fcm = double.NaN;
                    Ecm = double.NaN;
                    fctk005 = double.NaN;
                    break;
            }
            return new ConcProperties(fck, fcm, Ecm, fctk005);
        }


    }
}
