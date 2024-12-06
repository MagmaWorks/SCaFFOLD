using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Scaffold.Calculations;
using Scaffold.Core;
using Scaffold.Core.Abstract;
using Scaffold.Core.Interfaces;

namespace SCaFFOLDForGrasshopper
{
    public class SCaFFOLDForGrasshopperComponent : GH_Component
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
        public SCaFFOLDForGrasshopperComponent()
          : base("SCaFFOLDForGrasshopperComponent", "ASpi",
            "Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.",
            "Magma Works", "SCaFFOLD")
        {
            embeddedCalc = new ConcreteCreepAndShrinkage();
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
                embeddedCalc = new ConcreteCreepAndShrinkage();
                inputs = reader.GetInputs(embeddedCalc);
            }

            if (inputs == null) { return; };


            pManager.AddNumberParameter(inputs[0].DisplayName, inputs[0].Symbol, "SCaFFOLD calc", GH_ParamAccess.item);

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
            pManager.AddTextParameter("Result", "R", "Output of the test calc", GH_ParamAccess.item);

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
            double inputVal = 0;
            DA.GetData(0, ref inputVal);

            inputs[0].TryParse(inputVal.ToString());

            embeddedCalc.Update();

            // Finally assign the spiral to the output parameter.
            DA.SetData(0, outputs[0].ValueAsString());
        }

        /// <summary>
        /// The Exposure property controls where in the panel a component icon 
        /// will appear. There are seven possible locations (primary to septenary), 
        /// each of which can be combined with the GH_Exposure.obscure flag, which 
        /// ensures the component will only be visible on panel dropdowns.
        /// </summary>
        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("dc1c1cb1-484e-46c7-8288-1ddb3c613572");
    }
}
