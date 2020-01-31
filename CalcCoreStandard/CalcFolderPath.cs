using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcFolderPath : CalcValueBase
    {
        private string folderPath = "DEFAULT";
        public override string ValueAsString { get { return folderPath; } set { folderPath = value; } }
        public override CalcValueType Type { get { return CalcValueType.FOLDERPATH; } }
        public CalcFolderPath(string name, string path)
        {
            this.folderPath = path;
            this.Name = name;
        }
    }
}

