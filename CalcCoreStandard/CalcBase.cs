using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using MWGeometry;


namespace CalcCore
{
    public abstract class CalcBase : ICalc
    {
        protected CalcValueFactory inputValues = new CalcValueFactory();

        protected CalcValueFactory outputValues = new CalcValueFactory();

        protected List<Formula> formulae;

        public string InstanceName { get; set; } = "";

        public string ClassName
        {
            get
            {
                Type type = this.GetType();
                if (Attribute.IsDefined(type, typeof(CalcCore.CalcNameAttribute)))
                {
                    return ((CalcCore.CalcNameAttribute)Attribute.GetCustomAttribute(type, typeof(CalcCore.CalcNameAttribute))).CalcName;
                }
                else
                    return this.GetType().ToString();
            }
        }

        protected string _typeName;
        public string TypeName
        {
            get
            {
                return _typeName ?? (_typeName = this.GetType().ToString()); 
            }
        }

        public List<Formula> GetFormulae()
        {
            return formulae ?? (formulae = GenerateFormulae());
        }

        public abstract List<Formula> GenerateFormulae();

        public List<CalcValueBase> GetInputs()
        {
            return inputValues.GetValues();
        }

        public List<CalcValueBase> Inputs
        {
            get
            {
                return inputValues.GetValues();
            }
        }

        protected CalcStatus _status = CalcStatus.NONE;
        public CalcStatus Status
        {
            get
            {
                return _status;
            }
        }

        protected List<DxfDocument> _drawings;

        public virtual List<DxfDocument> GetDrawings()
        {
            return _drawings;
        }

        public List<CalcValueBase> GetOutputs()
        {
            return outputValues.GetValues();
        }

        public abstract void UpdateCalc();

        public virtual List<MW3DModel> Get3DModels()
        {
            return new List<MW3DModel>();
        }
    }
}
