// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Calculations
{
    public class ConcProperties
    {
        /// <summary>
        /// Characteristic cylinder strength
        /// </summary>
        public double fck { get; private set; }
        /// <summary>
        /// Characteristic cube strength
        /// </summary>
        public double fckcube { get; private set; }
        /// <summary>
        /// Mean cylinder strength
        /// </summary>
        public double fcm { get; private set; }

        /// <summary>
        /// Mean tensile strength
        /// </summary>
        public double fctm { get; private set; }

        /// <summary>
        /// Tensile strength 5% fractile
        /// </summary>
        public double fctk005 { get; private set; }

        public double fctk095 { get; private set; }

        /// <summary>
        /// Secant modulus of elasticity 
        /// </summary>
        public double Ecm { get; private set; }

        public double Epsilonc1 { get; private set; }

        public double Epsiloncu1 { get; private set; }

        public double Epsilonc2 { get; private set; }

        public double Epsiloncu2 { get; private set; }

        public double n { get; private set; }

        public double Epsilonc3 { get; private set; }

        public double Epsiloncu3 { get; private set; }


        private ConcProperties(
            double fck,
            double fckcube,
            double fcm,
            double fctm,
            double fctk005,
            double fctk095,
            double Ecm,
            double Epsilonc1,
            double Epsiloncu1,
            double Epsilonc2,
            double Epsiloncu2,
            double n,
            double Epsilonc3,
            double Epsiloncu3)
        {
            this.fck = fck;
            this.fckcube = fckcube;
            this.fcm = fcm;
            this.fctm = fctm;
            this.fctk005 = fctk005;
            this.fctk095 = fctk095;
            this.Ecm = Ecm;
            this.Epsilonc1 = Epsilonc1;
            this.Epsiloncu1 = Epsiloncu1;
            this.Epsilonc2 = Epsilonc2;
            this.Epsiloncu2 = Epsiloncu2;
            this.n = n;
            this.Epsilonc3 = Epsilonc3;
            this.Epsiloncu3 = Epsiloncu3;
        }

        public static ConcProperties ByGrade(string charStr)
        {
            double fck = double.NaN;
            double fckcube = double.NaN;
            double fcm = double.NaN;
            double fctm = double.NaN;
            double fctk005 = double.NaN;
            double fctk095 = double.NaN;
            double Ecm = double.NaN;
            double Epsilonc1 = double.NaN;
            double Epsiloncu1 = double.NaN;
            double Epsilonc2 = double.NaN;
            double Epsiloncu2 = double.NaN;
            double n = double.NaN;
            double Epsilonc3 = double.NaN;
            double Epsiloncu3 = double.NaN;

            switch (charStr)
            {
                case "30":
                    fck = 30;
                    fckcube = 37;
                    break;
                case "35":
                    fck = 35;
                    fckcube = 45;
                    break;
                case "40":
                    fck = 40;
                    fckcube = 50;
                    break;
                case "45":
                    fck = 45;
                    fckcube = 55;
                    break;
                case "50":
                    fck = 50;
                    fckcube = 60;
                    break;
                case "55":
                    fck = 55;
                    fckcube = 67;
                    break;
                case "60":
                    fck = 60;
                    fckcube = 75;
                    break;
                case "70":
                    fck = 70;
                    fckcube = 85;
                    break;
                case "80":
                    fck = 80;
                    fckcube = 95;
                    break;
                case "90":
                    fck = 90;
                    fckcube = 105;
                    break;
                default:
                    fck = double.NaN;

                    break;
            }
            fcm = fck + 8;

            if (fck <= 50)
            {
                fctm = 0.3 * Math.Pow(fck, 2d / 3d);
            }
            else
            {
                fctm = 2.12 * Math.Log(1 + (fcm / 10));
            }

            fctk005 = 0.7 * fctm;

            fctk095 = 1.3 * fctm;

            Ecm = 22 * Math.Pow(fcm / 10, 0.3);

            Epsilonc1 = Math.Min(0.7 * Math.Pow(fcm, 0.31), 2.8);

            if (fck > 50)
            {
                Epsiloncu1 = 2.8 + 27 * Math.Pow((98 - fcm) / 100, 4);
                Epsilonc2 = 2 + 0.085 * Math.Pow(fck - 50, 0.53);
                Epsiloncu2 = 2.6 + 35 * Math.Pow((90 - fck) / 100, 4);
                n = 1.4 + 23.4 * Math.Pow((90 - fck) / 100, 4); ;
                Epsilonc3 = 1.75 + 0.55 * ((fck - 50) / 40);
                Epsiloncu3 = 2.6 + 35 * Math.Pow((90 - fck) / 100, 4);
            }
            else
            {
                Epsiloncu1 = 3.5;
                Epsilonc2 = 2;
                Epsiloncu2 = 3.5;
                n = 2;
                Epsilonc3 = 1.75;
                Epsiloncu3 = 3.5;
            }

            return new ConcProperties(fck, fckcube, fcm, fctm, fctk005, fctk095, Ecm, Epsilonc1, Epsiloncu1, Epsilonc2, Epsiloncu2, n, Epsilonc3, Epsiloncu3);
        }


    }
}
