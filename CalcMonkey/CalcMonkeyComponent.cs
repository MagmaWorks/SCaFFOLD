using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using Grasshopper.GUI.Canvas;
using System.Drawing;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace CalcMonkey
{
    public class CalcMonkeyComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public CalcMonkeyComponent()
          : base("CalcMonkey", "Calculate!",
              "",
              "WhitbyWood", "Calcs")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Calc type", "", "Name of assembly and type", GH_ParamAccess.item);
            pManager.AddTextParameter("Calc input names", "", "", GH_ParamAccess.list);
            pManager.AddTextParameter("Calc input values", "", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text representation of output values", "Outputs", "List of output values represented as strings", GH_ParamAccess.list);
            pManager.AddGenericParameter("ICalc object", "", "A calculation object implementing the ICalc interface.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string assemblyName = "";
            if (!DA.GetData(0, ref assemblyName)) return;
            Type t = AvailableCalcsLoader.AvailableCalcs[0].Class;
            foreach (var item in AvailableCalcsLoader.AvailableCalcs)
            {
                if (item.Class.FullName == assemblyName)
                {
                    t = item.Class;
                }
            }
            var calcInstance = (CalcCore.ICalc)Activator.CreateInstance(t);

            var inputNames = new List<string>();
            if (!DA.GetDataList(1, inputNames)) return;
            var inputValues = new List<string>();
            if (!DA.GetDataList(2, inputValues)) return;
            int inputs = Math.Min(inputNames.Count, inputValues.Count);
            var calcInputs = calcInstance.GetInputs();
            for (int i = 0; i < inputs; i++)
            {
                var calcVal = calcInputs.First(a => a.Name == inputNames[i]);
                calcVal.ValueAsString = inputValues[i];
            }
            calcInstance.UpdateCalc();
            List<string> outString = new List<string>();
            foreach (var item in calcInstance.GetOutputs())
            {
                outString.Add(item.Name + ": " + item.ValueAsString);
            }
            DA.SetDataList(0, outString);
            DA.SetData(1, calcInstance);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource1.Calc;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b7ffe538-003a-4d29-b63e-213c1366a4d1"); }
        }
    }
}
