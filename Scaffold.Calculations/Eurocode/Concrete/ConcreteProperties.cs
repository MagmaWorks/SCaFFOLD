using System.Collections.Generic;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Calculations.Eurocode.Concrete.Utility;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Calculations.Eurocode.Concrete
{
    public class ConcreteProperties : ICalculation
    {
        public string ReferenceName { get; set; } = "";
        public string CalculationName { get; set; } = "Concrete Material Properties";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue("Grd", "Grade")]
        public CalcSelectionList ConcreteGrade { get; set; }
            = new CalcSelectionList("Concrete Grade", "C30/37", EnumSelectionListParser.ConcreteGrades);

        [OutputCalcValue("CP", "Concrete Properties")]
        public ConcreteMaterialProperties ConcreteMaterialProperties { get; private set; }

        [OutputCalcValue("f_{ck}", "Characteristic cylinder strength")]
        public CalcStress fck => ConcreteMaterialProperties.fck;

        [OutputCalcValue("f_{ck,cube}", "Characteristic cube strength")]
        public CalcStress fckcube => ConcreteMaterialProperties.fckcube;

        [OutputCalcValue("f_{cm}", "Mean cylinder strength")]
        public CalcStress fcm => ConcreteMaterialProperties.fcm;

        [OutputCalcValue("f_{ctm}", "Mean tensile strength")]
        public CalcStress fctm => ConcreteMaterialProperties.fctm;

        [OutputCalcValue("f_{ctk;0.05}", "Tensile strength 5% fractile")]
        public CalcStress fctk005 => ConcreteMaterialProperties.fctk005;

        [OutputCalcValue("f_{ctk;0.95}", "Tensile strength 95% fractile")]
        public CalcStress fctk095 => ConcreteMaterialProperties.fctk095;

        [OutputCalcValue("E_{cm}", "Secant modulus of elasticity")]
        public CalcStress Ecm => ConcreteMaterialProperties.Ecm;

        [OutputCalcValue("ε_{c1}", "Nominal peak strain")]
        public CalcStrain Epsilonc1 => ConcreteMaterialProperties.Epsilonc1;

        [OutputCalcValue("ε_{cu1}", "Nominal ultimate strain")]
        public CalcStrain Epsiloncu1 => ConcreteMaterialProperties.Epsiloncu1;

        [OutputCalcValue("ε_{c2}", "Simplified parabola-rectangle peak strain")]
        public CalcStrain Epsilonc2 => ConcreteMaterialProperties.Epsilonc2;

        [OutputCalcValue("ε_{cu2}", "Simplified ultimate strain")]
        public CalcStrain Epsiloncu2 => ConcreteMaterialProperties.Epsiloncu2;

        [OutputCalcValue(@"\textit{n}", "Exponent")]
        public CalcDouble n => ConcreteMaterialProperties.n;

        [OutputCalcValue("ε_{c3}", "Simplified bi-linear peak strain")]
        public CalcStrain Epsilonc3 => ConcreteMaterialProperties.Epsilonc3;

        [OutputCalcValue("ε_{cu3}", "Simplified ultimate strain")]
        public CalcStrain Epsiloncu3 => ConcreteMaterialProperties.Epsiloncu3;

        public ConcreteProperties()
        {
            Calculate();
        }

        public IList<IFormula> GetFormulae()
        {
            return new List<IFormula>();
        }

        public void Calculate()
        {
            ConcreteMaterialProperties = new Utility.ConcreteMaterialProperties(
                ConcreteGrade.GetEnum<EnConcreteGrade>("/", "_"));
        }
    }
}
