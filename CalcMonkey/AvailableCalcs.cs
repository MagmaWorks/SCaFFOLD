using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using Grasshopper.GUI.Canvas;
using System.Drawing;
using System.Windows.Forms;
using Grasshopper.Kernel.Parameters;
using GH_IO.Serialization;

namespace CalcMonkey
{
    public class AvailableCalcs : GH_Component, IGH_VariableParameterComponent
    {
        CalcCore.ICalc calc;

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public AvailableCalcs()
          : base("SCaFFOLD Calc", "Calc",
              "Select the SCaFFOLD calc you want to use",
              "Magma Works", "Calcs")
        {
        }

        private string selectedCalc = "";

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            if (calc != null)
            {
                var names = calc.GetInputs();
                foreach (var name in names)
                {
                    if (name.Type == CalcCore.CalcValueType.DOUBLE)
                    {
                        pManager.AddNumberParameter(name.Name, "", "", GH_ParamAccess.item);
                    }
                }
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Available calcs", "Calcs", "A list of all the available types of calculation", GH_ParamAccess.list);
            pManager.AddGenericParameter("ICalc object", "", "A calculation object implementing the ICalc interface.", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (calc == null)
            {
                return;
            }

            var calcInputs = calc.GetInputs();
            for (int i = 0; i < Params.Input.Count; i++)
            {
                var input = Params.Input[i];
                foreach (var calcInput in calcInputs)
                {
                    if (calcInput.Name == input.Name)
                    {
                        double inputValue = 0;
                        if (!DA.GetData(i, ref inputValue)) ;
                        (calcInput as CalcCore.CalcDouble).Value = inputValue;
                    }
                }
            }
            calc.UpdateCalc();
            string output = "";
            foreach (var outputVal in calc.GetOutputs())
            {
                output += outputVal.Name + ": " + outputVal.ValueAsString + ";     ";
            }
            DA.SetData(0, output);
            DA.SetData(1, calc);

            foreach (var calcOutput in calc.GetOutputs())
            {
                for (int i = 0; i < Params.Output.Count; i++)
                {
                    var outputGH = Params.Output[i];
                    if (calcOutput.Name == outputGH.Name)
                    {
                        DA.SetData(i, (calcOutput as CalcCore.CalcDouble).Value);
                    }
                }

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
            get { return new Guid("7dea9fe0-8a80-4166-9f98-4e013775a77b"); }
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            if (calc != null)
            {
                return;
            }
            var calcsAvailable = AvailableCalcsLoader.AvailableCalcs;          
            MenuItem subMenu = new MenuItem("SCaFFOLD Calculations available");
            List<ToolStripMenuItem> subMenuItems = new List<ToolStripMenuItem>();
            foreach (var item in calcsAvailable)
            {
                ToolStripMenuItem newItem = new ToolStripMenuItem(item.Name, null, doClick);
                newItem.Tag = item.Class;
                subMenuItems.Add(newItem);
            }
            var newitem = new ToolStripMenuItem("SCaFFOLD", null, subMenuItems.ToArray());
            menu.Items.Add(newitem);
        }

        void doClick(object sender, EventArgs e)
        {
            var calcInstance = (CalcCore.ICalc)Activator.CreateInstance(((sender as ToolStripMenuItem).Tag) as Type);
            calc = calcInstance;

            foreach (var input in calc.GetInputs())
            {
                if (input.Type == CalcCore.CalcValueType.DOUBLE)
                {
                    Params.RegisterInputParam(createGHParam(input.Name));
                }
            }

            foreach (var output in calc.GetOutputs())
            {
                if (output.Type == CalcCore.CalcValueType.DOUBLE)
                {
                    Params.RegisterOutputParam(createGHParam(output.Name));
                }
            }

            this.Name = calc.TypeName;
            this.NickName = calc.TypeName;
            this.Description = "SCaFFOLD Calculation";
            Params.OnParametersChanged();
            this.ExpireSolution(true);
            
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            return new Param_GenericObject();
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public void VariableParameterMaintenance()
        {
            
        }

        private IGH_Param createGHParam(string name)
        {
            IGH_Param param;

            param = new Param_Number();
            param.Name = name;
            param.Access = GH_ParamAccess.item;
            param.Description = "";
            param.NickName = name;

            return param;
        }

        public override bool Write(GH_IWriter writer)
        {
            string name = ((CalcCore.CalcNameAttribute)Attribute.GetCustomAttribute(calc.GetType(), typeof(CalcCore.CalcNameAttribute))).CalcName;
            writer.SetString("CalcType", name);
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {

            if (!base.Read(reader))
                return false;

            string name = reader.GetString("CalcType");
            var listOfCalcs = AvailableCalcsLoader.AvailableCalcs;

            foreach (var availableCalc in listOfCalcs)
            {
                if (availableCalc.Name == name)
                {
                    var calcInstance = (CalcCore.ICalc)Activator.CreateInstance(availableCalc.Class);
                    this.calc = calcInstance;
                }
            }

            return true;
        }
    }

}
