using System.Reflection;
using MagmaWorks.Taxonomy.Profiles;
using Newtonsoft.Json.Linq;

namespace Scaffold.Core.Extensions
{
    public static class ProfileDescriptionExtension
    {

        // 400 500 50 mm
        // 40 cm 500 50 mm
        // 0.4m 0.5m 50 mm
        // 0.4 500 mm 0.05 m
        public static T ProfileFromDescription<T>(this T type, string description) where T : IProfile
        {
            List<Length> lengths = GetLengths(description);

            Type t = typeof(T);
            ConstructorInfo[] constructors = t.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            var args = new List<object>();
            args.AddRange(lengths.Select(l => (object)l).ToList());
            args.Add(string.Empty);
            args.Add(string.Empty);
            object instance = constructors.FirstOrDefault().Invoke(args.ToArray());
            return (T)instance;
        }

        private static List<string> SplitBySpace(string str)
        {
            var strings = new List<string>();
            strings.AddRange(str.Split(' '));
            return strings.Select(s => s.Trim()).ToList();
        }

        private static List<Length> GetLengths(string str)
        {
            LengthUnit unit = GetUnit(str);
            List<string> splitStrings = SplitBySpace(str);
            var lengths = new List<Length>();
            for (int i = 0; i < splitStrings.Count - 1; i++)
            {
                string unitString = RemoveDigits(splitStrings[i + 1]);
                if (string.IsNullOrEmpty(unitString))
                {
                    lengths.Add(CastLengthFromString(splitStrings[i], unit));
                    continue;
                }

                lengths.Add(CastLengthFromString($"{splitStrings[i]} {unitString}", unit));
                i++;
            }

            return lengths;
        }

        private static LengthUnit GetUnit(string str)
        {
            return Length.ParseUnit(SplitBySpace(RemoveDigits(str)).LastOrDefault());
        }

        private static string RemoveDigits(string str)
        {
            return new String(str.Where(c => c < '0' || c > '9' ||
                c == Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                ).ToArray());
        }

        private static Length CastLengthFromString(string str, LengthUnit fallbackUnit)
        {
            if (Length.TryParse(str, out Length length))
            {
                return length;
            }

            double value = double.Parse(str, CultureInfo.InvariantCulture);
            return new Length(value, fallbackUnit);
        }
    }
}
