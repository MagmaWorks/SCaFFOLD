using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;
using BriefFiniteElementNet;
using BriefFiniteElementNet.Elements;

namespace TestCalcs
{
    public class FE_Test_Calc : CalcCore.CalcBase
    {
        Model model;
        CalcCore.CalcDouble _spacing;
        CalcCore.CalcDouble _reaction;

        public FE_Test_Calc()
        {
            model = new Model();
            _spacing = inputValues.CreateDoubleCalcValue("Spacing", "", "", 1);
            _reaction = outputValues.CreateDoubleCalcValue("Reactions", "", "", 0);
        }

        public override List<Formula> GenerateFormulae()
        {
            return new List<Formula>();
        }

        public override void UpdateCalc()
        {
            // Initiating Model, Nodes and Members
            model = new Model();

            double sp = _spacing.Value;
            var n1 = new Node(sp, sp, 0);
            n1.Label = "n1";//Set a unique label for node
            var n2 = new Node(-sp, sp, 0) { Label = "n2" };//using object initializer for assigning Label
            var n3 = new Node(sp, -sp, 0) { Label = "n3" };
            var n4 = new Node(-sp, -sp, 0) { Label = "n4" };
            var n5 = new Node(0, 0, 1) { Label = "n5" };

            var e1 = new TrussElement2Node(n1, n5) { Label = "e1" };
            var e2 = new TrussElement2Node(n2, n5) { Label = "e2" };
            var e3 = new TrussElement2Node(n3, n5) { Label = "e3" };
            var e4 = new TrussElement2Node(n4, n5) { Label = "e4" };
            //Note: labels for all members should be unique,
            //else you will receive InvalidLabelException when adding it to model

            e1.A = e2.A = e3.A = e4.A = 9e-4;
            e1.E = e2.E = e3.E = e4.E = 210e9;

            model.Nodes.Add(n1, n2, n3, n4, n5);
            model.Elements.Add(e1, e2, e3, e4);

            //Applying restraints

            n1.Constraints = n2.Constraints = n3.Constraints = n4.Constraints = Constraint.Fixed;
            n5.Constraints = Constraint.RotationFixed;

            //Applying load
            var force = new Force(0, 1000, -1000, 0, 0, 0);
            var loadCase = new LoadCase("test", LoadType.Live);
            n5.Loads.Add(new NodalLoad(force, loadCase));//adds a load with LoadCase of DefaultLoadCase to node loads

            //Adds a NodalLoad with Default LoadCase

            try
            {
                model.Solve();
                var r1 = n1.GetSupportReaction(loadCase);
                var r2 = n2.GetSupportReaction(loadCase);
                var r3 = n3.GetSupportReaction(loadCase);
                var r4 = n4.GetSupportReaction(loadCase);

                var rt = r1 + r2 + r3 + r4;//shows the Fz=1000 and Fx=Fy=Mx=My=Mz=0.0

                _reaction.Value = rt.Forces.Z;
            }
            catch (Exception)
            {
                _reaction.Value = double.NaN;
            }
            
            

        }
    }
}
