using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calcs
{
    public class MainViewData : ViewModelBase
    {
        public MainViewTypes ViewType { get; private set; }
        public CalcPluginBase Plugin { get; set; }
        public CalcCore.ICalc Calc { get; set; }
        public CalculationViewModel Parent { get; private set; }
        public string Name { get;private set; }

        public MainViewData(CalculationViewModel vm, CalcPluginBase plugin)
        {
            Parent = vm;
            ViewType = MainViewTypes.PLUGIN;
            Plugin = plugin;
            var name = ((PluginNameAttribute)Attribute.GetCustomAttribute(plugin.GetType(), typeof(PluginNameAttribute))).Name;
            Name = name;
        }

        public MainViewData(CalculationViewModel vm, CalcCore.ICalc calc)
        {
            Parent = vm;
            Calc = calc;
            ViewType = MainViewTypes.CALC;
            Name = "Calc";
        }

        MainViewData()
        {

        }

        public static MainViewData CreateMainViewAdd(CalculationViewModel vm)
        {
            var returnValue = new MainViewData();
            returnValue.Parent = vm;
            returnValue.ViewType = MainViewTypes.ADD;
            returnValue.Name = "+";
            return returnValue;
        }

        public static MainViewData CreateMainView3D(CalculationViewModel vm)
        {
            var returnValue = new MainViewData();
            returnValue.Parent = vm;
            returnValue.ViewType = MainViewTypes.VIEW3D;
            returnValue.Name = "3D View";
            return returnValue;
        }

        public static MainViewData CreateMainViewFormulae(CalculationViewModel vm)
        {
            var returnValue = new MainViewData();
            returnValue.Parent = vm;
            returnValue.ViewType = MainViewTypes.FORMULAE;
            returnValue.Name = "Output";
            return returnValue;
        }
    }
}
