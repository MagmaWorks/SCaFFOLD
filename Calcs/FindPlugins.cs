using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Calcs
{
    public class FindPlugins
    {
        public static List<PluginInfo> GetPlugins()
        {
            var plugins = new List<PluginInfo>();

            var ext = new List<string> { ".dll" };
            var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Libraries";
            var myFiles = Directory.GetFiles(filePath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => ext.Contains(System.IO.Path.GetExtension(s)));
            var myPlugins = myFiles.ToList();

            foreach (var assembly in myPlugins)
            {
                var ass = System.IO.Path.GetFileName(assembly);
                Assembly myAssembly = Assembly.LoadFile(assembly);
                var res = from type in myAssembly.GetTypes()
                          where typeof(Calcs.CalcPluginBase).IsAssignableFrom(type)
                          select type;
                foreach (var item in res.ToList())
                {
                    string name = ((PluginNameAttribute)Attribute.GetCustomAttribute(item, typeof(PluginNameAttribute))).Name;
                    plugins.Add(new PluginInfo(item, name));
                }
            }
            return plugins;
        }
    }
}
