using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace TaxonomyObjectCodeGenerator
{
    public class Program
    {
        private static readonly List<string> _assemblies = new() {
            "MagmaWorks.Taxonomy.Profiles",
            "MagmaWorks.Taxonomy.Materials"
        };

        private static readonly List<string> _globalUsings = new() {
            "UnitsNet",
            "UnitsNet.Units",
        };

        private static readonly Dictionary<string, string> _renamings = new() {
            { "Angle", "MagmaWorks.Taxonomy.Profiles.Angle" }
        };

        private const string _namespace = "Scaffold.Core.CalcObjects";

        public static void Main()
        {
            foreach (string assemblyName in _assemblies)
            {
                List<Type> classes = GetAllPublicClassesFrom(assemblyName);
                foreach (Type type in classes)
                {
                    WriteClassToFile(type, assemblyName);
                }
            }
        }

        private static List<Type> GetAllPublicClassesFrom(string assemblyName)
        {
            var data = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type[] typelist = Assembly.Load(assemblyName).GetTypes()
                    .Where(p => p.IsPublic && !(p.IsAbstract && p.IsSealed)).ToArray();
            foreach (Type t in typelist)
            {
                if (t.Namespace == null || t.IsInterface)
                {
                    continue;
                }

                if (t.GetCustomAttribute(typeof(CompilerGeneratedAttribute), true) == null)
                {
                    data.Add(t);
                }
            }

            return data;
        }

        private static string GetPath(string assemblyName, string assembly)
        {
            DirectoryInfo? directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
            return Path.Combine(directory.FullName, "Scaffold.Core", "CalcObjects", assembly);
        }

        private static void WriteClassToFile(Type type, string assemblyName)
        {
            string assembly = assemblyName.Split('.').LastOrDefault();
            string filePath = GetPath(assemblyName, assembly);
            string name = type.Name;

            var sb = new StringBuilder();
            sb.AppendLine($@"using {type.Namespace};
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;");
            if (_renamings.ContainsKey(name))
            {
                sb.AppendLine($@"using {name} = {_renamings[name]};");
            }

            sb.AppendLine($@"
namespace {_namespace}.{assembly};
public sealed class Calc{name} : CalcTaxonomyObject<{name}>
#if NET7_0_OR_GREATER
    , IParsable<Calc{name}>
#endif
{{
    public Calc{name}({name} {name.ToLower()}, string name, string symbol = """")
        : base({name.ToLower()}, name, symbol) {{ }}");

            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            foreach (ConstructorInfo constructor in constructors)
            {
                ParameterInfo[] parameters = constructor.GetParameters();
                string inputs = Inputs(parameters, type.Namespace);
                sb.AppendLine($@"
    public Calc{name}({inputs}, string name, string symbol = """")
        : base(new {name}({string.Join(", ", parameters.Select(s => s.Name).ToList())}), name, symbol) {{ }}");
            }
            sb.Append($@"
    public static bool TryParse(string s, IFormatProvider provider, out Calc{name} result)
    {{
        try
        {{
            result = s.FromJson<Calc{name}>();
            return true;
        }}
        catch
        {{
            result = null;
            return false;
        }}
    }}

    public static Calc{name} Parse(string s, IFormatProvider provider)
    {{
        return s.FromJson<Calc{name}>();
    }}
}}
");
            Directory.CreateDirectory(filePath);
            var file = new StreamWriter($"{filePath}/Calc{name}.cs");
            file.Write(sb);
            file.Close();
        }

        private static string Inputs(ParameterInfo[] parameters, string assemblyName)
        {
            var sb = new StringBuilder();
            foreach (ParameterInfo parameter in parameters)
            {
                string type = parameter.ParameterType.FullName;
                foreach (string @using in _globalUsings)
                {
                    type = type.Replace(@using, string.Empty);
                }

                type = type.Replace(assemblyName, string.Empty);
                sb.Append($"{type.Trim('.')} {parameter.Name}, ");
            }

            return sb.ToString().TrimEnd(' ').TrimEnd(',');
        }
    }
}
