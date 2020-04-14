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
    public class GetInputDetails : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GetInputDetails()
          : base("Get details of input", "Input info",
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
            pManager.AddIntegerParameter("Index", "", "", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Possible output values", "Outputs", "List of output values represented as strings", GH_ParamAccess.list);
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
            int index = 0;
            if (!DA.GetData(1, ref index)) return;
            List<string> returnList = new List<string>();
            var input = calcInstance.GetInputs()[index];
            if (input.Type == CalcCore.CalcValueType.SELECTIONLIST)
            {
                returnList = (input as CalcCore.CalcSelectionList).SelectionList;
            }
            else if (input.Type == CalcCore.CalcValueType.DOUBLE)
            {
                returnList.Add("Use any valid double. Default value is " + input.ValueAsString);
            }
            else if (input.Type == CalcCore.CalcValueType.FILEPATH)
            {
                returnList.Add("File path");
            }
            else if (input.Type == CalcCore.CalcValueType.FOLDERPATH)
            {
                returnList.Add("Folder path");
            }
            DA.SetDataList(0, returnList);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource1.InputDetails;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2d9afceb-5cc4-4501-a403-028c918d0201"); }
        }
    }
}

