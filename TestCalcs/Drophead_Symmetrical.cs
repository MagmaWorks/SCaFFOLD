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
    public class DropHead_Symmetrical : CalcCore.CalcBase
    {
        Model model;
        CalcCore.CalcDouble _spacingTop;
        CalcDouble _spacingBottom;
        CalcDouble _spacingVertical;
        CalcDouble _loadA;
        CalcCore.CalcDouble _reactionA;
        CalcCore.CalcDouble _reactionB;
        CalcDouble _strutResolvedForce;
        CalcDouble _tieForce;

        public DropHead_Symmetrical()
        {
            model = new Model();
            _spacingTop = inputValues.CreateDoubleCalcValue("Spacing at top", "L_1", "m", 0.8);
            _spacingBottom = inputValues.CreateDoubleCalcValue("Spacing at bottom", "L_2", "m", 0.8);
            _spacingVertical = inputValues.CreateDoubleCalcValue("Vertical spacing", "h", "m", 1);
            _loadA = inputValues.CreateDoubleCalcValue("Force A", "F_A", "kN", 1);
            _reactionA = outputValues.CreateDoubleCalcValue("Reactions", "", "", 0);
            _reactionB = outputValues.CreateDoubleCalcValue("Reactions", "", "", 0);
            _strutResolvedForce = outputValues.CreateDoubleCalcValue("Resolved strut force", "F_{strut}", "kN", 0);
            _tieForce = outputValues.CreateDoubleCalcValue("Bottom tie force", "F_{tie}", "kN", 0);
        }

        public override List<Formula> GenerateFormulae()
        {
            return new List<Formula>();
        }

        public override void UpdateCalc()
        {
            // Initiating Model, Nodes and Members
            model = new Model();

            double spt = _spacingTop.Value / 2;
            double spb = _spacingBottom.Value / 2;
            double spv = _spacingVertical.Value;

            var n1a = new BriefFiniteElementNet.Node(-spt, 0, spv) { Label = "1a" };
            var n1b = new BriefFiniteElementNet.Node(spt, 0, spv) { Label = "1b" };
            var n2a = new BriefFiniteElementNet.Node(0, -spb, 0) { Label = "2a" };
            var n2b = new BriefFiniteElementNet.Node(0, spb, 0) { Label = "2b" };

            var e1 = new TrussElement2Node(n1a, n2a) { Label = "e1" };
            var e2 = new TrussElement2Node(n1a, n2b) { Label = "e2" };
            var e3 = new TrussElement2Node(n1b, n2a) { Label = "e3" };
            var e4 = new TrussElement2Node(n1b, n2b) { Label = "e4" };
            var e5 = new TrussElement2Node(n1a, n1b) { Label = "e5" };
            var e6 = new TrussElement2Node(n2a, n2b) { Label = "e6" };
            //Note: labels for all members should be unique,
            //else you will receive InvalidLabelException when adding it to model

            e1.A = e2.A = e3.A = e4.A = e5.A = e6.A = 0.009;
            e1.E = e2.E = e3.E = e4.E = e5.E = e6.E = 210e9;

            model.Nodes.Add(n1a, n1b, n2a, n2b);
            model.Elements.Add(e1, e2, e3, e4, e5, e6);

            //Applying restraints

            n1a.Constraints = Constraint.FromString("100111");
            n1b.Constraints = Constraint.FromString("000111");
            n2a.Constraints = Constraint.FromString("111111");
            n2b.Constraints = Constraint.FromString("101111");

            //Applying load
            var forceA = new Force(0, 0, -_loadA.Value / 2, 0, 0, 0);
            var forceB = new Force(0, 0, -_loadA.Value / 2, 0, 0, 0);
            var loadCase = new LoadCase("test", LoadType.Live);
            n1a.Loads.Add(new NodalLoad(forceA, loadCase));
            n1b.Loads.Add(new NodalLoad(forceB, loadCase));
            var loadComb = new LoadCombination();
            loadComb.Add(loadCase, 1);

            //Adds a NodalLoad with Default LoadCase

            try
            {
                model.Solve();
                var rA = n2a.GetSupportReaction(loadCase);
                var rB = n2b.GetSupportReaction(loadCase);
                var T = e6.GetInternalForceAt(0.5, loadComb);
                var F1 = e1.GetInternalForceAt(0.5, loadComb);
                var F2 = e3.GetInternalForceAt(0.5, loadComb);
                var Fresolved = F1 + F2;
                
                _reactionA.Value = rA.Forces.Z;
                _reactionB.Value = rB.Forces.Z;
                _strutResolvedForce.Value = Fresolved.Forces.Length;
                _tieForce.Value = T.Forces.Length;

            }
            catch (Exception)
            {
                _reactionA.Value = double.NaN;
                _reactionB.Value = double.NaN;
            }
        }
    }
}
