using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcFilePath : CalcValueBase
    {
        private string filePath;
        public override string ValueAsString { get { return filePath; } set { filePath = value; } }
        public override CalcValueType Type {get {return CalcValueType.FILEPATH;} }
        public CalcFilePath(string name, string path)
        {
            this.filePath = path;
            this.Name = name;
        }

    }
}
