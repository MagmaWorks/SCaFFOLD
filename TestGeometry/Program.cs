using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using ClipperLib;
using ClipperLib;


using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;


namespace TestGeometry
{
    class Program
    {
        static void Main(string[] args)
        {
            Paths subj = new Paths(2);
            subj.Add(new Path(4));
            subj[0].Add(new IntPoint(180, 200));
            subj[0].Add(new IntPoint(260, 200));
            subj[0].Add(new IntPoint(260, 150));
            subj[0].Add(new IntPoint(180, 150));



            subj.Add(new Path(3));
            subj[1].Add(new IntPoint(215, 160));
            subj[1].Add(new IntPoint(230, 190));
            subj[1].Add(new IntPoint(200, 190));

            Paths clip = new Paths(1);
            clip.Add(new Path(4));
            clip[0].Add(new IntPoint(190, 210));
            clip[0].Add(new IntPoint(240, 210));
            clip[0].Add(new IntPoint(240, 130));
            clip[0].Add(new IntPoint(190, 130));

            Paths solution = new Paths();

            Clipper c = new Clipper();
            c.AddPaths(subj, PolyType.ptSubject, true);
            c.AddPaths(clip, PolyType.ptClip, true);
            c.Execute(ClipType.ctIntersection, solution,
              PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);


        }
    }
}
