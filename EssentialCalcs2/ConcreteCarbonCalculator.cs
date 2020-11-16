using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;

namespace EssentialCalcs
{
    [CalcName("Concrete Carbon Calculator")]
    public class ConcreteCarbonCalculator : CalcCore.CalcBase
    {
        CalcSelectionList _replacement;
        CalcSelectionList _mix;
        CalcDouble _carbonDensity;
        CalcDouble _carbonDensityWithRebar;
        CalcDouble _rebarDensity;
        CalcDouble _rcVol;
        CalcDouble _rcMass;
        CalcDouble _totalCarbon;
        Dictionary<string,Dictionary<string, double>> carbonData;

        public ConcreteCarbonCalculator()
        {
            InstanceName = "Concrete element";
            _mix = inputValues.CreateCalcSelectionList("Concrete mix", "RC30", new List<string> { "GEN0", "GEN1", "GEN2", "GEN3", "RC20/25", "RC25/30", "RC28/35", "RC32/40", "RC40/50", "PAV1", "PAV2" });
            _replacement = inputValues.CreateCalcSelectionList("Cement replacement", "zero", new List<string> {"zero", "15%PFA", "30%PFA", "25%GGBS", "50%GGBS"});
            _rebarDensity = inputValues.CreateDoubleCalcValue("Rebar density", "", "kg/m^3", 150);
            _rcVol = inputValues.CreateDoubleCalcValue("Volume of RC", "", "m^3", 1);

            _rcMass = outputValues.CreateDoubleCalcValue("Mass of RC", "", "kg", double.NaN);
            _carbonDensity = outputValues.CreateDoubleCalcValue("Carbon density", "", "kg/m^3", double.NaN);
            _carbonDensityWithRebar = outputValues.CreateDoubleCalcValue("RC carbon density", "", "kg/m^3", double.NaN);
            _totalCarbon = outputValues.CreateDoubleCalcValue("Total carbon", "", "T", double.NaN);

            genDictionary();
            UpdateCalc();
        }

        public override List<Formula> GenerateFormulae()
        {
            return new List<Formula>
            {
                Formula.FormulaWithNarrative("This calculation is based on the Inventory of Carbon and Energy (ICE) v2 published by the University of Bath in 2011. The figures are for \"cradle to gate\", i.e. they do not consider contributions to embodied carbon during service or at the end of life (demolition etc)." ),
                Formula.FormulaWithNarrative("Embodied carbon from ICEv2 database for " + _mix.ValueAsString + " concrete with " + _replacement.ValueAsString + " cement replacement:")
                .AddFirstExpression("CO_2e = " + _carbonDensity.Value + _carbonDensity.Unit),
                Formula.FormulaWithNarrative("Embodied carbon for reinforced concrete using modification factor:")
                .AddFirstExpression("CO_2e = " + _carbonDensity.Value + @" + 0.077 \left(\frac{" + _rebarDensity.Value + @"}{100}\right)=" + _carbonDensityWithRebar.Value.ToString("N3") + _carbonDensityWithRebar.Unit),
                Formula.FormulaWithNarrative("Mass of RC:")
                .AddFirstExpression(_rcVol.Value.ToString("N3") + @"\times2500=" + _rcMass.Value + _rcMass.Unit),
                Formula.FormulaWithNarrative("Total embodied carbon:")
                .AddFirstExpression("CO_2e = " + _carbonDensityWithRebar.Value.ToString("N3") + @"\times" + _rcMass.Value + " = " + _totalCarbon.Value.ToString("N3") + _totalCarbon.Unit)
            };
        }

        public override void UpdateCalc()
        {
            formulae = null;
            _carbonDensity.Value = carbonData[_replacement.ValueAsString][_mix.ValueAsString];
            _carbonDensityWithRebar.Value = _carbonDensity.Value + 0.077 * (_rebarDensity.Value / 100d);
            _rcMass.Value = _rcVol.Value * 2500;
            _totalCarbon.Value = _rcMass.Value * _carbonDensityWithRebar.Value / 1000;

        }

        void genDictionary()
        {
            carbonData = new Dictionary<string, Dictionary<string, double>>
            {
                {"zero", new Dictionary<string, double>
                    {{"GEN0",0.076},{"GEN1",0.104},{"GEN2",0.114},{"GEN3",0.123},{"RC20/25",0.132},{"RC25/30",0.14},{"RC28/35",0.148},{"RC32/40",0.163},{"RC40/50",0.188},{"PAV1",0.148},{"PAV2",0.163}}
                },
                {"15%PFA", new Dictionary<string, double>
                    {{"GEN0",0.069},{"GEN1",0.094},{"GEN2",0.105},{"GEN3",0.112},{"RC20/25",0.122},{"RC25/30",0.13},{"RC28/35",0.138},{"RC32/40",0.152},{"RC40/50",0.174},{"PAV1",0.138},{"PAV2",0.152} }
                },
                {"30%PFA", new Dictionary<string, double>
                {{"GEN0",0.061},{"GEN1",0.082},{"GEN2",0.093},{"GEN3",0.1},{"RC20/25",0.108},{"RC25/30",0.115},{"RC28/35",0.124},{"RC32/40",0.136},{"RC40/50",0.155},{"PAV1",0.123},{"PAV2",0.137} }
                },
                {"25%GGBS", new Dictionary<string, double>
                {{"GEN0",0.06},{"GEN1",0.08},{"GEN2",0.088},{"GEN3",0.096},{"RC20/25",0.104},{"RC25/30",0.111},{"RC28/35",0.119},{"RC32/40",0.133},{"RC40/50",0.153},{"PAV1",0.118},{"PAV2",0.133} }
                },
                {"50%GGBS", new Dictionary<string, double>
                {{"GEN0",0.045},{"GEN1",0.058},{"GEN2",0.065},{"GEN3",0.07},{"RC20/25",0.077},{"RC25/30",0.081},{"RC28/35",0.088},{"RC32/40",0.1},{"RC40/50",0.115},{"PAV1",0.088},{"PAV2",0.1} }
                }
            };
        }
    }
}
