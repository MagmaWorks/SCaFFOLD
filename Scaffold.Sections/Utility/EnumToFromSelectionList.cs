using System;
using System.Collections.Generic;
using System.Linq;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using Scaffold.Core.CalcValues;

namespace Scaffold.Core.Utility
{
    internal static class EnumToFromSelectionList
    {
        internal static readonly List<string> BarDiameters = Enum.GetValues(typeof(BarDiameter)).Cast<BarDiameter>()
        .Select(v => v.ToString().Replace("D", string.Empty)).ToList();
        internal static readonly List<string> ConcreteGrades = Enum.GetValues(typeof(EnConcreteGrade)).Cast<EnConcreteGrade>()
            .Select(v => v.ToString().Replace("_", "/")).ToList();
        internal static readonly List<string> RebarGrades = Enum.GetValues(typeof(EnRebarGrade)).Cast<EnRebarGrade>()
            .Select(v => v.ToString()).ToList();

        internal static BarDiameter GetBarDiameter(CalcSelectionList list)
        {
            return GetBarDiameter(list.Value);
        }

        internal static BarDiameter GetBarDiameter(string name)
        {
            Enum.TryParse("D" + name, out BarDiameter value);
            return value;
        }

        internal static EnConcreteGrade GetConcreteGrade(CalcSelectionList list)
        {
            return GetConcreteGrade(list.Value);
        }

        internal static EnConcreteGrade GetConcreteGrade(string name)
        {
            Enum.TryParse(name.Replace("/", "_"), out EnConcreteGrade value);
            return value;
        }

        internal static EnRebarGrade GetRebarGrade(CalcSelectionList list)
        {
            return GetRebarGrade(list.Value);
        }

        internal static EnRebarGrade GetRebarGrade(string name)
        {
            Enum.TryParse(name, out EnRebarGrade value);
            return value;
        }
    }
}
