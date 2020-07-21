using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// We need to bring the functionality provided by CalcCore into scope so add a 'using' statement:
using CalcCore;
using StructuralDrawing2D;
using SkiaSharp;


namespace EssentialCalcs
{
    // The calculation class is named SimpleMoment and is based on the CalcBase class
    // CalcBase provides the functionality required for our SimpleMoment class to communicate with the rest of the WW calcs ecosystem
    // Two methods have to be present - UpdateCalc() and GenerateFormulae(), plus a parameterless constructor
    [CalcName("RC Beam Section Design")]
    [CalcAlternativeName("TestCalcs.RC_Beam_Section_Design")]
    public class RC_Beam_Section_Design : CalcBase
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
        CalcSelectionList _Bardiaminlist;
        Double d;
        Double d2;
        CalcDouble _Minlink_spacing;
        CalcDouble _Linkspacing;
        CalcDouble _linklegs;
        CalcDouble _VRdmax;
        CalcDouble _VRdc;
        CalcDouble _notensbars;
        CalcDouble _nocompbars;
        double rho;
        double rhototal;
        Double theta;
        CalcDouble _fy;
        Double fyd;
        CalcDouble _Asi;
        double gammas;
        double gammac;
        CalcDouble _VRds;
        Double rholink;
        CalcDouble _Asreq;
        CalcDouble _Ascreq;
        CalcDouble _MEd_ULS;
        CalcDouble _MEd_SLS;
        Double z;
        Double K;
        Double Kdash;
        Double x;
        CalcDouble _Minbarspacing;
        CalcDouble _Bardia;
        CalcDouble _Bardiacomp;
        CalcDouble _Asprov;
        CalcDouble _Ascprov;
        Double Asmin;
        int barref;
        int barref2;
        int barrefbase;
        Double linkdiasel;
        CalcSelectionList _crackwidthlimit;
        double crackwidthlimit;
        Double ULStoSLS;
        double crackwidth;
        double Asmincr;
        double Srmax;
        double epsmepcm;
        double minspacgov;
        double spac;
        double slsbarstress;
        bool bendingcheck;
        bool sectionconcheck;

        List<Formula> expressions = new List<Formula>();

        // This is the parameterless constructor
        // A constructor creates an instance of the class - i.e. creates an object
        // Parameterless means it doesn't take any parameters, so the brackets are empty
        public RC_Beam_Section_Design()
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
            _crackwidthlimit = inputValues.CreateCalcSelectionList("Crack width limit", "0.3", new List<string> { "0.2", "0.3", "0.4"});

            //Loads
            _TEd = inputValues.CreateDoubleCalcValue("Torsion Load", "T_{Ed}", "kNm", 0);
            _VEd = inputValues.CreateDoubleCalcValue("Shear Load", "V_{Ed}", "kN", 10);
            _MEd_ULS = inputValues.CreateDoubleCalcValue("Bending Moment ULS", "M_{Ed,ULS}", "kNm", 10);
            _MEd_SLS = inputValues.CreateDoubleCalcValue("Bending Moment SLS", "M_{Ed,SLS}", "kNm", 7);

            //Design
            _Bardiaminlist = inputValues.CreateCalcSelectionList("Min Bar diameter", "16", new List<string> { "10", "12", "16", "20", "25", "32", "40" });
            _linkdiameter = inputValues.CreateCalcSelectionList("Link diameter", "10", new List<string> {"8", "10", "12", "16", "20", "25", "32", "40" });
            _Minlink_spacing = inputValues.CreateDoubleCalcValue("Min link spacing", "S_{L,min}", "mm", 100);
            _Minbarspacing = inputValues.CreateDoubleCalcValue("Min Bar spacing", "S_{min}", "mm", 50);

            //Ouputs
            _teff = outputValues.CreateDoubleCalcValue("Effective thickess", "t_{eff}", "mm", 10);
            _Area = outputValues.CreateDoubleCalcValue("Section Area", "A", "mm2", 10);
            _Areak = outputValues.CreateDoubleCalcValue("Area k", "A_{k}", "mm2", 10);
            _TRdc = outputValues.CreateDoubleCalcValue("Torsion capacity Conc", "T_{Rd,c}", "kNm", 10);
            _TRdmax = outputValues.CreateDoubleCalcValue("Torsion capacity Max", "T_{Rd,max}", "kNm", 10);
            _VRdmax = outputValues.CreateDoubleCalcValue("Maximum Shear capacity", "V_{Rd,max}", "kN", 10);
            _VRdc = outputValues.CreateDoubleCalcValue("Concrete Shear capacity", "V_{Rd,c}", "kN", 10);
            _Asi = outputValues.CreateDoubleCalcValue("Add. Long. Torsion Rebar", "A_{si}", "mm^2", 0);
            _Linkspacing = outputValues.CreateDoubleCalcValue("Shear link spacing", "s", "mm", 200);
            _linklegs = outputValues.CreateDoubleCalcValue("No. Shear link legs", "no.", "", 2);
            _VRds = outputValues.CreateDoubleCalcValue("Shear link resistance", "V_{Rds}", "kN", 10);
            _notensbars = outputValues.CreateDoubleCalcValue("Number of tension rebars", "No.", "", 3);
            _Bardia = outputValues.CreateDoubleCalcValue("Tension bar dia", "Dia", "mm", 16);
            _Asreq = outputValues.CreateDoubleCalcValue("Tension Bending Rebar Area", "A_{s,req}", "mm^2", 10);
            _Asprov = outputValues.CreateDoubleCalcValue("Tension Bending Rebar Area Provided", "A_{s,prov}", "mm^2", 10);
            _nocompbars = outputValues.CreateDoubleCalcValue("Number of Compression rebars", "No.", "", 0);
            _Bardiacomp = outputValues.CreateDoubleCalcValue("Compression bar dia", "Dia", "mm", 16);
            _Ascreq = outputValues.CreateDoubleCalcValue("Compression Bending Rebar Area Required", "A_{s2,req}", "mm^2", 0);
            _Ascprov = outputValues.CreateDoubleCalcValue("Compression Bending Rebar Area Provided", "A_{s2,prov}", "mm^2", 0);

            // Call your UpdateCalc() method to run the calc for the first time
            UpdateCalc();

        }

        //The below generates the narative, expressions is defined as a list
        public override List<Formula> GenerateFormulae()
        {
            var image = generateImage();
            expressions.Insert(0, new Formula
            {
                Narrative = "Image:",
                Image = image[0],
            });
            return expressions;
        }

        private void resetFields()
        {
            
            _Ascprov.Value = 0;
            _Asprov.Value = 0;
            _notensbars.Value = 2;
            _nocompbars.Value = 0;
            _Ascreq.Value = 0;
            rho = 0;
            rhototal = 0;
            _Bardia.Value = double.Parse(_Bardiaminlist.ValueAsString);
            _Bardiacomp.Value = double.Parse(_Bardiaminlist.ValueAsString);
            crackwidthlimit = double.Parse(_crackwidthlimit.ValueAsString);

            List<double> bardiameters = new List<double> { 10, 12, 16, 20, 25, 32, 40 };
            while (bardiameters[barrefbase] != double.Parse(_Bardiaminlist.ValueAsString))
            {
                barrefbase = barrefbase + 1;
            }

            barref = barrefbase;
            barref2 = barrefbase;

            theta = Math.PI / 8;

            linkdiasel = double.Parse(_linkdiameter.ValueAsString);
            d = _beamhdim.Value - _Cover.Value - linkdiasel - (_Bardia.Value / 2);
            d2 = _Cover.Value + linkdiasel + (_Bardiacomp.Value / 2);



        }



        // this method is used to update your calc whenever input values are changed
        public override void UpdateCalc()
        {
            // reset the formule field. This ensures it will be regenerated with the GenerateFormulae method
            formulae = null;
            
            resetFields();
            expressions = new List<Formula>();
            List<double> bardiameters = new List<double> { 10, 12, 16, 20, 25, 32, 40 };

            // Geometry
            _Area.Value = (_beambdim.Value * _beamhdim.Value);

            Formula f1 = new Formula();
            f1.Narrative = "Geometry";
            f1.Expression = new List<string>();
            f1.Expression.Add(String.Format("{3} = {1}  {2}  = {0} mm^2", Math.Round(_Area.Value,1), _beambdim.Symbol, _beamhdim.Symbol,_Area.Symbol));
            f1.Expression.Add(String.Format("u = 2 ({1}  + {2} ) = {0} mm", Math.Round(Uperi, 1), _beambdim.Symbol, _beamhdim.Symbol));
            f1.Expression.Add(string.Format("{0} = {1} / u = {2}{3}", _teff.Symbol, _Area.Symbol,Math.Round(_teff.Value,1),_teff.Unit));
            f1.Expression.Add(string.Format("{3} = {1}/ {2} = {0} mm^2 ",Math.Round(_Areak.Value,1), _Area.Symbol, "u",_Areak.Symbol));
            expressions.Add(f1);

            //Materials
            
            fyd = _fy.Value / gammas;

            var concprop = ConcProperties.ByGrade(_ConcreteGrade.ValueAsString);

            fck = concprop.fck;
            fcd = 0.85 * (fck) / gammac; //0.85 Alpha cc value is taken conservatively as struts are in perm compression
            fctm = concprop.fctm;
            fctd = (concprop.fctk005) / gammac; //alpha ct is taken as 1 as recommended

            Formula f4 = new Formula();
            f4.Narrative = "Concrete and reinforcement strength";
            f4.Expression.Add(@"f_{ck} =" + Math.Round(fck, 1) + @"N/mm^2");
            f4.Expression.Add(@"f_{cd} = " + Math.Round(fcd, 1) + @" N /mm^2");
            f4.Expression.Add(@"f_{ctm} = " + Math.Round(fctm, 1) + @"N /mm^2");
            f4.Expression.Add(@"f_{ctd} = " + Math.Round(fctd, 1) + @" N /mm^2");
            f4.Expression.Add(@"f_{yd} = " + Math.Round(fyd, 1) + @" N /mm^2");
            expressions.Add(f4);
            

            //Design checks - the below section undertakes the design for bending, torsion and shear

            //Bending design

            ULStoSLS = _MEd_SLS.Value / _MEd_ULS.Value;

            bool bendingcheck = false;
            bool sectionconcheck = true;

            while (bendingcheck == false)
            {
                //Calculation of required bending reinforcement with
                var bres = BendingAreq(_MEd_ULS.Value, d, _beambdim.Value, fck, _fy.Value, d2, fyd);
                _Asreq.Value = bres.Item1;
                z = bres.Item2;
                K = bres.Item3;
                Kdash = bres.Item4;
                _Ascreq.Value = bres.Item5;
                Asmin = bres.Item6;
                x = bres.Item7;

                //get number of bars required tension for bending only
                var bendingbarspac = BendingBarsspacing(_Asreq.Value, _Bardia.Value, _Minbarspacing.Value, _Cover.Value, _beambdim.Value, linkdiasel);
                _notensbars.Value = bendingbarspac.Item1;
                minspacgov = bendingbarspac.Item2;

                _Asprov.Value = Math.PI * 0.25 * Math.Pow(_Bardia.Value, 2) * _notensbars.Value;

                rho = (_Asprov.Value) / (_Area.Value);
                spac = (_beambdim.Value - 2 * _Cover.Value - 2 * linkdiasel - _Bardia.Value) / (_notensbars.Value - 1) - _Bardia.Value;

                //crack width calculations
                slsbarstress = fyd * ULStoSLS * (_Asreq.Value / _Asprov.Value);

                var cres = Crackcontrol(fck, slsbarstress, _Bardia.Value, _notensbars.Value, d, _beambdim.Value, _beamhdim.Value, x, _Cover.Value, linkdiasel, _fy.Value);
                Asmincr = cres.Item1;
                crackwidth = cres.Item2;
                Srmax = cres.Item3;
                epsmepcm = cres.Item4;

                //get number of bars required compression for bending only
                if (_Ascreq.Value > 0)
                {
                    var bendingbarspac2 = BendingBarsspacing(_Ascreq.Value, _Bardiacomp.Value, _Minbarspacing.Value, _Cover.Value, _beambdim.Value, linkdiasel);
                    _nocompbars.Value = bendingbarspac2.Item1;

                    _Ascprov.Value = _nocompbars.Value * Math.PI * 0.25 * Math.Pow(_Bardiacomp.Value, 2);
                }

                rhototal = (_Asprov.Value + _Ascprov.Value) / _Area.Value;

                //checks


                if (_Asreq.Value == 0)
                {
                    sectionconcheck = false;
                    _Bardiacomp.Value = 16;
                    _nocompbars.Value = 0;
                    _notensbars.Value = 0;
                    _Bardia.Value = 16;
                    _Asprov.Value = Math.PI * 0.25 * Math.Pow(_Bardia.Value, 2) * _notensbars.Value;
                    _Ascprov.Value = _nocompbars.Value * Math.PI * 0.25 * Math.Pow(_Bardiacomp.Value, 2);
                    break;
                }
                else if (_Ascprov.Value < _Ascreq.Value)
                {
                    if (_Bardiacomp.Value == bardiameters[6])
                    {
                        _Bardia.Value = 16;
                        _notensbars.Value = 0;
                        _Bardiacomp.Value = 16;
                        _nocompbars.Value = 0;
                        _Asprov.Value = Math.PI * 0.25 * Math.Pow(_Bardia.Value, 2) * _notensbars.Value;
                        _Ascprov.Value = _nocompbars.Value * Math.PI * 0.25 * Math.Pow(_Bardiacomp.Value, 2);
                        break;
                    }
                    else
                    {
                        barref2 = barref2 + 1;
                        _Bardiacomp.Value = bardiameters[barref2];

                        d2 = _Cover.Value + linkdiasel + (_Bardiacomp.Value / 2);
                    }
                }
                else if (_Asprov.Value < _Asreq.Value)
                {
                    if (_Bardia.Value == bardiameters[6])
                    {
                        _Bardia.Value = 16;
                        _notensbars.Value = 0;
                        _Bardiacomp.Value = 16;
                        _nocompbars.Value = 0;
                        _Asprov.Value = Math.PI * 0.25 * Math.Pow(_Bardia.Value, 2) * _notensbars.Value;
                        _Ascprov.Value = _nocompbars.Value * Math.PI * 0.25 * Math.Pow(_Bardiacomp.Value, 2);
                        break;
                    }
                    else
                    {
                        //increase bar diameter
                        barref = barref + 1;
                        _Bardia.Value = bardiameters[barref];

                        //strength checks - can a number of bars be found to suit the strength requirements with this bar diameter
                        d = _beamhdim.Value - _Cover.Value - linkdiasel - (_Bardia.Value / 2);
                    }

                }
                else
                {
                    //increase number of bars to limit crack widths if required and possible

                    bool crackwidthcheck= false;

                    while (true)
                    {
                        _Asprov.Value = Math.PI * 0.25 * Math.Pow(_Bardia.Value, 2) * _notensbars.Value;
                        rho = (_Asprov.Value) / (_Area.Value);
                        rhototal = (_Asprov.Value + _Ascprov.Value) / _Area.Value;
                        spac = (_beambdim.Value - 2 * _Cover.Value - 2 * linkdiasel - _Bardia.Value) / (_notensbars.Value - 1) - _Bardia.Value;

                        slsbarstress = fyd * ULStoSLS * (_Asreq.Value / _Asprov.Value);

                        var cres1 = Crackcontrol(fck, slsbarstress, _Bardia.Value, _notensbars.Value, d, _beambdim.Value, _beamhdim.Value, x, _Cover.Value, linkdiasel, _fy.Value);
                        Asmincr = cres1.Item1;
                        crackwidth = cres1.Item2;
                        Srmax = cres1.Item3;
                        epsmepcm = cres1.Item4;

                        if (crackwidth > crackwidthlimit)
                        {
                            if ((_beambdim.Value - 2 * _Cover.Value - 2 * linkdiasel - _Bardia.Value) / (_notensbars.Value) - _Bardia.Value > minspacgov)
                            {
                                _notensbars.Value = _notensbars.Value + 1;
                            }
                            else
                            {
                               break;
                            }
                        }
                        else if (_Asprov.Value < Asmincr)
                        {
                            if ((_beambdim.Value - 2 * _Cover.Value - 2 * linkdiasel - _Bardia.Value) / (_notensbars.Value) - _Bardia.Value > minspacgov)
                            {
                                _notensbars.Value = _notensbars.Value + 1;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            bendingcheck = true;
                            crackwidthcheck = true;
                            break;
                        }
                    }

                    if (crackwidthcheck == false)
                    {
                        if (_Bardia.Value == bardiameters[6])
                        {
                            _Bardia.Value = 16;
                            _notensbars.Value = 0;
                            _Bardiacomp.Value = 16;
                            _nocompbars.Value = 0;
                            _Asprov.Value = Math.PI * 0.25 * Math.Pow(_Bardia.Value, 2) * _notensbars.Value;
                            _Ascprov.Value = _nocompbars.Value * Math.PI * 0.25 * Math.Pow(_Bardiacomp.Value, 2);
                            break;
                        }
                        else
                        {
                            //increase bar diameter
                            barref = barref + 1;
                            _Bardia.Value = bardiameters[barref];

                            //strength checks - can a number of bars be found to suit the strength requirements with this bar diameter
                            d = _beamhdim.Value - _Cover.Value - linkdiasel - (_Bardia.Value / 2);
                        }
                    }

                }
            }


            //Check bending reinforcement can be provided then check shear/torsion

            if (sectionconcheck == false)
            {
                Formula f5 = new Formula();
                f5.Narrative = "Bending Concrete check";
                f5.Conclusion = "Fail - Concrete failure in beam";
                f5.Status = CalcStatus.FAIL;
                expressions.Add(f5);
            }
            else if (bendingcheck == true)
            {
                Formula f5 = new Formula();
                f5.Narrative = "Bending Reinforcement check";
                f5.Expression.Add("d=h-" + _Cover.Symbol + @"-\phi_{link}" + @"-0.5\phi=" + d + "mm");
                f5.Expression.Add("K=" + "M/(f_{ck}bd^2)=" + Math.Round(K, 4));
                f5.Expression.Add("K'=" + Math.Round(Kdash, 4));


                if (_Ascprov.Value > 0)
                {
                    f5.Expression.Add("K>K'");
                    f5.Expression.Add("d2=" + _Cover.Symbol + @"-\phi_{link}" + @"+0.5\phi_2=" + d2 + "mm");
                    f5.Expression.Add("z=" + "(d/2)(1+(1-3.53K')^{0.5})" + "<0.95d=" + Math.Round(z, 1) + "mm");
                    f5.Expression.Add("M'=" + "bd^2f_{ck}(K-K')");
                    f5.Expression.Add("A_{sc,req} = M'/(f_{yd}(d-d_2))=" + Math.Round(_Ascreq.Value, 1) + "mm^2");
                    f5.Expression.Add("A_{sc,prov} =" + _nocompbars.Value + " x " + _Bardiacomp.Value + "mm=" + Math.Round(_Ascprov.Value, 1) + "mm^2");

                    f5.Expression.Add("A_{s,min} = (0.26f_{ctm}b_td)/(f_{yk})>0.0013b_td = " + Math.Round(Asmin, 1) + "mm^2");
                    f5.Expression.Add("A_{s,req} = (K'f_{ck}bd^2)/(f_{yd}z)+A_{sc,req}=" + Math.Round(_Asreq.Value, 1) + "mm^2");
                    f5.Expression.Add("A_{s,prov} =" + _notensbars.Value + " * " + _Bardia.Value + "mm=" + Math.Round(_Asprov.Value, 1) + "mm^2");

                }
                else
                {

                    f5.Expression.Add("K<K'");
                    f5.Expression.Add("z=" + "(d/2)(1+(1-3.53K)^{0.5})" + "<0.95d=" + Math.Round(z, 1) + "mm");
                    f5.Expression.Add("A_{s,min} = (0.26f_{ctm}b_td)/(f_{yk})>0.0013b_td = " + Math.Round(Asmin, 1) + "mm^2");
                    f5.Expression.Add("A_{s,req} = M/(f_{yd}z)=" + Math.Round(_Asreq.Value, 1) + "mm^2");
                    f5.Expression.Add("A_{s,prov} =" + _notensbars.Value + " * " + _Bardia.Value + "mm=" + Math.Round(_Asprov.Value, 1) + "mm^2");

                }

                if (rhototal > 0.04)
                {
                    f5.Expression.Add(@"\rho=" + Math.Round(rhototal, 6) + ">0.04" + " " + "Warning!");
                }
                else
                {
                    f5.Expression.Add(@"\rho=" + Math.Round(rhototal, 6));
                }

                f5.Conclusion = "Pass";
                f5.Status = CalcStatus.PASS;
                expressions.Add(f5);

                Formula f6 = new Formula();
                f6.Narrative = "Crack width checks";
                f6.Expression.Add(@"\sigma_s=" + Math.Round(slsbarstress, 2)+_fy.Unit);
                f6.Expression.Add(@"S_{r,max}=" + Math.Round( Srmax,2));
                f6.Expression.Add(@"(\epsilon_{sm}-\epsilon_{cm})=" +Math.Round( epsmepcm,6));
                f6.Expression.Add(@"w_k=S_{r,max}(\epsilon_{sm}-\epsilon_{cm})="+Math.Round(crackwidth,3)+"mm<"+crackwidthlimit+"mm");
                f6.Expression.Add(@"A_{s,min}=(k_ckf_{ct,eff}A_{ct})/\sigma_s)="+Math.Round(Asmincr,2) +"mm^2<"+Math.Round( _Asprov.Value,2));
                f6.Conclusion = "Pass";
                f6.Status = CalcStatus.PASS;
                expressions.Add(f6);

                // VRdmax and TRdmax checks

                //material limits on concrete grades

                if (concprop.fck > 50)
                {
                    var concpropadj = ConcProperties.ByGrade("50");
                    fck = concpropadj.fck;
                    fcd = 0.85 * (fck) / gammac; //0.85 Alpha cc value is taken conservatively as struts are in perm compression
                    fctm = concpropadj.fctm;
                    fctd = (concpropadj.fctk005) / gammac; //alpha ct is taken as 1 as recommended

                    Formula f3 = new Formula();
                    f3.Narrative = "Note:The shear strength of concrete is limited to C50/60";
                    f3.Ref = "3.1.2(2)P";
                    expressions.Add(f3);

                    Formula f2 = new Formula();
                    f2.Narrative = "Concrete and reinforcement strength for shear checks";
                    f2.Expression.Add(@"f_{ck} =" + Math.Round(fck, 1) + @"N/mm^2");
                    f2.Expression.Add(@"f_{cd} = " + Math.Round(fcd, 1) + @" N /mm^2");
                    f2.Expression.Add(@"f_{ctm} = " + Math.Round(fctm, 1) + @"N /mm^2");
                    f2.Expression.Add(@"f_{ctd} = " + Math.Round(fctd, 1) + @" N /mm^2");
                    f2.Expression.Add(@"f_{yd} = " + Math.Round(fyd, 1) + @" N /mm^2");
                    expressions.Add(f2);

                }

                double checkmax = 2;

                Uperi = 2 * (_beamhdim.Value + _beambdim.Value);
                double teffcalc = _Area.Value / Uperi;
                double teffmin = 2 * (_Cover.Value + linkdiasel + _Bardia.Value * 0.5);
                _teff.Value = Math.Max(teffcalc, teffmin);
                _Areak.Value = (_beambdim.Value - _teff.Value) * (_beamhdim.Value - _teff.Value);
                Uperik = 2 * (_beambdim.Value - 2 * _teff.Value + _beamhdim.Value);

                while (true)
                {
                    //Check maximum shear and torsion capacity of the section
                    _TRdmax.Value = T_res_concrete_max(_Areak.Value, _Area.Value, fcd, fck, _teff.Value, theta);
                    _VRdmax.Value = V_res_concrete_max(_beambdim.Value, d, fcd, fck, theta, z);

                    checkmax = (_VEd.Value / _VRdmax.Value) + (_TEd.Value / _TRdmax.Value);

                    if (checkmax < 1)

                    {
                        Formula f7 = new Formula();
                        f7.Narrative = "Shear and Torsional max capacity";
                        f7.Ref = "(6.29)";
                        f7.Expression.Add(@"\theta = " + Math.Round(theta * (180 / Math.PI), 4) + "'");
                        f7.Expression.Add(_TRdmax.Symbol + @"=2 \nu \alpha_{cw} f_{cd} " + _Areak.Symbol + _teff.Symbol + @"sin\theta cos\theta = " + Math.Round(_TRdmax.Value, 2) + _TRdmax.Unit);
                        f7.Expression.Add(_VRdmax.Symbol + @"=(\alpha_{cw} b_{w} z \nu_{1} f_{cd})/(cot\theta + tan\theta)=" + Math.Round(_VRdmax.Value, 2) + _VRdmax.Unit);
                        f7.Expression.Add(string.Format("{0}/{1} + {2}/{3} = {4} < 1", _TEd.Symbol, _VRdmax.Symbol, _TEd.Symbol, _TRdmax.Symbol, Math.Round(checkmax, 4)));
                        f7.Conclusion = "Pass";
                        f7.Status = CalcStatus.PASS;
                        expressions.Add(f7);

                        //Concrete shear capacity

                        _TRdc.Value = T_res_concrete(_Areak.Value, fctd, _teff.Value);
                        double vRdc = PunchingShear.shearResistanceNoRein(rho, d, fck, 1.5);
                        _VRdc.Value = _beambdim.Value * d * vRdc / 1000;

                        double checkc = 2;

                        checkc = (_VEd.Value / _VRdc.Value) + (_TEd.Value / _TRdc.Value);

                        //Torsion link requirements

                        if (_TEd.Value > 0)
                        {
                            if (checkc > 1)
                            {
                                Formula f10 = new Formula();
                                f10.Narrative = "Shear and Torsional resistance concrete";
                                f10.Expression = new List<string>();
                                f10.Ref = "(6.31)";
                                f10.Expression.Add(_TRdc.Symbol + "=2 f_{ctd} " + _Areak.Symbol + _teff.Symbol + "=" + Math.Round(_TRdc.Value, 2) + _TRdc.Unit);
                                f10.Expression.Add(_VRdc.Symbol + @"=b_{w} d C_{Rd,c} k (100 \rho_{I} f_{ck})^{1/3}=" + Math.Round(_VRdc.Value, 2) + _VRdc.Unit);
                                f10.Expression.Add(String.Format("{0}/{1} + {2}/{3} = {4}>1", _VEd.Symbol, _VRdc.Symbol, _TEd.Symbol, _TRdc.Symbol, Math.Round(checkc, 3)));
                                f10.Conclusion = "Additional torsional reinforcement required";
                                expressions.Add(f10);

                                _Asi.Value = A_Si_add(_TEd.Value, _Areak.Value, theta, Uperik, fyd);

                                Formula f11 = new Formula();
                                f11.Narrative = "Additional steel requirements";
                                f11.Expression = new List<string>();
                                f11.Ref = "(6.28)";
                                f11.Expression.Add(_Asi.Symbol + "=(" + _TEd.Symbol + @"cot\theta u_{k})/(2 f_{yd}" + _Areak.Symbol + @")=" + Math.Round(_Asi.Value, 2) + _Asi.Unit);
                                f11.Conclusion = "Additional steel to be distrubted around section perimeter";
                                expressions.Add(f11);

                            }
                            else
                            {
                                Formula f9 = new Formula();
                                f9.Narrative = "Shear and Torsional resistance concrete";
                                f9.Expression = new List<string>();
                                f9.Ref = "(6.31)";
                                f9.Expression.Add(_TRdc.Symbol + "=2 f_{ctd} " + _Areak.Symbol + _teff.Symbol + "=" + Math.Round(_TRdc.Value, 2) + _TRdc.Unit);
                                f9.Expression.Add(_VRdc.Symbol + @"=b_{w} d C_{Rd,c} k (100 \rho_{I} f_{ck})^{1/3}=" + Math.Round(_VRdc.Value, 2) + _VRdc.Unit);
                                f9.Expression.Add(String.Format("{0}/{1} + {2}/{3} = {4}<1", _VEd.Symbol, _VRdc.Symbol, _TEd.Symbol, _TRdc.Symbol, Math.Round(checkc, 3)));
                                f9.Conclusion = "No Additional Torsion reinforcement required";
                                f9.Status = CalcStatus.PASS;
                                expressions.Add(f9);

                            }

                        }

                        //Calc shear links
                        var res = Shear_links(_VEd.Value, fyd, theta, d, _beambdim.Value, _Cover.Value, linkdiasel, _Minlink_spacing.Value, fck, _fy.Value, z);
                        _Linkspacing.Value = res.Item1;
                        _linklegs.Value = res.Item2;
                        _VRds.Value = res.Item3;
                        rholink = res.Item4;

                        if (_linklegs.Value < 2)
                        {
                            Formula f12 = new Formula();
                            f12.Narrative = "Section cannot acommodate shear link, increase section width";
                            f12.Conclusion = "Fail";
                            f12.Status = CalcStatus.FAIL;
                            expressions.Add(f12);
                        }
                        else if (_VRds.Value > _VEd.Value)
                        {
                            Formula f13 = new Formula();
                            f13.Narrative = "Shear link requirements";
                            f13.Expression = new List<string>();
                            f13.Ref = "(6.8)";
                            f13.Expression.Add(@"link diameter=" + Math.Round(linkdiasel, 0) + _linkdiameter.Unit);
                            f13.Expression.Add(_Linkspacing.Symbol + "=" + Math.Round(_Linkspacing.Value, 0) + _Linkspacing.Unit);
                            f13.Expression.Add(_linklegs.Symbol + "=" + Math.Round(_linklegs.Value, 0));
                            f13.Expression.Add(@"\rho_{links}=" + Math.Round(rholink, 4));
                            f13.Expression.Add(_VRds.Symbol + @"=(A_{sw}/s)zf_{ywd}cot\theta=" + Math.Round(_VRds.Value, 2) + _VRds.Unit);
                            f13.Conclusion = "Pass";
                            f13.Status = CalcStatus.PASS;
                            expressions.Add(f13);
                        }
                        else
                        {
                            Formula f12 = new Formula();
                            f12.Narrative = "Shear link cannot accomodate load, try increasing link diameter or section size";
                            f12.Ref = "(6.8)";
                            f12.Conclusion = "Fail";
                            f12.Status = CalcStatus.FAIL;
                            expressions.Add(f12);
                        }
                        break;
                    }
                    else
                    {
                        if (theta > Math.PI / 4)
                        {
                            Formula f8 = new Formula();
                            f8.Narrative = "Shear and Torsional max capacity";
                            f8.Expression = new List<string>();
                            f8.Expression.Add(_TRdmax.Symbol + @"=2 \nu \alpha_{cw} f_{cd} " + _Areak.Symbol + _teff.Symbol + @"sin\theta cos\theta = " + Math.Round(_TRdmax.Value, 2) + _TRdmax.Unit);
                            f8.Expression.Add(_VRdmax.Symbol + @"=(\alpha_{cw} b_{w} z \nu_{1} f_{cd})/(cot\theta + tan\theta)=" + Math.Round(_VRdmax.Value, 2) + _VRdmax.Unit);
                            f8.Expression.Add(string.Format("{0}/{1} + {2}/{3} = {4} > 1", _TEd.Symbol, _VRdmax.Symbol, _TEd.Symbol, _TRdmax.Symbol, Math.Round(checkmax, 3)));
                            f8.Conclusion = "Max capacity of section exceeded, Increase section size";
                            f8.Status = CalcStatus.FAIL;
                            expressions.Add(f8);
                            break;
                        }
                        else
                        {
                            theta = theta + 0.01;
                        }
                    }
                }

            }
            else
            {
                Formula f5 = new Formula();
                f5.Narrative = "Bending Reinforcement check";
                f5.Conclusion = "Fail - Not able to find suitable reinforcement layout";
                f5.Status = CalcStatus.FAIL;
                expressions.Add(f5);
            }

        }

        private List<SkiaSharp.SKBitmap> generateImage()
        {
            double sf = 1 ;

            DisplayDataSet dataset = DisplayDataSet.GetDisplayDataSet();
            dataset.AddFormattingInstruction("Beam",
                                             new SkiaSharp.SKPaint { StrokeWidth = 2, Color = SkiaSharp.SKColors.Gray },
                                             new SkiaSharp.SKPaint { StrokeWidth = 2, Color = SkiaSharp.SKColors.Transparent });
            dataset.AddFormattingInstruction("Rebar",
                                             new SkiaSharp.SKPaint { StrokeWidth = 2, Color = SkiaSharp.SKColors.Brown },
                                             new SkiaSharp.SKPaint { StrokeWidth = 2, Color = SkiaSharp.SKColors.Transparent });
            dataset.AddFormattingInstruction("Link",
                                             new SkiaSharp.SKPaint { StrokeWidth = 2, Color = SkiaSharp.SKColors.Transparent },
                                             new SkiaSharp.SKPaint { StrokeWidth = (float)linkdiasel, Color = SkiaSharp.SKColors.Brown });
            StructuralDrawing2D.StructuralDrawing2D drawing = new StructuralDrawing2D.StructuralDrawing2D(dataset);

            // axis
            var pathX = new SkiaSharp.SKPath();
            pathX.MoveTo((float)(-_beambdim.Value * sf), 0);
            pathX.LineTo((float)(_beambdim.Value * sf), 0);
            drawing.AddElement(DisplayFormatPreset.Gridline, pathX, false);

            var pathY = new SkiaSharp.SKPath();
            pathY.MoveTo(0, (float)(-_beamhdim.Value * sf));
            pathY.LineTo(0, (float)(_beamhdim.Value * sf));
            drawing.AddElement(DisplayFormatPreset.Gridline, pathY, false);

            // beam shape
            var path = new SkiaSharp.SKPath();
            var rect = new SkiaSharp.SKRect((float)(-_beambdim.Value / 2 * sf), (float)(_beamhdim.Value / 2 * sf), (float)(_beambdim.Value / 2 * sf), (float)(-_beamhdim.Value / 2 * sf));
            path.AddRect(rect);
            drawing.AddElement("Beam", path, true);


            // rebars btm
            double dx = (_beambdim.Value - 2 * (_Cover.Value + linkdiasel) - _Bardia.Value) / (_notensbars.Value - 1);


            for (int i = 0; i < _notensbars.Value; i++)
            {

                var pathR = new SkiaSharp.SKPath();

                double x = _Cover.Value + linkdiasel + _Bardia.Value / 2 + i * dx - _beambdim.Value / 2;
                double y = -_beamhdim.Value / 2 + (_Cover.Value + linkdiasel + _Bardia.Value / 2);
                pathR.AddCircle((float)(x * sf), (float)(y * sf), (float)(_Bardia.Value / 2 * sf));

                drawing.AddElement("Rebar", pathR, true);

            }

            // rebars top
            double dxc = (_beambdim.Value - 2 * (_Cover.Value + linkdiasel) - _Bardiacomp.Value) / (_nocompbars.Value - 1);

            for (int i = 0; i < _nocompbars.Value; i++)
            {

                var pathRC = new SkiaSharp.SKPath();

                double x = _Cover.Value + linkdiasel + _Bardiacomp.Value / 2 + i * dxc - _beambdim.Value / 2;
                double y = _beamhdim.Value / 2 - (_Cover.Value + linkdiasel + _Bardiacomp.Value / 2);
                pathRC.AddCircle((float)(x * sf), (float)(y * sf), (float)(_Bardiacomp.Value / 2 * sf));

                drawing.AddElement("Rebar", pathRC, true);

            }
            

            //rebar link

            var pathlink = new SkiaSharp.SKPath();
            double linkx = _beambdim.Value/2 - _Cover.Value - linkdiasel/2;
            double linky = _beamhdim.Value/2 -  _Cover.Value - linkdiasel/2;
            double linkrad;

            if (linkdiasel > 16)
            {
                linkrad = 3.5 * linkdiasel;
            }
            else
            {
                linkrad = 2 * linkdiasel;
            }

            var rectli = new SkiaSharp.SKRect((float)(-linkx), (float)(linky), (float)(linkx), (float)(-linky));
           
            pathlink.AddRoundedRect(rectli, (float)(linkrad), (float)(linkrad));
            drawing.AddElement( "Link" ,pathlink, true);

            var bitmap = drawing.GenerateBitmapImage(1000,1000,100);
            return new List<SKBitmap> { bitmap};
        }

        //Concrete resistance to Torsion maximum tre
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
        public Double V_res_concrete_max(Double bw, Double d, Double conccompdes, Double conccomp, double angle, Double z)
        {
            Double v1;
            Double Alphacw;
            Double VRDMAX;

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

        //Shear links
        public Tuple<double,double,double,double> Shear_links(Double Ved, Double fyd, double theta, Double d, Double b, Double c, Double linkdia, Double Minlinkspacing, double fck, double fyk, Double z)
        {
            Double s;
            Double linkno;
            Double Asw;
            Double VRds;
            Double link_tang_S;
            Double fywd;
            Double base_s;
            Double rho;
            double rhomin;
            double stmax;

            fywd = fyd;
            linkno = 2;
            
            //max spacing along beam
            s = Math.Round((0.75 * d) / 25) * 25; 
            if (s > 0.75 * d)
                {
                    s = s - 25;
                }
            base_s = s;

            //max tang spacing check of links
            stmax = Math.Min(0.75 * d, 600);

            //initial max spacing between shear legs
            link_tang_S = (b - 2 * c - linkdia) / (linkno - 1);

            //required number of legs to achieve max spacing rules

            linkno = Math.Round(link_tang_S / stmax);
            link_tang_S = (b - 2 * c - linkdia) / (linkno - 1);

            if (link_tang_S > stmax)
            {
                linkno = linkno + 1;
            }
            
            //min link area check

            Asw = (Math.PI * (linkdia * linkdia) * 0.25) * linkno;
            rho = (Asw)/(s*b);
            rhomin = (0.08 * Math.Sqrt(fck)) / fyk;

            while (rho < rhomin)
            {
                if (s <= Minlinkspacing)
                {
                    linkno = linkno + 1;
                    s = base_s;
                }
                else
                {
                    s = s - 25;
                }

                Asw = (Math.PI * (linkdia * linkdia) * 0.25) * linkno;
                rho = (Asw) / (s * b);

            }

            //Updated base_s to suit minimum rho
            base_s = s;

            //design resistance of shear links, spacing is reduced and numbers increased automatically to achieve Ved

            VRds = (Asw / s) * (z * fywd * (1 / (Math.Tan(theta)))) / 1000;

            while (true)
            {
                    if (Ved < VRds)
                    {
                        break;
                    }
                    else
                    {
                        if (s <= Minlinkspacing)
                        {
                            linkno = linkno + 1;
                            link_tang_S = (b - 2 * c - linkdia)/(linkno-1);

                            if (link_tang_S < Minlinkspacing)
                                {
                                    linkno = linkno - 1;
                                    link_tang_S = b - linkno * c - linkdia;
                                    break;
                                }
                            else
                                {
                                    s = base_s;
                                }       
                        }
                        else
                        {
                            s = s - 25;
                        }
                    }

                    Asw = (Math.PI * (linkdia * linkdia) * 0.25) * linkno;
                    VRds = (Asw / s) * (z * fywd * (1 / (Math.Tan(theta)))) / 1000;

            }

            return new Tuple<double, double, double, double>(s,linkno,VRds,rho);
        }

        //Bending reinforcement required
        public Tuple<double,double,double,double, double,double,double> BendingAreq(Double bendingMom,Double effd,Double secwidth, Double charCompStr,Double rebaryield, double effd2, double fyd)
        {
            Double K;
            Double Kdash;
            double k1;
            Double k2;
            double k3;
            double k4;
            var concprop = ConcProperties.ByGrade(Convert.ToString(charCompStr));
            Double delta;
            Double xu;
            Double z;
            Double As;
            Double Asmin;
            Double Acs;
            double strngred;
            double x;
            double xred;
            double rho;
            double n;



            strngred = 1;
            rho = 0.8;
            n = 1;

            //Strength reduction when greater than 50 Mpa when required for determining z,

            if (concprop.fck > 50)
            {
                rho = (0.8 - (fck - 50) / 400);
                n = 1 - (fck - 50) / 200;
                xred = (1 / (rho * 0.5));
                strngred = (rho/0.8) * (n/1) * (xred/2.5);
            }

            k1 = 0.4; // as per NA
            k2 = 0.6 + (0.0014 / (concprop.Epsiloncu2/1000));
            k3 = k1;
            k4 = k2;

            xu = (concprop.Epsiloncu3*effd) / (concprop.Epsiloncu3 + (fyd / 200));

            delta = Math.Min( k1 + (k2 * xu / effd),1);

            K = bendingMom * 1000000 / (secwidth * Math.Pow(effd, 2) * charCompStr);

            if (K > (1 / (3.53 * strngred)))
            {
                z = 1;
                As = 0;
                Acs = 0;
                Kdash = 0.168;
                x = 0;
                Asmin = Math.Min((0.26 * concprop.fctm * secwidth * effd) / (rebaryield), 0.0013 * secwidth * effd);

                return new Tuple<double, double,double, double, double,double,double>(As, z,K,Kdash,Acs,Asmin,x);
            }

            Kdash = Math.Min((2 * n * (0.85 / 1.5)) * (1 - rho * ((delta-k3)/(2*k4)))*(rho*((delta-k3)/(2*k4))), 0.168); //Limit of K' limited to 0.168 as per Concrete centre guidance

            if (K < Kdash)
            {
                Acs = 0;
                Asmin = Math.Max((0.26 * concprop.fctm * secwidth*effd) / (rebaryield), 0.0013 * secwidth * effd);

                z = Math.Min( (effd / 2) * (1+Math.Pow(1-3.53*strngred*K,0.5)),0.95*effd);
                x = (effd - z) / (rho * 0.5);
                As = (bendingMom * 1000000) / (fyd * z);
 
                if (As < Asmin)
                {
                    As = Asmin;
                }


            }
            else
            {
                Asmin = Math.Max((0.26 * concprop.fctm * secwidth * effd) / (rebaryield), 0.0013 * secwidth * effd);

                z = (effd / 2) * (1 + Math.Pow(1 - 3.53 * strngred * Kdash, 0.5));
                x = (effd - z) / (rho * 0.5);

                Double Mdash = secwidth * Math.Pow(effd, 2) * concprop.fck*(K - Kdash);
                Acs = (Mdash) / (fyd*(effd-effd2));

                As = ((Kdash*concprop.fck*secwidth*Math.Pow(effd,2)) / (fyd * z))+Acs;

                if (As < Asmin)
                {
                    As = Asmin;
                }
            }

            return new Tuple<double, double, double,double,double, double,double>(As, z,K,Kdash,Acs,Asmin,x);
        }

        //Bending reinforcement spacing 
        // returns highest value of bars which either satisfies As,req or fits within section
        public Tuple<double,double, double> BendingBarsspacing(Double Asreq, Double mindia, Double minspac, Double cover,Double width,double link)
        {
            Double Asprov;
            Double Bardia;
            Double nobars;
            Double spac;

            nobars = 2;
            Bardia = mindia;

            Asprov = Math.PI * 0.25 * (Bardia*Bardia) * nobars;

            //check spacing
            spac = (width - 2 * cover - 2 * link) / (nobars - 1) - Bardia;
            double minspacgov = Math.Max(minspac, Math.Min(25, Bardia));

            while(true)
            {

                if (spac < minspacgov)
                {
                    //bars cannot fit, provide final number of bars as a return which doesn't provide As,req
                    nobars = nobars-1;
                    Asprov = Math.PI * 0.25 * Bardia * Bardia * nobars;
                    spac = (width - 2 * cover - 2 * link) / (nobars - 1) - Bardia;
                    break;
                }
                else
                {
                    if (Asprov < Asreq)//strength check
                    {
                        nobars = nobars + 1;
                        Asprov = Math.PI * 0.25 * Bardia*Bardia * nobars;
                        spac = (width - 2 * cover - 2 * link) / (nobars - 1) - Bardia;
                    }

                    else
                    {   
                        break;
                    }

                }


            }

            return new Tuple<double, double,double>(nobars, minspacgov, spac);
        }

        //crack width calc
        public Tuple<double, double, double, double> Crackcontrol(Double fck, Double fsls, double bardia, double nobars, Double d, double Secwidth, double secdepth, double x, double cover, double linkdia, double fy)
        {
            Double crwidth;
            Double epsmepcm;
            double kt = 0.4;
            var concprop = ConcProperties.ByGrade(Convert.ToString(fck));
            double fcteff = concprop.fctm;
            double rhopeff;
            double Aceff;
            double hcef;
            double alphae;
            double Es;
            double Srmax;
            double As;
            double s;
            double c;
            double k1;
            double k2;
            double k3;
            double k4;
            double Asmin;
            double kc;
            double k;

            As = nobars * Math.PI * 0.25 * Math.Pow(bardia, 2);
            s = (Secwidth - 2 * (cover + linkdia) - bardia) / (nobars - 1);

            //cover to main rebar
            c = cover + linkdia;

            Es = 200000;
            alphae = Es / (concprop.Ecm*1000);

            hcef = Math.Min(2.5 * (secdepth - d), Math.Min(secdepth / 2, (secdepth - x) / 3));
            Aceff = Secwidth * hcef;
            rhopeff = As / Aceff;

            epsmepcm =Math.Max( (fsls-kt*(fcteff/rhopeff)*(1+alphae*rhopeff))/Es, 0.6*fsls/Es );

            k1 = 0.8;
            k2 = 0.5;
            k3 = 3.4;
            k4 = 0.425;
            kc = 0.4;
            k = 1;

            if (s <= 5 * (cover + bardia * 0.5))
            {
                // spacing as per (7.11) in EC2
                Srmax = k3 * c + (k1 * k2 * k4 * bardia) / rhopeff;

            }
            else
            {
                //Spacing as per (7.14) in EC2
                Srmax = 1.3 * (secdepth - x);

            }

            crwidth = Srmax * epsmepcm;

            //minimum steel requirements
            Asmin = (kc*k*fcteff*Aceff) / fy;

            return new Tuple<double, double, double, double>(Asmin, crwidth, Srmax, epsmepcm);
        }
    }
}