using System;
using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations.Eurocode.Concrete.Utility
{
    public class ConcreteMaterialProperties
    {
        public EnConcreteMaterial Material { get; }
            = new EnConcreteMaterial(EnConcreteGrade.C30_37, NationalAnnex.UnitedKingdom);

        public IParabolaRectangleMaterial ParabolaRectangleMaterialModel
            => EnConcreteFactory.CreateParabolaRectangleAnalysisMaterial(Material.Grade);
        public ILinearElasticMaterial LinearElasticMaterialModel
            => EnConcreteFactory.CreateLinearElastic(Material.Grade);

        private Pressure GetStrength() => new Pressure(
            double.Parse(Material.Grade.ToString().Split('C', '_')[1]), PressureUnit.Megapascal);

        /// <summary>
        /// Characteristic cylinder strength
        /// </summary>
        public CalcStress fck
            => new CalcStress(GetStrength(), "Characteristic cylinder strength", "f_{ck}");

        /// <summary>
        /// Characteristic cube strength
        /// </summary>
        public CalcStress fckcube => new CalcStress(
            new Pressure(double.Parse(Material.Grade.ToString().Split('_')[1]), PressureUnit.Megapascal),
            "Characteristic cube strength", "f_{ck,cube}");

        /// <summary>
        /// Mean cylinder strength
        /// </summary>
        public CalcStress fcm => new CalcStress(GetStrength() + new Pressure(8, PressureUnit.Megapascal),
                "Mean cylinder strength", "f_{cm}");

        /// <summary>
        /// Mean tensile strength
        /// </summary>
        public CalcStress fctm => new CalcStress(fck <= 50
                    ? 0.3 * Math.Pow(fck, 2d / 3d)
                    : 2.12 * Math.Log(1 + fcm / 10),
            PressureUnit.Megapascal, "Mean tensile strength", "f_{ctm}");

        /// <summary>
        /// Tensile strength 5% fractile
        /// </summary>
        public CalcStress fctk005 => new CalcStress(0.7 * fctm,
            PressureUnit.Megapascal, "Tensile strength 5% fractile", "f_{ctk;0.05}");

        /// <summary>
        /// Tensile strength 95% fractile
        /// </summary>
        public CalcStress fctk095 => new CalcStress(1.3 * fctm,
            PressureUnit.Megapascal, "Tensile strength 95% fractile", "f_{ctk;0.95}");

        /// <summary>
        /// Secant modulus of elasticity 
        /// </summary>
        public CalcStress Ecm => new CalcStress(22 * Math.Pow(fcm / 10, 0.3),
            PressureUnit.Gigapascal, "Secant modulus of elasticity", "E_{cm}");

        /// <summary>
        /// Compressive strain in the concrete at the peak stress fc
        /// </summary>
        public CalcStrain Epsilonc1 => new CalcStrain(Math.Min(2.8, 0.7 * Math.Pow(fcm, 0.31)),
            RatioUnit.PartPerThousand, "Nominal peak strain", "ε_{c1}");

        public CalcStrain Epsiloncu1 => new CalcStrain(fck >= 50
                    ? 2.8 + 27.0 * Math.Pow((98 - fcm) / 100, 4)
                    : 3.5,
            RatioUnit.PartPerThousand, "Nominal ultimate strain", "ε_{cu1}");

        public CalcStrain Epsilonc2 => new CalcStrain(fck >= 50
                    ? 2.0 + 0.085 * Math.Pow(fck - 50, 0.53)
                    : 2.0,
            RatioUnit.PartPerThousand, "Simplified parabola-rectangle peak strain", "ε_{c2}");

        public CalcStrain Epsiloncu2 => new CalcStrain(fck >= 50
                    ? 2.6 + 35.0 * Math.Pow((90 - fck) / 100, 4)
                    : 3.5,
            RatioUnit.PartPerThousand, "Simplified ultimate strain", "ε_{cu2}");

        public CalcDouble n => new CalcDouble(fck >= 50
                    ? 1.4 + 23.4 * Math.Pow((90 - fck) / 100, 4)
                    : 2.0,
             "Exponent", @"\textit{n}");

        public CalcStrain Epsilonc3 => new CalcStrain(fck >= 50
                    ? 1.75 + 0.55 * ((fck - 50) / 40)
                    : 1.75,
            RatioUnit.PartPerThousand, "Simplified bi-linear peak strain", "ε_{c3}");

        public CalcStrain Epsiloncu3 => new CalcStrain(fck >= 50
                    ? 2.6 + 35.0 * Math.Pow((90 - fck) / 100, 4)
                    : 3.5,
            RatioUnit.PartPerThousand, "Simplified ultimate strain", "ε_{cu3}");

        public ConcreteMaterialProperties(EnConcreteGrade grade, NationalAnnex nationalAnnex = NationalAnnex.UnitedKingdom)
        {
            Material = new EnConcreteMaterial(grade, nationalAnnex);
        }
    }
}
