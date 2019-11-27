using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calcs
{
    public class PluginInfo
    {
        public Type PluginType { get; private set; }
        public string Name { get; private set; }

        public PluginInfo(Type type, string name)
        {
            PluginType = type;
            Name = name;
        }
    }
}
