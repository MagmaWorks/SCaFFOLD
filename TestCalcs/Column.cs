using InteractionDiagram3D;
using MWGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWGeometry;

namespace TestCalcs
{
    public enum FireExposition { OneSide, MoreThanOneSide };
    
    public class Column
    {
        public string Name { get; set; }
        // Geometry
        public double LX { get; set; } = 350;
        public double LY { get; set; } = 350;
        public double Length { get; set; } = 3150;
        //public Point3D Point1 { get; set; }
        //public Point3D Point2 { get; set; }
        public double Angle { get; set; }

        // Material
        public Concrete ConcreteGrade { get; set; }
        public Concrete CustomGrade { get; set; }
        public double MaxAggSize { get; set; } = 20;

        // Loads
        public double MxTop { get; set; } = 0;
        public double MxBot { get; set; } = 0;
        public double MyTop { get; set; } = 0;
        public double MyBot { get; set; } = 0;
        public double P { get; set; } = 0;

        public double Mxd { get; set; }
        public double Myd { get; set; }

        // Design
        public double EffectiveLength { get; set; } = 0.7;
        public double CoverToLinks { get; set; } = 40;
        public Steel steelGrade = new Steel("500B", 500);
        public int BarDiameter { get; set; } = 16;
        public int LinkDiameter { get; set; } = 10;
        public int NRebarX { get; set; } = 3;
        public int NRebarY { get; set; } = 3;
        public int R { get; set; } = 120; // fire resistance in min
        public FireExposition SidesExposed { get; set; } = FireExposition.MoreThanOneSide;

        public List<MWPoint3D> diagramVertices = new List<MWPoint3D>();
        public List<Tri3D> diagramFaces;

        // Checks
        public bool CapacityCheck { get; set; } = false;
        public bool FireCheck { get; set; } = false;
        public bool SpacingCheck { get; set; } = false;
        public bool MinMaxSteelCheck { get; set; } = false;

        public List<FireData> fireTable = new List<FireData>();
        const double gs = 1.15;
        const double gc = 1.5;
        const double acc = 0.85;

        public Dictionary<double, double> CarbonData = new Dictionary<double, double>();
        public Dictionary<double, double[]> SteelCosts = new Dictionary<double, double[]>();
        public Dictionary<double, double[]> ConcreteCosts = new Dictionary<double, double[]>();

        const double concreteVolMass = 2.5e3;
        const double steelVolMass = 7.5e3;

        // Optimisation
        public double Cost { get; set; } = 0;

        public Column()
        { }

        /*public Column(ETABSColumnDesig_Plugin.Column c0)
        {
            Name = c0.name;
            LX = c0.LX;
            LY = c0.LY;
            Length = c0.length;
            CustomGrade = new Concrete("Custom", c0.fc, c0.E);
            ConcreteGrade = CustomGrade;
            MxTop = Math.Ceiling(c0.MxTop);
            MxBot = Math.Ceiling(c0.MxBot);
            MyTop = Math.Ceiling(c0.MyTop);
            MyBot = Math.Ceiling(c0.MyBot);
            P = Math.Ceiling(Math.Abs(c0.P));
            Point1 = c0.Point1;
            Point2 = c0.Point2;
            Angle = c0.Angle;

            //NRebarX = (int)((LX - 2 * CoverToLinks) / 90);
            //NRebarY = (int)((LY - 2 * CoverToLinks) / 90);

            SetFireData();
            InitializeCarbonData();
            InitializeConcreteCosts();
            InitializeSteelCosts();
        }*/

        public Column Clone()
        {
            Column col = new Column();
            col.Name = this.Name;
            col.LX = this.LX;
            col.LY = this.LY;
            col.Length = this.Length;
            //col.Point1 = this.Point1;
            //col.Point2 = this.Point2;
            col.Angle = this.Angle;

            col.CoverToLinks = this.CoverToLinks;
            col.EffectiveLength = this.EffectiveLength;
            col.fireTable = this.fireTable;
            col.LinkDiameter = this.LinkDiameter;
            col.MaxAggSize = this.MaxAggSize;
            col.R = this.R;

            col.CustomGrade = this.CustomGrade;
            col.ConcreteGrade = this.ConcreteGrade;
            col.MxTop = this.MxTop;
            col.MxBot = this.MxBot;
            col.MyTop = this.MyTop;
            col.MyBot = this.MyBot;
            col.P = this.P;

            col.NRebarX = this.NRebarX;
            col.NRebarY = this.NRebarY;
            col.BarDiameter = this.BarDiameter;

            //col.steelGrade = this.steelGrade;
            col.diagramVertices = this.diagramVertices;
            col.diagramFaces = this.diagramFaces;

            col.CapacityCheck = this.CapacityCheck;
            col.FireCheck = this.FireCheck;
            col.SpacingCheck = this.SpacingCheck;

            col.Cost = this.Cost;

            return col;
        }

        public void GetInteractionDiagram()
        {
            List<Composite> composites = new List<Composite>();

            // Creation of the concrete section
            Material concrete = new Material(ConcreteGrade.Name, MatYpe.Concrete, 0.85 * ConcreteGrade.Fc / 1.5, 3);
            ConcreteSection cs = new ConcreteSection(new List<MWPoint2D>()
                                                    {
                                                        new MWPoint2D(0,0),
                                                        new MWPoint2D(LX,0),
                                                        new MWPoint2D(LX,LY),
                                                        new MWPoint2D(0,LY)
                                                    },
                                                    concrete);
            /*ConcreteSection cs = new ConcreteSection(new List<Point>()
                                                    {
                                                        new Point(-0.5,-0.5),
                                                        new Point(0.5,-0.5),
                                                        new Point(0.5,0.5),
                                                        new Point(-0.5,0.5)
                                                    },
            concrete);*/
            composites.Add(cs);

            // Creation of the rebars
            //Material steel = new Material(steelGrade.Name, MatYpe.Steel, steelGrade.Fy / 1.15, steelGrade.Fy / 1.15);
            Material steel = new Material(steelGrade.Name, MatYpe.Steel, steelGrade.Fy / 1.15, 0);
            double xspace = (LX - 2 * (CoverToLinks + LinkDiameter + BarDiameter / 2)) / (NRebarX - 1);
            double yspace = (LY - 2 * (CoverToLinks + LinkDiameter + BarDiameter / 2)) / (NRebarY - 1);
            for (int i = 0; i < NRebarX; i++)
            {
                var x = CoverToLinks + LinkDiameter + BarDiameter / 2 + i * xspace;
                for (int j = 0; j < NRebarY; j++)
                {
                    var y = CoverToLinks + LinkDiameter + BarDiameter / 2 + j * yspace;
                    Rebar r = new Rebar(new MWPoint2D(x, y), Math.PI * Math.Pow(BarDiameter / 2, 2), steel);
                    composites.Add(r);
                }
            }

            Diagram d = new Diagram(composites);

            diagramVertices = d.vertices;
            diagramFaces = d.faces;

        }

        public bool isInsideCapacity()
        {
            GetDesignMoments();
            return isInsideInteractionDiagram(this.diagramFaces, this.diagramVertices);
        }

        public bool isInsideInteractionDiagram(List<Tri3D> faces, List<MWPoint3D> vertices)
        {
            GetDesignMoments();
            MWPoint3D p0 = new MWPoint3D
            (
                2 * vertices.Min(x => x.X),
                0,
                0
            );

            MWPoint3D p = new MWPoint3D
            (
                Mxd,
                Myd,
                -P
            );
            int compt = 0;

            for (int i = 0; i < faces.Count; i++)
            {
                MWPoint3D pInter0 = Polygon3D.PlaneLineIntersection(new MWPoint3D[] { p0, p }, faces[i].Points);
                if (pInter0.X != double.NaN)
                {
                    MWPoint3D pInter = pInter0;
                    MWVector3D v = Vectors3D.VectorialProduct(new MWVector3D(p0.X - pInter.X, p0.Y - pInter.Y, p0.Z - pInter.Z),
                                                              new MWVector3D(p.X - pInter.X, p.Y - pInter.Y, p.Z - pInter.Z));

                    List<MWPoint3D> pts = faces[i].Points;
                    double a1 = Vectors3D.TriangleArea(new MWVector3D(pts[0].X - pInter.X, pts[0].Y - pInter.Y, pts[0].Z - pInter.Z),
                                                       new MWVector3D(pts[1].X - pInter.X, pts[1].Y - pInter.Y, pts[1].Z - pInter.Z));
                    double a2 = Vectors3D.TriangleArea(new MWVector3D(pts[1].X - pInter.X, pts[1].Y - pInter.Y, pts[1].Z - pInter.Z),
                                                       new MWVector3D(pts[2].X - pInter.X, pts[2].Y - pInter.Y, pts[2].Z - pInter.Z));
                    double a3 = Vectors3D.TriangleArea(new MWVector3D(pts[2].X - pInter.X, pts[2].Y - pInter.Y, pts[2].Z - pInter.Z),
                                                       new MWVector3D(pts[0].X - pInter.X, pts[0].Y - pInter.Y, pts[0].Z - pInter.Z));
                    double a0 = Vectors3D.TriangleArea(new MWVector3D(pts[1].X - pts[0].X, pts[1].Y - pts[0].Y, pts[1].Z - pts[0].Z),
                                                       new MWVector3D(pts[2].X - pts[0].X, pts[2].Y - pts[0].Y, pts[2].Z - pts[0].Z));
                    if (Math.Abs(a1 + a2 + a3 - a0) < 10)
                        compt++;
                }
            }

            if (compt % 2 == 0)
                return false;
            else
                return true;
        }

        public bool CheckSpacing()
        {
            List<double> sizes = new List<double>() { this.BarDiameter, this.MaxAggSize + 5, 20 };
            double smin = sizes.Max();
            double sx = (this.LX - 2 * (this.CoverToLinks + this.LinkDiameter) - this.BarDiameter) / (this.NRebarX - 1);
            double sy = (this.LY - 2 * (this.CoverToLinks + this.LinkDiameter) - this.BarDiameter) / (this.NRebarY - 1);
            if (sx >= smin && sy >= smin)
                return true;
            return false;
        }

        public bool CheckFire()
        {
            double Nrd = ((this.NRebarX * this.NRebarY - 4) * Math.PI * Math.Pow(this.BarDiameter / 2, 2) * this.steelGrade.Fy + this.LX * this.LY * this.ConcreteGrade.Fc) / 1E3;
            double mufi = 0.7 * this.P / Nrd;

            if (fireTable.Count == 0) SetFireData();

            // Eurocode Table 5.2.1a
            double afi = 0;
            mufi = (mufi <= 0.35) ? 0.2 : ((mufi <= 0.6) ? 0.5 : 0.7);
            List<FireData> fdata = fireTable.Where(x => x.mu == mufi && x.R == this.R && x.sidesExposed == this.SidesExposed).ToList();
            fdata = fdata.OrderByDescending(x => x.minDimension).ToList();
            for (int i = 0; i < fdata.Count; i++)
            {
                if (this.LX >= fdata[i].minDimension && this.LY >= fdata[i].minDimension)
                {
                    afi = fdata[i].axisDistance;
                    break;
                }
            }
            double cminb = Math.Max(this.LinkDiameter, this.BarDiameter - this.LinkDiameter);
            double cnommin = Math.Max(afi - this.BarDiameter / 2 - this.LinkDiameter, cminb + 10);

            if (afi == 0)
            {
                return false;
            }
            else
            {
                if (this.CoverToLinks >= cnommin)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CheckSteelQtty()
        {
            double Ac = LX * LY;
            double Asmin = Math.Max(0.1 * P / (steelGrade.Fy * 1e-3 / gs), 0.002 * Ac);
            double Asmax = 0.04 * Ac;
            double As = 2 * (NRebarX + NRebarY - 2) * Math.PI * Math.Pow(BarDiameter / 2, 2);

            if (As > Asmax)
                return false;
            else if (As < Asmin)
                return false;

            return true;
        }

        public void GetDesignMoments()
        {
            List<double> sizes = new List<double>() { this.BarDiameter, this.MaxAggSize + 5, 20 };
            double smin = sizes.Max();
            double sx = (this.LX - 2 * (this.CoverToLinks + this.LinkDiameter) - this.BarDiameter) / (this.NRebarX - 1);
            double sy = (this.LY - 2 * (this.CoverToLinks + this.LinkDiameter) - this.BarDiameter) / (this.NRebarY - 1);

            double abar = Math.PI * Math.Pow(this.BarDiameter / 2, 2);
            double As = (this.NRebarX * this.NRebarY - (this.NRebarX - 2) * (this.NRebarY - 2)) * Math.PI * Math.Pow(this.BarDiameter / 2.0, 2);
            double[] dxs = new double[this.NRebarY];
            dxs[0] = this.LY - this.CoverToLinks - this.LinkDiameter - this.BarDiameter / 2;

            for (int i = 1; i < this.NRebarY; i++)
                dxs[i] = dxs[i - 1] - sy;

            double Isx = 2 * abar * this.NRebarX * Math.Pow((dxs[0] - this.LY / 2), 2);
            for (int i = 1; i < this.NRebarY / 2; i++)
            {
                Isx += 2 * abar * 2 * Math.Pow((dxs[i] - this.LY / 2), 2);
            }

            double isx = Math.Sqrt(Isx / As);
            double dx = this.LY / 2 + isx;

            double[] dys = new double[this.NRebarX];
            dys[0] = this.LX - this.CoverToLinks - this.LinkDiameter - this.BarDiameter / 2;

            for (int i = 1; i < this.NRebarX; i++)
                dys[i] = dys[i - 1] - sx;

            double Isy = 2 * abar * this.NRebarY * Math.Pow((dys[0] - this.LX / 2), 2);
            for (int i = 1; i < this.NRebarX / 2; i++)
                Isy += 2 * abar * 2 * Math.Pow((dys[i] - this.LX / 2), 2);

            double isy = Math.Sqrt(Isy / As);
            double dy = this.LX / 2 + isy;

            double l0 = this.EffectiveLength * this.Length;

            double ix = this.LY / Math.Sqrt(12);
            double lambdax = l0 / ix;

            double iy = this.LX / Math.Sqrt(12);
            double lambday = l0 / iy;

            double ei = l0 / 400;
            double M01x = Math.Min(Math.Abs(this.MxTop), Math.Abs(this.MxBot)) + ei * this.P / 1E3;
            double M02x = Math.Max(Math.Abs(this.MxTop), Math.Abs(this.MxBot)) + ei * this.P / 1E3;

            double omega = As * this.steelGrade.Fy / gs /
                (this.LX * this.LY * acc * this.ConcreteGrade.Fc / gc);
            double B = Math.Sqrt(1 + 2 * omega);
            double rmx = M01x / M02x;
            double C = 1.7 - rmx;
            double n = this.P / (this.LX * this.LY * acc * this.ConcreteGrade.Fc / gc) * 1E3;
            double lambdaxlim = 20 * 0.7 * B * C / Math.Sqrt(n);
            bool secondorderx = false;

            if (lambdax < lambdaxlim)
            {
            }
            else
            {
                secondorderx = true;
            }

            double M01y = Math.Min(Math.Abs(this.MyTop), Math.Abs(this.MyBot)) + ei * this.P / 1E3;
            double M02y = Math.Max(Math.Abs(this.MyTop), Math.Abs(this.MyBot)) + ei * this.P / 1E3;

            double rmy = M01y / M02y;
            C = 1.7 - rmy;
            double lambdaylim = 20 * 0.7 * B * C / Math.Sqrt(n);
            bool secondordery = false;

            if (lambday < lambdaylim)
            {
            }
            else
            {
                secondordery = true;
            }

            if (!secondorderx)
            {
                double Medx = Math.Max(M02x, this.P * Math.Max(this.LY * 1E-3 / 30, 20 * 1E-3));
                this.Mxd = Math.Round(Medx, 1);
            }
            else
            {
                double u = 2 * (this.LX + this.LY);
                double nu = 1 + omega;
                double Kr = Math.Min(1, (nu - n) / (nu - 0.4));
                double d = this.LY - this.CoverToLinks - this.LinkDiameter - this.BarDiameter / 2;
                double eyd = this.steelGrade.Fy / gs / this.steelGrade.E / 1E3;
                double r0 = 0.45 * dx / eyd;
                double h0 = 2 * this.LY * this.LX / u;
                double alpha1 = Math.Pow(35 / (this.ConcreteGrade.Fc + 8), 0.7);
                double alpha2 = Math.Pow(35 / (this.ConcreteGrade.Fc + 8), 0.2);
                double phiRH = (1 + (0.5 / (0.1 * Math.Pow(h0, 1.0 / 3))) * alpha1) * alpha2;
                double bfcm = 16.8 / Math.Sqrt(this.ConcreteGrade.Fc + 8);
                double bt0 = 1 / (0.1 + Math.Pow(7, 0.2));
                double phi0 = phiRH * bfcm * bt0;
                double phiInf = phi0;
                double phiefy = phiInf * 0.8;
                double betay = 0.35 + this.ConcreteGrade.Fc / 200 - lambdax / 150;
                double kphiy = Math.Max(1, 1 + betay * phiefy);
                double r = r0 / (Kr * kphiy);
                double e2x = Math.Pow(l0, 2) / (r * 10);
                double m2x = this.P * e2x * 1E-3;
                double M0e = 0.6 * M02x + 0.4 * M01x;
                List<double> Ms = new List<double>() { M02x, M0e + m2x, M01x + 0.5 * m2x, Math.Max(this.LY * 1E-3 / 30, 20 * 1E-3) * this.P };
                double Medx = Ms.Max();
                this.Mxd = Math.Round(Medx, 1);
            }

            if (!secondordery)
            {
                double Medy = Math.Max(M02y, this.P * Math.Max(this.LX * 1E-3 / 30, 20 * 1E-3));
                this.Myd = Math.Round(Medy, 1);
            }
            else
            {
                double u = 2 * (this.LX + this.LY);
                double nu = 1 + omega;
                double Kr = Math.Min(1, (nu - n) / (nu - 0.4));
                double d = this.LY - this.CoverToLinks - this.LinkDiameter - this.BarDiameter / 2;
                double eyd = this.steelGrade.Fy / gs / this.steelGrade.E / 1E3;
                double r0 = 0.45 * dy / eyd;
                double h0 = 2 * this.LY * this.LX / u;
                double alpha1 = Math.Pow(35 / (this.ConcreteGrade.Fc + 8), 0.7);
                double alpha2 = Math.Pow(35 / (this.ConcreteGrade.Fc + 8), 0.2);
                double phiRH = (1 + (0.5 / (0.1 * Math.Pow(h0, 1.0 / 3))) * alpha1) * alpha2;
                double bfcm = 16.8 / Math.Sqrt(this.ConcreteGrade.Fc + 8);
                double bt0 = 1 / (0.1 + Math.Pow(7, 0.2));
                double phi0 = phiRH * bfcm * bt0;
                double phiInf = phi0;
                double phiefy = phiInf * 0.8;
                double betay = 0.35 + this.ConcreteGrade.Fc / 200 - lambday / 150;
                double kphiy = Math.Max(1, 1 + betay * phiefy);
                double r = r0 / (Kr * kphiy);
                double e2y = Math.Pow(l0, 2) / (r * 10);
                double m2y = this.P * e2y * 1E-3;
                double M0e = 0.6 * M02y + 0.4 * M01y;
                List<double> Ms = new List<double>() { M02y, M0e + m2y, M01y + 0.5 * m2y, Math.Max(this.LX * 1E-3 / 30, 20 * 1E-3) * this.P };
                double Medy = Ms.Max();
                this.Myd = Math.Round(Medy, 1);

            }

        }

        public double SteelVol()
        {
            return 2 * (this.NRebarX + this.NRebarY - 2) * Math.PI * Math.Pow(this.BarDiameter / 2, 2) * this.Length;
        }

        public double ConcreteVol()
        {
            return LX * LY * Length - SteelVol();
        }

        public double[] GetEmbodiedCarbon()
        {
            if (CarbonData.Count == 0) InitializeCarbonData();
            double Fc = ConcreteGrade.Fc;

            int NbRebar = NRebarX * NRebarY - (NRebarY - 2) * (NRebarX - 2);
            double rebarCarbon = Math.Round(1.46 * SteelVol() / 1E9 * steelVolMass, 1);

            double concreteRatio = 0;
            if (CarbonData.Keys.Contains(ConcreteGrade.Fc))
                concreteRatio = CarbonData[Fc];
            else
            {
                var xsup = CarbonData.First(x => x.Key > Fc).Key;
                var xinf = CarbonData.Reverse().First(x => x.Key < Fc).Key;
                var ysup = CarbonData.First(x => x.Key > Fc).Value;
                var yinf = CarbonData.Reverse().First(x => x.Key < Fc).Value;
                concreteRatio = yinf + (ysup - yinf) / (xsup - xinf) * (Fc - xinf);
            }
            double concreteCarbon = Math.Round(concreteRatio * ConcreteVol() / 1E9 * concreteVolMass, 1);

            return new double[] { concreteCarbon, rebarCarbon, concreteCarbon + rebarCarbon };

        }

        public double[] GetCost()
        {
            if (SteelCosts.Count == 0) InitializeSteelCosts();
            if (ConcreteCosts.Count == 0) InitializeConcreteCosts();
            double steel = SteelVol() / 1e9 * steelVolMass / 1e3 * SteelCosts.FirstOrDefault(x => x.Key == BarDiameter).Value[0];
            double concrete = ConcreteVol() / 1e9 * ConcreteCosts.FirstOrDefault(x => x.Key == Math.Round(ConcreteGrade.Fc)).Value[0];
            double formwork = 2 * (LX + LY) * Length * 45 / 1e6;

            return new double[] { concrete, steel, formwork, steel + concrete + formwork };
        }

        private void InitializeCarbonData()
        {
            CarbonData.Add(32, 0.163);
            CarbonData.Add(40, 0.188);
            CarbonData.Add(50, 0.205);
            CarbonData.Add(60, 0.23);
        }

        private void InitializeSteelCosts()
        {
            SteelCosts.Add(10, new double[] { 1300, 1450 });
            SteelCosts.Add(12, new double[] { 1225, 1372 });
            SteelCosts.Add(16, new double[] { 1100, 1220 });
            SteelCosts.Add(20, new double[] { 998, 1100 });
            SteelCosts.Add(25, new double[] { 950, 1050 });
            SteelCosts.Add(32, new double[] { 910, 1001 });
            SteelCosts.Add(40, new double[] { 870, 960 });
        }

        private void InitializeConcreteCosts()
        {
            ConcreteCosts.Add(30, new double[] { 96.4, 96.4 });
            ConcreteCosts.Add(32, new double[] { 98.5, 98.5 });
            ConcreteCosts.Add(35, new double[] { 101.17, 101.17 });
            ConcreteCosts.Add(40, new double[] { 102.13, 102.13 });
            ConcreteCosts.Add(50, new double[] { 104.03, 104.03 });
        }

        public void SetFireData()
        {
            fireTable.Add(new FireData(30, 0.2, 200, 25));
            fireTable.Add(new FireData(30, 0.5, 200, 25));
            fireTable.Add(new FireData(30, 0.7, 200, 32));
            fireTable.Add(new FireData(30, 0.7, 300, 27));
            fireTable.Add(new FireData(60, 0.2, 200, 25));
            fireTable.Add(new FireData(60, 0.5, 200, 36));
            fireTable.Add(new FireData(60, 0.5, 300, 31));
            fireTable.Add(new FireData(60, 0.7, 250, 46));
            fireTable.Add(new FireData(60, 0.7, 350, 40));
            fireTable.Add(new FireData(90, 0.2, 200, 31));
            fireTable.Add(new FireData(90, 0.2, 300, 25));
            fireTable.Add(new FireData(90, 0.5, 300, 45));
            fireTable.Add(new FireData(90, 0.5, 400, 38));
            fireTable.Add(new FireData(90, 0.7, 350, 53));
            fireTable.Add(new FireData(90, 0.7, 450, 40));
            fireTable.Add(new FireData(120, 0.2, 250, 40));
            fireTable.Add(new FireData(120, 0.2, 350, 35));
            fireTable.Add(new FireData(120, 0.5, 350, 45));
            fireTable.Add(new FireData(120, 0.5, 450, 40));
            fireTable.Add(new FireData(120, 0.7, 350, 57));
            fireTable.Add(new FireData(120, 0.7, 450, 51));
            fireTable.Add(new FireData(180, 0.2, 350, 45));
            fireTable.Add(new FireData(180, 0.5, 350, 63));
            fireTable.Add(new FireData(180, 0.7, 450, 70));
            fireTable.Add(new FireData(240, 0.2, 350, 61));
            fireTable.Add(new FireData(240, 0.5, 350, 75));
            fireTable.Add(new FireData(30, 0.7, 155, 25, FireExposition.OneSide));
            fireTable.Add(new FireData(60, 0.7, 155, 25, FireExposition.OneSide));
            fireTable.Add(new FireData(90, 0.7, 155, 25, FireExposition.OneSide));
            fireTable.Add(new FireData(120, 0.7, 175, 35, FireExposition.OneSide));
            fireTable.Add(new FireData(180, 0.7, 230, 55, FireExposition.OneSide));
            fireTable.Add(new FireData(240, 0.7, 295, 70, FireExposition.OneSide));
        }
    }

    public class FireData
    {
        public int R;
        public double mu;
        public FireExposition sidesExposed;
        public int minDimension;
        public int axisDistance;

        public FireData(int r, double m, int mindim, int a, FireExposition e = FireExposition.MoreThanOneSide)
        {
            R = r;
            mu = m;
            sidesExposed = e;
            minDimension = mindim;
            axisDistance = a;
        }
    }
}
