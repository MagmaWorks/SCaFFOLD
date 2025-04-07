using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using OasysUnits;
using Scaffold.Calculations;
using Scaffold.Core;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Interfaces;

namespace SCaFFOLDForGrasshopper
{
    public class SCaFFOLDForGrasshopperComponentRCBeam : GH_Component
    {
        ICalculation embeddedCalc;
        private CalculationReader reader = new();
        private IReadOnlyList<ICalcValue> inputs;
        private IReadOnlyList<ICalcValue> outputs;

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SCaFFOLDForGrasshopperComponentRCBeam()
          : base("SCaFFOLDForGrasshopperComponent", "Test",
            "Test calc in GH.",
            "Magma Works", "SCaFFOLD")
        {
            embeddedCalc = new RectangularRcBeamCalculation();
            inputs = reader.GetInputs(embeddedCalc);
            outputs = reader.GetOutputs(embeddedCalc);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // Use the pManager object to register your input parameters.
            // You can often supply default values when creating parameters.
            // All parameters must have the correct access type. If you want 
            // to import lists or trees of values, modify the ParamAccess flag.
            if (embeddedCalc == null)
            {
                embeddedCalc = new RectangularRcBeamCalculation();
                inputs = reader.GetInputs(embeddedCalc);
            }

            foreach (var item in inputs)
            {
                string unit = "";
                if (typeof(ICalcQuantity).IsAssignableFrom(item.GetType()))
                {
                    unit = ((ICalcQuantity)item).Quantity.Unit.ToString();
                }

                pManager.AddTextParameter(item.DisplayName + " " + unit, item.Symbol + " " + unit, "SCaFFOLD calc", GH_ParamAccess.item, item.ValueAsString());
            }

            // If you want to change properties of certain parameters, 
            // you can use the pManager instance to access them by index:
            //pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.

            outputs = reader.GetOutputs(embeddedCalc);

            foreach (var item in outputs)
            {
                string unit = "";
                if (typeof(ICalcQuantity).IsAssignableFrom(item.GetType()))
                {
                    unit = ((ICalcQuantity)item).Quantity.Unit.ToString();
                }

                pManager.AddTextParameter(item.DisplayName + " " + unit, item.Symbol + " " + unit, "SCaFFOLD calc", GH_ParamAccess.item);
            }

            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            //pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                string inputVal = "";
                DA.GetData(i, ref inputVal);
                var item = inputs[i];
                if (typeof(ICalcQuantity).IsAssignableFrom(item.GetType()))
                {
                    (item as ICalcQuantity).TryParse(inputVal);
                }
                else
                {
                    (item as ICalcValue).TryParse(inputVal);
                }
            }

            embeddedCalc.Calculate();

            // Finally assign the spiral to the output parameter.
            for (int i = 0; i < outputs.Count; i++)
            {
                DA.SetData(i, outputs[i].ValueAsString());

            }
        }

        /// <summary>
        /// The Exposure property controls where in the panel a component icon 
        /// will appear. There are seven possible locations (primary to septenary), 
        /// each of which can be combined with the GH_Exposure.obscure flag, which 
        /// ensures the component will only be visible on panel dropdowns.
        /// </summary>
        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("58b06a88-8a78-4379-8135-76a24b0386ac");
    }
}
