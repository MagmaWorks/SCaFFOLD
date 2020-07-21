using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using Grasshopper.Kernel.Types;
using Grasshopper;
using Grasshopper.Kernel.Data;

namespace CalcMonkey
{
    public class ConvertCalcListOfDoubleArrays : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ConvertCalcListOfDoubleArrays()
          : base("Unpack output values", "Unpack",
              "Unpacks values from calc output into GH",
              "Magma Works", "Calcs")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ICalc object", "", "", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Index", "", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Numbers", "", "Unpack output as GH value", GH_ParamAccess.tree);
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
            int index= 0;
            if (!DA.GetData(1, ref index)) return;
            var myOutput = myCalc.GetOutputs()[index];
            var myList = new List<double[]>();

            if (myOutput.Type == CalcCore.CalcValueType.LISTOFDOUBLEARRAYS)
            {
                var temp = myOutput as CalcCore.CalcListOfDoubleArrays;
                GH_Structure<GH_Number> myTree = new GH_Structure<GH_Number>();
                for (int i = 0; i < temp.Value.Count; i++)
                {
                    for (int j = 0; j < temp.Value[i].Length; j++)
                    {
                        GH_Path myPath = new GH_Path(i);
                        myTree.Append(new GH_Number(temp.Value[i][j]), myPath);
                    }
                }
                DA.SetDataTree(0, myTree);
            }
            else
            {
                var tree = new GH_Structure<GH_String>();
                tree.Append(new GH_String(myOutput.ValueAsString));
                DA.SetDataTree(0, tree);
            }
            return;
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource1.Unpack;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ff849f7f-8449-4775-adc2-ff2870627d12"); }
        }
    }

}
