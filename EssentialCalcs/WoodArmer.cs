using CalcCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EssentialCalcs
{
    public class WoodArmer : CalcBase
    {

        CalcDouble mx;
        CalcDouble my;
        CalcDouble mxy;
        CalcDouble mxb;
        CalcDouble myb;
        CalcDouble mxt;
        CalcDouble myt;
        private List<string> expression;

        public WoodArmer()
        {
            initValues(0,0,0);
        }

        public WoodArmer(double mx, double my, double mxy)
        {
            initValues(mx,my,mxy);
        }

        private void initValues(double mx, double my, double mxy)
        {
            this.mx = inputValues.CreateDoubleCalcValue("Moment about x axis", "M_x", "kNm", mx);
            this.my = inputValues.CreateDoubleCalcValue("Moment about y axis", "M_y", "kNm", my);
            this.mxy = inputValues.CreateDoubleCalcValue("Twist", "M_xy", "kNm", mxy);
            mxb = outputValues.CreateDoubleCalcValue("Moment about x axis for bottom reinforcement", "M_xb", "kNm", 0);
            myb = outputValues.CreateDoubleCalcValue("Moment about y axis for bottom reinforcement", "M_yb", "kNm", 0);
            mxt = outputValues.CreateDoubleCalcValue("Moment about x axis for top reinforcement", "M_xt", "kNm", 0);
            myt = outputValues.CreateDoubleCalcValue("Moment about y axis for top reinforcement", "M_yt", "kNm", 0);
        }

        public override List<Formula> GenerateFormulae()
        {
            return new List<Formula>()
            {
                new Formula(){
                    Expression = expression,
                    Ref ="Mike's email" }
            };
        }

        public override void UpdateCalc()
        {
            formulae = null;
            expression = new List<string>();
            if (mx.Value <= my.Value && mx.Value >= - Math.Abs(mxy.Value))
            {
                expression.Add("M_x <= M_y");
                expression.Add("M_x >= -|M_xy|");
                mxb.Value = mx.Value + Math.Abs(mxy.Value);
                expression.Add("M_xb = M_x + |M_xy|");
                expression.Add(string.Format("M_xb = {0} + |{1}|", mx.Value, mxy.Value));
                expression.Add(string.Format("M_xb = {0}", mxb.Value));
                myb.Value = my.Value + Math.Abs(mxy.Value);
            }
            else if (mx.Value > my.Value && my.Value >= - Math.Abs(mxy.Value))
            {
                expression.Add("M_x > M_y");
                expression.Add("M_y >= -| M_xy |");
                mxb.Value = mx.Value + Math.Abs(mxy.Value);
                expression.Add("M_xb = M_x + |M_xy|");
                expression.Add(string.Format("M_xb = {0} + |{1}|", mx.Value, mxy.Value));
                expression.Add(string.Format("M_xb = {0}", mxb.Value));
                myb.Value = my.Value + Math.Abs(mxy.Value);
            }
            else if (mx.Value <= my.Value && mx.Value <= - Math.Abs(mxy.Value))
            {
                expression.Add("M_x <= M_y");
                expression.Add("M_x <= -|M_xy|");
                mxb.Value = 0;
                expression.Add("M_xb = 0");
                myb.Value = my.Value + (Math.Pow(mxy.Value, 2) / Math.Abs(mx.Value));
            }
            else if (mx.Value > my.Value && my.Value <= - Math.Abs(mxy.Value))
            {
                expression.Add("M_x > M_y");
                expression.Add("M_y <= -| M_xy |");
                mxb.Value = mx.Value + (Math.Pow(mxy.Value, 2) / Math.Abs(my.Value));
                expression.Add("M_xb = M_x + (M_xy^2)/(|M_y|)");
                expression.Add(string.Format("M_xb = {0} + (({2})^2)/(|{1}|)", mx.Value, my.Value, mxy.Value));
                expression.Add(string.Format("M_xb = {0}", mxb.Value));
                myb.Value = 0;
            }

            if (mx.Value <= my.Value && my.Value <= Math.Abs(mxy.Value))
            {
                mxt.Value = -mx.Value + Math.Abs(mxy.Value);
                myt.Value = -my.Value + Math.Abs(mxy.Value);
            }
            else if (mx.Value > my.Value && mx.Value <= Math.Abs(mxy.Value))
            {
                mxt.Value = -mx.Value + Math.Abs(mxy.Value);
                myt.Value = -my.Value + Math.Abs(mxy.Value);
            }
            else if (mx.Value <= my.Value && my.Value > Math.Abs(mxy.Value))
            {
                mxt.Value = -mx.Value + (Math.Pow(mxy.Value, 2) / Math.Abs(my.Value));
                myt.Value = 0;
            }
            else if (mx.Value > my.Value && mx.Value > Math.Abs(mxy.Value))
            {
                mxt.Value = 0;
                myt.Value = -my.Value + (Math.Pow(mxy.Value, 2) / Math.Abs(mx.Value));
            }
        }
    }
}
