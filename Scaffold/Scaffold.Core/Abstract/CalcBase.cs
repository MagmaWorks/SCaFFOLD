using System.Reflection;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues.Abstract;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.Core.Abstract
{
    // TODO: This model should determine if it calculates on each change or later in the pipeline.
    public abstract class CalcBase : ICalc
    {
        private List<ICalcDetails> _inputs;
        private List<ICalcDetails> _outputs;
        
        protected CalcBase()
        {
            InstanceName = "";
        }

        protected CalcStatus _status = CalcStatus.NONE;
        protected List<Formula> _formulae;
        protected List<DxfDocument> _drawings;
        
        public string ClassName
        {
            get
            {
                var type = GetType();
                var attr = type.GetCustomAttribute<CalcNameAttribute>();
                return attr != null ? attr.CalcName : type.Name;
            }
        }
        
        public string InstanceName { get; set; }
        public string TypeName => GetType().Name;
        public CalcStatus Status => _status;

        public IReadOnlyList<ICalcDetails> Inputs => _inputs;
        public IReadOnlyList<ICalcDetails> Outputs => _outputs;
        public List<Formula> GetFormulae() => _formulae ??= GenerateFormulae();
        public virtual List<DxfDocument> GetDrawings() => _drawings;
        public virtual List<MW3DModel> Get3DModels() => new();
        
        /// <summary>
        /// A required string value to identify this calculation from other calculations in the Scaffold system. 
        /// Suggestion: Use an online GUID editor such as https://www.guidgenerator.com/online-guid-generator.aspx to
        /// generate a unique value, then never change it.
        /// </summary>
        public abstract string UniqueCalculationIdentifier();
        public abstract List<Formula> GenerateFormulae();
        protected abstract void UpdateCalc();

        public void LoadIoCollections()
        {
            if (_inputs != null && _outputs != null) 
                return;
            
            _inputs = new List<ICalcDetails>();
            _outputs = new List<ICalcDetails>();
                
            var allProps = GetType().GetRuntimeProperties();
            foreach (var prop in allProps)
            {
                var baseTypeName = prop.PropertyType.BaseType?.ToString() ?? "";
                if (baseTypeName.Contains(typeof(CalcValue<>).Name) == false)
                    continue;

                var value = (ICalcDetails) prop.GetValue(this);
                if (value?.Group == IoDirection.Input)
                {
                    _inputs.Add(value);
                }
                else
                {
                    _outputs.Add(value);
                }
            }
        }
        
        public void Recalculate()
        {
            LoadIoCollections();
            UpdateCalc();
        }

        protected static byte[] BitmapToBytes(Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        protected SKBitmap GetBitmapResource(string name)
        {
            var coreLib = Assembly.GetAssembly(typeof(CalcBase));
            var resourceLocation = $@"{AppContext.BaseDirectory}\Resources\Images\{name}";

            using var stream = new FileStream(resourceLocation, FileMode.Open);
            var bitmap = SKBitmap.Decode(stream);

            return bitmap;
        }
    }
}