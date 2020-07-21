using System;
using System.Collections.Generic;
using CalcCore;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using Grasshopper.Kernel.Types;

namespace CalcMonkey
{
    public class CalcDocOutput : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public CalcDocOutput()
          : base("CalcDocOutput", "docx",
              "Outputs the calculation to an open office word processing document",
              "Magma Works", "Calcs")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ICalc object", "", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Save to file", "", "", GH_ParamAccess.item);
            pManager.AddTextParameter("File path", "", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Result", "OK?", "Result of the export", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object obj = new object();
            if (!DA.GetData(0, ref obj)) return;
            CalcCore.ICalc myCalc;
            IGH_Goo myGoo = obj as IGH_Goo;
            myGoo.CastTo<CalcCore.ICalc>(out myCalc);
            bool output = false;
            if (!DA.GetData(1, ref output)) return;
            string filePath = "";
            if (!DA.GetData(2, ref filePath)) return;

            if (myCalc != null && output)
            {
                OutputToODT.WriteToODT(new List<ICalc> { myCalc }, true, true, true, filePath);
            }
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource1.ExportToDoc;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("22ceef93-88e1-4e34-84c9-24db34066188"); }
        }
    }

}
