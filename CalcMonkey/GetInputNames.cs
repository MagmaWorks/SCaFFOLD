using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using Grasshopper.GUI.Canvas;
using System.Drawing;

namespace CalcMonkey
{
    public class GetInputNames : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GetInputNames()
          : base("Get Input Names", "Input names",
              "",
              "Magma Works", "Calcs")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Calc type", "", "Name of assembly and type", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Input names", "Inputs", "A list of all the inputs available for this type of calc", GH_ParamAccess.list);
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

            List<string> outString = new List<string>();
            foreach (var item in calcInstance.GetInputs())
            {
                outString.Add(item.Name);
            }
            DA.SetDataList(0, outString);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource1.ListInputs;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("66856908-d9d4-4d89-af10-ac8d6dbf0a59"); }
        }
    }

}
