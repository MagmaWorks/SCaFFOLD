using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace CalcMonkey
{
    public class CalcMonkeyInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "CalcMonkey";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("b47f2148-9602-47cf-966d-6523d312e771");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
