using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// We need to bring the functionality provided by CalcCore into scope so add a 'using' statement:
using CalcCore;


namespace TestCalcs
{
    // The calculation class is named SimpleMoment and is based on the CalcBase class
    // CalcBase provides the functionality required for our SimpleMoment class to communicate with the rest of the WW calcs ecosystem
    // Two methods have to be present - UpdateCalc() and GenerateFormulae(), plus a parameterless constructor
    public class RC_Beam_Torsion : CalcBase
    {
        // We're going to need some values for our calc. Let's define them here
        CalcDouble _beambdim;
        CalcDouble _beamhdim;
        CalcDouble _TEd;
        CalcDouble _VEd;
        CalcDouble _teff;
        CalcDouble _Area;
        CalcDouble _Areak;
        Double Uperi;
        Double Uperik;
        CalcSelectionList _ConcreteGrade;
        Double fck;
        Double fcd;
        CalcDouble _TRdc;
        Double fctm;
        Double fctd;
        CalcDouble _TRdmax;
        CalcDouble _Cover;
        CalcSelectionList _linkdiameter;
        CalcSelectionList _Bardia;
        Double d;
        double vRdmax;
        CalcDouble _VRdmax;
        CalcDouble _VRdc;
        CalcDouble _notensbars;
        double rho;
        Double theta;
        CalcDouble _fy;
        Double fyd;
        CalcDouble _Asi;
        double gammas;
        double gammac;

        
        List<Formula> expressions = new List<Formula>();

        // This is the parameterless constructor
        // A constructor creates an instance of the class - i.e. creates an object
        // Parameterless means it doesn't take any parameters, so the brackets are empty
        public RC_Beam_Torsion()
        {
            // we define our values here, creating them with 'inputvalues' and 'outputvalues'
            // these are 'factory' methods that ensure that the base class can properly manage our newly created values

            //Inputs

            gammas = 1.15;
            gammac = 1.5;

            //Geometry
            _beambdim = inputValues.CreateDoubleCalcValue("Beam Width", "b", "mm", 350);
            _beamhdim = inputValues.CreateDoubleCalcValue("Beam Depth", "h", "mm", 350);
            _Cover = inputValues.CreateDoubleCalcValue("Cover", "c", "mm", 35);

            //Materials
            _ConcreteGrade = inputValues.CreateCalcSelectionList("Concrete grade f_{ck}", "40", new List<string> { "30", "35", "40", "45", "50", "60", "70", "80", "90" });
            _fy = inputValues.CreateDoubleCalcValue("Rebar Yield", "f_{y}", "N/mm^2", 500);

            //Loads
            _TEd = inputValues.CreateDoubleCalcValue("Torsion Load", "T_{Ed}", "kNm", 10);
            _VEd = inputValues.CreateDoubleCalcValue("Shear Load", "V_{Ed}", "kNm", 10);

            //Design
            _Bardia = inputValues.CreateCalcSelectionList("Bar diameter", "16", new List<string> { "10", "12", "16", "20", "25", "32", "40" });
            _linkdiameter = inputValues.CreateCalcSelectionList("Link diameter", "10", new List<string> {"8", "10", "12", "16", "20", "25", "32", "40" });
            _notensbars = inputValues.CreateDoubleCalcValue("Number of tension bars", "No.", "", 3);

            //Ouputs
            _teff = outputValues.CreateDoubleCalcValue("Effective thickess", "t_{eff}", "mm", 10);
            _Area = outputValues.CreateDoubleCalcValue("Section Area", "A", "mm2", 10);
            _Areak = outputValues.CreateDoubleCalcValue("Area k", "A_{k}", "mm2", 10);
            _TRdc = outputValues.CreateDoubleCalcValue("Torsion capacity Conc", "T_{Rd,c}", "kNm", 10);
            _TRdmax = outputValues.CreateDoubleCalcValue("Torsion capacity Max", "T_{Rd,max}", "kNm", 10);
            _VRdmax = outputValues.CreateDoubleCalcValue("Maximum Shear capacity", "V_{Rd,max}", "kN", 10);
            _VRdc = outputValues.CreateDoubleCalcValue("Concrete Shear capacity", "V_{Rd,c}", "kN", 10);
            _Asi = outputValues.CreateDoubleCalcValue("Additional Steel Required", "A_{si}", "mm^2", 0);

            // finally, call your UpdateCalc() method to run the calc for the first time
            UpdateCalc();
        }

        //The below generates the narative, expressions is defined as a list
        public override List<Formula> GenerateFormulae()
        {
            return expressions;
        }

        // this method is used to update your calc whenever input values are changed
        public override void UpdateCalc()
        {
            // reset the formule field. This ensures it will be regenerated with the GenerateFormulae method
            formulae = null;
            expressions = new List<Formula>();

            // Geometry
            _Area.Value = (_beambdim.Value * _beamhdim.Value);
            Uperi = 2 * (_beamhdim.Value + _beambdim.Value);
            double teffcalc = _Area.Value / Uperi;
            double teffmin = 2 * (_Cover.Value + double.Parse(_linkdiameter.ValueAsString) + double.Parse(_Bardia.ValueAsString) * 0.5);
            _teff.Value = Math.Max(teffcalc, teffmin);
            _Areak.Value = (_beambdim.Value - _teff.Value) * (_beamhdim.Value - _teff.Value);
            Uperik = 2 * (_beambdim.Value - 2 * _teff.Value + _beamhdim.Value);

            d = _beamhdim.Value - _Cover.Value - double.Parse(_linkdiameter.ValueAsString) - (double.Parse(_Bardia.ValueAsString)) / 2;
            rho = (_notensbars.Value * Math.PI * 0.25 * Math.Pow(double.Parse(_Bardia.ValueAsString),2)) / (_Area.Value);

            Formula f1 = new Formula();
            f1.Narrative = "Geometry";
            f1.Expression = new List<string>();
            f1.Expression.Add(String.Format("{3} = {1}  {2}  = {0} mm^2", Math.Round(_Area.Value,1), _beambdim.Symbol, _beamhdim.Symbol,_Area.Symbol));
            f1.Expression.Add(String.Format("u = 2 ({1}  + {2} ) = {0} mm", Math.Round(Uperi, 1), _beambdim.Symbol, _beamhdim.Symbol));
            f1.Expression.Add(string.Format("{0} = {1} / u = {2}{3}", _teff.Symbol, _Area.Symbol,Math.Round(_teff.Value,1),_teff.Unit));
            f1.Expression.Add(string.Format("{3} = {1}/ {2} = {0} mm^2 ",Math.Round(_Areak.Value,1), _Area.Symbol, "u",_Areak.Symbol));
            f1.Expression.Add("d=h"+ @"-\phi_{link}" + @"-0.5\phi=" + d + "mm" );
            f1.Expression.Add(@"\rho=" + Math.Round( rho,4) );
            expressions.Add(f1);

            

            //Materials
            var concprop = ConcProperties.ByGrade(_ConcreteGrade.ValueAsString);

            if (concprop.fck > 50)
            {
                var concpropadj = ConcProperties.ByGrade("50");
                fck = concpropadj.fck;
                fcd = 0.85 * (fck) / gammac; //0.85 Alpha cc value is taken conservatively as struts are in perm compression
                fctm = concpropadj.fctm;
                fctd = (concpropadj.fctk005) / gammac; //alpha ct is taken as 1 as recommended

                fyd = _fy.Value / gammas;

                Formula f3 = new Formula();
                f3.Narrative = "Note:The shear strength of concrete is limited to C50/60";
                f3.Ref = "3.1.2(2)P";
                expressions.Add(f3);

                Formula f2 = new Formula();
                f2.Narrative = "Concrete and reinforcement strength";
                f2.Expression.Add(@"f_{ck} =" + Math.Round(fck, 1) + @"N/mm^2");
                f2.Expression.Add(@"f_{cd} = " + Math.Round(fcd, 1) + @" N /mm^2" );
                f2.Expression.Add(@"f_{ctm} = " + Math.Round(fctm, 1) + @"N /mm^2" );
                f2.Expression.Add(@"f_{ctd} = " + Math.Round(fctd, 1) + @" N /mm^2" );
                f2.Expression.Add(@"f_{yd} = " + Math.Round(fyd, 1) + @" N /mm^2" );
                expressions.Add(f2);

            }
            else
            {
                fck = concprop.fck;
                fcd = 0.85 * (fck) / gammac; //0.85 Alpha cc value is taken conservatively as struts are in perm compression
                fctm = concprop.fctm;
                fctd = (concprop.fctk005) / gammac; //alpha ct is taken as 1 as recommended

                fyd = _fy.Value / gammas;

                Formula f4 = new Formula();
                f4.Narrative = "Concrete and reinforcement strength";
                f4.Expression.Add(@"f_{ck} =" + Math.Round(fck, 1) + @"N/mm^2");
                f4.Expression.Add(@"f_{cd} = " + Math.Round(fcd, 1) + @" N /mm^2");
                f4.Expression.Add(@"f_{ctm} = " + Math.Round(fctm, 1) + @"N /mm^2");
                f4.Expression.Add(@"f_{ctd} = " + Math.Round(fctd, 1) + @" N /mm^2");
                f4.Expression.Add(@"f_{yd} = " + Math.Round(fyd, 1) + @" N /mm^2");
                expressions.Add(f4);
            }

            //Design checks

            // VRdmax and TRdmax checks

            double checkmax = 2;

            theta = Math.PI / 8;

            while (true)
            {
                _TRdmax.Value = T_res_concrete_max(_Areak.Value, _Area.Value, fcd, fck, _teff.Value, theta);
                _VRdmax.Value = V_res_concrete_max(_beambdim.Value, d, fcd, fck, theta);

                checkmax= (_VEd.Value / _VRdmax.Value)+(_TEd.Value / _TRdmax.Value);

                if (checkmax < 1)

                {
                    Formula f5 = new Formula();
                    f5.Narrative = "Shear and Torsional max capacity";
                    f5.Ref = "(6.29)";
                    f5.Expression.Add(@"\theta = " + Math.Round(theta, 4));
                    f5.Expression.Add(_TRdmax.Symbol + @"=2 \nu \alpha_{cw} f_{cd} "+_Areak.Symbol + _teff.Symbol + @"sin\theta cos\theta = " +Math.Round(_TRdmax.Value, 2)+_TRdmax.Unit);
                    f5.Expression.Add(_VRdmax.Symbol+@"=(\alpha_{cw} b_{w} z \nu_{1} f_{cd})/(cot\theta + tan\theta)="+Math.Round(_VRdmax.Value,2)+_VRdmax.Unit);
                    f5.Expression.Add(string.Format("{0}/{1} + {2}/{3} = {4} < 1", _TEd.Symbol, _VRdmax.Symbol, _TEd.Symbol, _TRdmax.Symbol, Math.Round(checkmax, 4)));
                    f5.Conclusion = "Pass";
                    f5.Status = CalcStatus.PASS;
                    expressions.Add(f5);

                    //Concrete shear capacity

                    _TRdc.Value = T_res_concrete(_Areak.Value, fctd, _teff.Value);
                    double vRdc = PunchingShear.shearResistanceNoRein(rho, d, fck, 1.5);
                    _VRdc.Value = _beambdim.Value * d * vRdc / 1000;

                    double checkc = 2;

                    checkc = (_VEd.Value / _VRdc.Value) + (_TEd.Value / _TRdc.Value);

                    if (checkc > 1)
                    {
                        Formula f9 = new Formula();
                        f9.Narrative = "Shear and Torsional resistance concrete";
                        f9.Expression = new List<string>();
                        f9.Ref = "(6.31)";
                        //f9.Expression.Add(string.Format("{1} = {0} {2}", Math.Round(_TRdc.Value, 2), _TRdc.Symbol, _TRdc.Unit));
                        //f9.Expression.Add(string.Format("{1} = {0} {2}", Math.Round(_VRdc.Value, 2), _VRdc.Symbol, _TRdc.Unit));
                        f9.Expression.Add(_TRdc.Symbol + "=2 f_{ctd} " + _Areak.Symbol  + _teff.Symbol + "=" +Math.Round(_TRdc.Value,2) + _TRdc.Unit);
                        f9.Expression.Add(_VRdc.Symbol + @"=b_{w} d C_{Rd,c} k (100 \rho_{I} f_{ck})^{1/3}=" + Math.Round(_VRdc.Value, 2) + _VRdc.Unit);
                        f9.Expression.Add(String.Format("{0}/{1} + {2}/{3} = {4}>1", _VEd.Symbol, _VRdc.Symbol, _TEd.Symbol, _TRdc.Symbol, Math.Round(checkc, 3)));
                        f9.Conclusion = "Additional reinforcement required";
                        expressions.Add(f9);

                        _Asi.Value = A_Si_add(_TEd.Value, _Areak.Value, theta, Uperik, fyd);

                        Formula f10 = new Formula();
                        f10.Narrative = "Additional steel requirements";
                        f10.Expression = new List<string>();
                        f10.Ref = "(6.28)";
                        f10.Expression.Add(_Asi.Symbol + "=(" + _TEd.Symbol + @"cot\theta u_{k})/(2 f_{yd}" + _Areak.Symbol + @")=" + Math.Round(_Asi.Value, 2) + _Asi.Unit);
                        expressions.Add(f10);
                    }
                    else
                    {
                        Formula f8 = new Formula();
                        f8.Narrative = "Shear and Torsional resistance concrete";
                        f8.Expression = new List<string>();
                        f8.Ref = "(6.31)";
                        f8.Expression.Add(_TRdc.Symbol + "=2 f_{ctd} " + _Areak.Symbol + _teff.Symbol + "=" + Math.Round(_TRdc.Value, 2) + _TRdc.Unit);
                        f8.Expression.Add(_VRdc.Symbol + @"=b_{w} d C_{Rd,c} k (100 \rho_{I} f_{ck})^{1/3}=" + Math.Round(_VRdc.Value, 2) + _VRdc.Unit);
                        f8.Expression.Add(String.Format("{0}/{1} + {2}/{3} = {4}<1", _VEd.Symbol, _VRdc.Symbol, _TEd.Symbol, _TRdc.Symbol,Math.Round( checkc,3)));
                        f8.Conclusion = "No Additional reinforcement required";
                        f8.Status = CalcStatus.PASS;
                        expressions.Add(f8);
                    }
                    break;
                }
                else
                {
                    if (theta > Math.PI / 4)
                    {
                        Formula f6 = new Formula();
                        f6.Narrative = "Shear and Torsional max capacity";
                        f6.Expression = new List<string>();
                        f6.Expression.Add(_TRdmax.Symbol + @"=2 \nu \alpha_{cw} f_{cd} " + _Areak.Symbol + _teff.Symbol + @"sin\theta cos\theta = " + Math.Round(_TRdmax.Value, 2) + _TRdmax.Unit);
                        f6.Expression.Add(_VRdmax.Symbol + @"=(\alpha_{cw} b_{w} z \nu_{1} f_{cd})/(cot\theta + tan\theta)=" + Math.Round(_VRdmax.Value, 2) + _VRdmax.Unit);
                        f6.Expression.Add(string.Format("{0}/{1} + {2}/{3} = {4} > 1", _TEd.Symbol, _VRdmax.Symbol, _TEd.Symbol, _TRdmax.Symbol,Math.Round( checkmax,3)));
                        f6.Conclusion = "Max capacity of section exceeded, Increase section size";
                        f6.Status = CalcStatus.FAIL;
                        expressions.Add(f6);
                        break;
                    }
                    else
                    {
                        theta = theta + 0.01;
                    }
                }
            }
        }

        //Concrete resistance to Torsion maximum 
        public Double T_res_concrete_max(Double Crosssectionk, Double Crosssection, Double conccompdes, Double conccomp, Double thickeff, double angle)
        {
            Double v;
            Double Alpha=1;//for non-prestressed structures 6.2.3(3) NA
            Double TRDMAX;
            
                v = 0.6 * (1 - (conccomp / 250));

                TRDMAX = (2 * v * Alpha * conccompdes * Crosssectionk * thickeff * Math.Sin(angle) * Math.Cos(angle))/1000000;

                return TRDMAX;
        }

        // Concrete resistance to Torsion
        public Double T_res_concrete(Double Crosssectionk, Double tensconc, Double thickeff)
        {
            Double X;
            X = 4*((2 * Crosssectionk) * tensconc * thickeff) / 1000000;

            //4no sides so value is multiplied for 4

            return X;
        }

        //Concrete residetance to shear maximum
        public Double V_res_concrete_max(Double bw, Double d, Double conccompdes, Double conccomp, double angle)
        {
            Double v1;
            Double Alphacw;
            Double VRDMAX;
            Double z;

            z = 0.9 * d;


            if (conccomp > 60)
            {
                v1 = 0.9 - (conccomp / 250);
            }
            else
            {
                v1 = 0.6;
            }

            Alphacw = 1;//for non-prestressed structures 6.2.3(3) NA

            VRDMAX = ((Alphacw * bw * z * v1 * conccompdes) / (Math.Tan(angle) + (1 / (Math.Tan(angle)))))/1000;

            return VRDMAX;
        }

        // Additional longidudinal Reinforcement
        public Double A_Si_add(Double Ted, Double Ak, double theta, double uk, double fyd)
        {
            Double X;
            X = (Ted * (1 / Math.Tan(theta)) * uk*1000000) / (2 * Ak * fyd);
            return X;
        }
    }
}