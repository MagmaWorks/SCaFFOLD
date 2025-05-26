﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace TaxonomyObjectCodeGenerator
{
    public class Program
    {
        private static readonly List<string> _assemblies = new() {
            "MagmaWorks.Taxonomy.Profiles",
            "MagmaWorks.Taxonomy.Materials",
            "MagmaWorks.Taxonomy.Sections",
            "MagmaWorks.Taxonomy.Loads",
            "MagmaWorks.Taxonomy.Cases",
        };

        private static readonly List<string> _globalUsings = new() {
            "System",
            "System.Collections.Generic",
            "System.Globalization",
            "System.Linq",
            "UnitsNet",
            "UnitsNet.Units",
        };

        private static readonly Dictionary<string, string> _renamings = new() {
            { "Angle", "MagmaWorks.Taxonomy.Profiles.Angle" },
        };

        private const string _namespace = "Scaffold.Core.CalcObjects";

        public static void Main()
        {
            foreach (string assemblyName in _assemblies)
            {
                List<Type> classes = GetAllPublicClassesFrom(assemblyName);
                foreach (Type type in classes)
                {
                    if (type.BaseType.Name == "Exception")
                    {
                        continue;
                    }

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
            var paths = new List<string>()
            {
                directory.FullName, "Scaffold.Core", "CalcObjects", "GeneratedCode"
            };
            paths.AddRange(assembly.Split('.'));
            return Path.Combine(paths.ToArray());
        }

        private static void WriteClassToFile(Type type, string assemblyName)
        {
            string assemblyBase = assemblyName.Replace(assemblyName.Split('.').LastOrDefault(), string.Empty);
            string assembly = type.Namespace.Replace(assemblyBase, string.Empty);
            string filePath = GetPath(assemblyName, assembly);
            string name = type.Name;

            var usings = new HashSet<string>();
            var sb = new StringBuilder();
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
                string inputs = Inputs(parameters, type.Namespace, ref usings);
                sb.AppendLine($@"
    public Calc{name}({inputs}string name, string symbol = """")
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
            sb.Insert(0, Usings(type, usings));
            Directory.CreateDirectory(filePath);
            var file = new StreamWriter($"{filePath}/Calc{name}.cs");
            file.Write(sb);
            file.Close();
        }

        private static string Inputs(ParameterInfo[] parameters, string assemblyName, ref HashSet<string> usings)
        {
            var sb = new StringBuilder();
            foreach (ParameterInfo parameter in parameters)
            {
                string type = parameter.ParameterType.FullName;
                string typeNamespace = parameter.ParameterType.Namespace;
                type = type.Replace("Int32", "int");
                type = type.Replace("Double", "double");
                type = type.Replace("String", "string");
                type = type.Replace("Boolean", "bool");

                if (!_globalUsings.Contains(typeNamespace))
                {
                    usings.Add(typeNamespace);
                }

                foreach (string @using in _globalUsings)
                {
                    type = type.Replace(@using, string.Empty);
                }

                type = type.Replace(assemblyName, string.Empty);
                if (type.StartsWith(".Collections.Generic"))
                {
                    type = type.Replace(".Collections.Generic.", string.Empty);
                    type = type.Replace("`1[[", string.Empty);
                    string listType = type.Split("List")[0];
                    type = type.Split("List")[1].Split(',')[0].TrimEnd(' ');
                    type = type.Split('.').LastOrDefault();
                    type = $"{listType}List<{type.Trim('.')}>";
                } else
                {
                    type = type.Split('.').LastOrDefault();
                }

                sb.Append($"{type} {parameter.Name}, ");
            }

            string par = sb.ToString().TrimEnd(' ').Trim(',').Trim(' ');
            if (string.IsNullOrEmpty(par))
            {
                return string.Empty;
            }

            return $"{par}, ";
        }

        private static string Usings(Type type, HashSet<string> usings)
        {
            var s = new List<string>()
            {
                "using MagmaWorks.Taxonomy.Serialization;",
                "using Scaffold.Core.Abstract;",
                $"using {type.Namespace};",
            };

            foreach (string use in usings)
            {
                if (!s.Contains($@"using {use};"))
                {
                    s.Add($@"using {use};");
                }
            }
            
            s.Sort();
            if (_renamings.ContainsKey(type.Name))
            {
                s.Add($@"using {type.Name} = {_renamings[type.Name]};");
            }

            var sb = new StringBuilder();
            foreach (string str in s)
            {
                sb.AppendLine(str);
            }

            return sb.ToString();
        }
    }
}
