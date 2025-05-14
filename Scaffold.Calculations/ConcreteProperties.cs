using System;
using System.Xml.Linq;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Core.CalcQuantities;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations.Eurocode.En1992_1_1
{
    public class ConcreteProperties
    {
        public EnConcreteMaterial Material { get; }
            = new EnConcreteMaterial(EnConcreteGrade.C30_37, NationalAnnex.UnitedKingdom);

        private Pressure GetStrength() => new Pressure(
            double.Parse(Material.Grade.ToString().Split('C', '_')[1]), PressureUnit.Megapascal);

        /// <summary>
        /// Characteristic cylinder strength
        /// </summary>
        public CalcStress fck
            => new CalcStress(GetStrength(), "Characteristic cylinder strength", "f_ck");

        /// <summary>
        /// Characteristic cube strength
        /// </summary>
        public CalcStress fckcube => new CalcStress(
            new Pressure(double.Parse(Material.Grade.ToString().Split('_')[1]), PressureUnit.Megapascal),
            "Characteristic cube strength", "f_ck,cube");

        /// <summary>
        /// Mean cylinder strength
        /// </summary>
        public CalcStress fcm => new CalcStress(GetStrength() + new Pressure(8, PressureUnit.Megapascal),
                "Mean cylinder strength", "f_cm");

        /// <summary>
        /// Mean tensile strength
        /// </summary>
        public CalcStress fctm => new CalcStress(fck <= 50
                    ? 0.3 * Math.Pow(fck, 2d / 3d)
                    : 2.12 * Math.Log(1 + (fcm / 10)),
            PressureUnit.Megapascal, "Mean tensile strength", "f_ctm");

        /// <summary>
        /// Tensile strength 5% fractile
        /// </summary>
        public CalcStress fctk005 => new CalcStress(0.7 * fctm,
            PressureUnit.Megapascal, "Tensile strength 5% fractile", "f_ctk;0.05");

        /// <summary>
        /// Tensile strength 95% fractile
        /// </summary>
        public CalcStress fctk095 => new CalcStress(1.3 * fctm,
            PressureUnit.Megapascal, "Tensile strength 95% fractile", "f_ctk;0.95");

        /// <summary>
        /// Secant modulus of elasticity 
        /// </summary>
        public CalcStress Ecm => new CalcStress(22 * Math.Pow(fctm / 10, 0.3),
            PressureUnit.Megapascal, "Secant modulus of elasticity", "E_cm");

        /// <summary>
        /// Compressive strain in the concrete at the peak stress fc
        /// </summary>
        public double Epsilonc1 => new CalcStrain(Math.Max(2.8, 0.7* Math.Pow(fcm, 0.31)),
            RatioUnit.PartPerThousand, "Peak compressive strain", "ε_1");

        public double Epsiloncu1 { get; private set; }

        public double Epsilonc2 { get; private set; }

        public double Epsiloncu2 { get; private set; }

        public double n { get; private set; }

        public double Epsilonc3 { get; private set; }

        public double Epsiloncu3 { get; private set; }

        private ConcreteProperties(
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

        public static ConcreteProperties ByGrade(string charStr)
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

            return new ConcreteProperties(fck, fckcube, fcm, fctm, fctk005, fctk095, Ecm, Epsilonc1, Epsiloncu1, Epsilonc2, Epsiloncu2, n, Epsilonc3, Epsiloncu3);
        }
    }
}
