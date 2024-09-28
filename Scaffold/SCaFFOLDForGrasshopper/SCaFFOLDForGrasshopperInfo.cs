using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace SCaFFOLDForGrasshopper
{
    public class SCaFFOLDForGrasshopperInfo : GH_AssemblyInfo
    {
        public override string Name => "SCaFFOLDForGrasshopper";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("38089dc0-7e1b-43d5-8d2a-932c29bd3cc4");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";

        //Return a string representing the version.  This returns the same version as the assembly.
        public override string AssemblyVersion => GetType().Assembly.GetName().Version.ToString();
    }
}