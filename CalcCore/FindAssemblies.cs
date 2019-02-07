using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public static class FindAssemblies
    {
        public static List<CalcAssembly> GetAssemblies()
        {
            var calcs = new List<CalcAssembly>();

            var ext = new List<string> { ".dll" };
            var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Libraries";
            var myFiles = Directory.GetFiles(filePath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => ext.Contains(System.IO.Path.GetExtension(s)));
            var myAssemblies = myFiles.ToList();

            foreach (var assembly in myAssemblies)
            {
                var ass = System.IO.Path.GetFileName(assembly);
                Assembly myAssembly = Assembly.LoadFile(assembly);
                var res = from type in myAssembly.GetTypes()
                          where typeof(CalcCore.ICalc).IsAssignableFrom(type)
                          select type;
                foreach (var item in res.ToList())
                {
                    calcs.Add(new CalcAssembly(item, ass));
                }
            }
            return calcs;
        }
    }
}
