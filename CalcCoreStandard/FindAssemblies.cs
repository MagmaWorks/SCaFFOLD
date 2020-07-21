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

            filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Magma Works\SCaFFOLD\Libraries";
            if (Directory.Exists(filePath))
            {
                myFiles = Directory.GetFiles(filePath, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(s => ext.Contains(System.IO.Path.GetExtension(s)));
                myAssemblies.AddRange(myFiles);
            }



            foreach (var assembly in myAssemblies)
            {
                var ass = System.IO.Path.GetFileName(assembly);
                Assembly myAssembly = Assembly.LoadFile(assembly);
                var res = from type in myAssembly.GetTypes()
                          where typeof(CalcCore.ICalc).IsAssignableFrom(type)
                          select type;
                foreach (var item in res.ToList())
                {
                    if (Attribute.IsDefined(item, typeof(CalcCore.CalcNameAttribute)) 
                        && Attribute.IsDefined(item, typeof(CalcCore.CalcAlternativeNameAttribute)))
                    {
                        string name = ((CalcCore.CalcNameAttribute)Attribute.GetCustomAttribute(item, typeof(CalcCore.CalcNameAttribute))).CalcName;
                        List<string> altNames = (Attribute.GetCustomAttributes(item, typeof(CalcCore.CalcAlternativeNameAttribute))).Select(a => ((CalcAlternativeNameAttribute)a).CalcAlternativeName).ToList();
                        calcs.Add(new CalcAssembly(item, ass, name, altNames));
                    }
                    else if (Attribute.IsDefined(item, typeof(CalcCore.CalcNameAttribute)))
                    {
                        string name = ((CalcCore.CalcNameAttribute)Attribute.GetCustomAttribute(item, typeof(CalcCore.CalcNameAttribute))).CalcName;
                        calcs.Add(new CalcAssembly(item, ass, name, null));
                    }

                }
            }
            return calcs;
        }
    }
}
