using System.Collections.Generic;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Calculations.Eurocode.En1992_1_1;
using Scaffold.Calculations.Utility;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Calculations
{
    public class ConcretePropertiesCalculation : ICalculation
    {
        public string ReferenceName { get; set; } = "";
        public string CalculationName { get; set; } = "Concrete Material Properties";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue("Grd", "Grade")]
        public CalcSelectionList ConcreteGrade { get; set; }
            = new CalcSelectionList("Concrete Grade", "C30/37", EnumToFromSelectionList.ConcreteGrades);
        [InputCalcValue("Grd", "Grade")]
        public CalcSelectionList NationalAnnex { get; set; }
            = new CalcSelectionList("Concrete Grade", "C30/37", EnumToFromSelectionList.NationalAnnexes);

        [OutputCalcValue(@"CP", "Concrete Properties")]
        public ConcreteProperties ConcreteProperties { get; private set; }

        [OutputCalcValue(@"fck", "Characteristic cylinder strength")]
        public CalcStress fck => ConcreteProperties.fck;

        [OutputCalcValue(@"fck,cube", "Characteristic cube strength")]
        public CalcStress fckcube => ConcreteProperties.fckcube;

        [OutputCalcValue(@"fcm", "Mean cylinder strength")]
        public CalcStress fcm => ConcreteProperties.fcm;

        [OutputCalcValue(@"fctm", "Mean tensile strength")]
        public CalcStress fctm => ConcreteProperties.fctm;

        [OutputCalcValue(@"fctk,0.05", "Tensile strength 5% fractile")]
        public CalcStress fctk005 => ConcreteProperties.fctk005;

        [OutputCalcValue(@"fctk,0.95", "Tensile strength 95% fractile")]
        public CalcStress fctk095 => ConcreteProperties.fctk095;

        [OutputCalcValue(@"Ecm", "Secant modulus of elasticity")]
        public CalcStress Ecm => ConcreteProperties.Ecm;

        [OutputCalcValue(@"εc1", "Nominal peak strain")]
        public CalcStrain Epsilonc1 => ConcreteProperties.Epsilonc1;

        [OutputCalcValue(@"εcu1", "Nominal ultimate strain")]
        public CalcStrain Epsiloncu1 => ConcreteProperties.Epsiloncu1;

        [OutputCalcValue(@"εc2", "Simplified parabola-rectangle peak strain")]
        public CalcStrain Epsilonc2 => ConcreteProperties.Epsilonc2;

        [OutputCalcValue(@"εcu2", "Simplified ultimate strain")]
        public CalcStrain Epsiloncu2 => ConcreteProperties.Epsiloncu2;

        [OutputCalcValue(@"n", "Exponent")]
        public CalcDouble n => ConcreteProperties.n;
        [OutputCalcValue(@"εc3", "Simplified bi-linear peak strain")]
        public CalcStrain Epsilonc3 => ConcreteProperties.Epsilonc3;

        [OutputCalcValue(@"εcu2", "Simplified ultimate strain")]
        public CalcStrain Epsiloncu3 => ConcreteProperties.Epsiloncu3;

        public ConcretePropertiesCalculation()
        {
            Calculate();
        }

        public IList<IFormula> GetFormulae()
        {
            return new List<IFormula>();
        }

        public void Calculate()
        {
            ConcreteProperties = new ConcreteProperties(
                ConcreteGrade.GetEnum<EnConcreteGrade>("/", "_"), NationalAnnex.GetEnum<NationalAnnex>());
        }
    }
}
