using CalcCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestCalcs
{
    public class Moment : CalcBase
    {
        CalcDouble span;
        CalcDouble udl;
        CalcDouble moment;
        CalcDouble shear;
        CalcFilePath filePath;
        CalcSelectionList endConditions;
        Dictionary<string, Action> momentCalc;
        List<Section> sections;

        public Moment()
        {
            //just some arbitrary default values
            initialise(10, 5);
        }

        public Moment(double span, double udl)
        {
            initialise(span, udl);
        }

        void initialise(double span, double udl)
        {
            this.span = this.inputValues.CreateDoubleCalcValue("span", "l", "m", span);
            this.udl = this.inputValues.CreateDoubleCalcValue("udl", "w", "kN/m", udl);
            moment = this.outputValues.CreateDoubleCalcValue("moment", "M", "kNm", 0);
            shear = this.outputValues.CreateDoubleCalcValue("Shear", "V", "kN", 0);
            endConditions = this.inputValues.CreateCalcSelectionList("End Condition", "Pinned", new List<string> { "Fixed", "Pinned" });
            momentCalc = new Dictionary<string, Action>() { { "Fixed", new Action(calcFixedMoment) }, { "Pinned", new Action(calcPinnedMoment) } };
            //filePath = this.inputValues.CreateCalcFilePath("Section Names", AppDomain.CurrentDomain.BaseDirectory + @"Libraries\Section_Names.csv");
            UpdateCalc();
        }

        public override void UpdateCalc()
        {
            formulae = null;
            //sections = Section.openSteelSectionFile(filePath.ValueAsString);
            momentCalc[endConditions.ValueAsString]();
            shear.Value = (udl.Value * span.Value) / 2;
        }

        void calcPinnedMoment()
        {
            moment.Value = (udl.Value * Math.Pow(span.Value, 2)) / 8;
        }

        void calcFixedMoment()
        {
            moment.Value = (udl.Value * Math.Pow(span.Value, 2)) / 12; 
        }

        public override List<Formula> GenerateFormulae()
        {
            List<string> momentCalc = new List<string>();
            momentCalc.Add(String.Format("{0} = {1} {2}^2 / 8", moment.Symbol, udl.Symbol, span.Symbol));
            momentCalc.Add(String.Format("{0} = {1} {2}^2 / 8", moment.Symbol, udl.Value, span.Value));
            momentCalc.Add(String.Format("{0} = {1}", moment.Symbol, moment.Value));
            List<string> shearCalc = new List<string>();
            shearCalc.Add(String.Format("{0} = {1}{2}/2", shear.Symbol, udl.Symbol, span.Symbol));
            shearCalc.Add(String.Format("{0} = {1}{2}/2", shear.Symbol, udl.Value, span.Value));
            shearCalc.Add(String.Format("{0} = {1}", shear.Symbol, shear.Value));
            return new List<Formula>()
            {
                new Formula(){Ref="Moment calc", Expression = momentCalc},
                new Formula(){Ref="Shear calc", Expression=shearCalc}
            };
        }
    }
}
