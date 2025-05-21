using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using Scaffold.Calculations.Eurocode.Concrete;

namespace Scaffold.Calculations.Tests
{
    /// <summary>
    /// Automated test that will check any calculation inheriting from ICalculation
    /// with a constructor initialises its public properties to non-nullable values.
    /// </summary>
    public class AutomatedTestsOfICalcalculation
    {

        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void ConstructorTest(Type type)
        {
            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
            object instance = constructor.Invoke(null);
            Assert.NotNull(instance);
            TestObjectsPropertiesAreNotNull(instance);
        }

        public class TestDataGenerator : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = GetAllClassesOf<ICalculation>();

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private static List<object[]> GetAllClassesOf<T>()
            {
                var data = new List<object[]>();
                Type[] typelist = Assembly.GetAssembly(typeof(ConcreteProperties)).GetTypes()
                    .Where(p => typeof(T).IsAssignableFrom(p)).ToArray();
                foreach (Type type in typelist)
                {
                    if (type.Namespace == null)
                    {
                        continue;
                    }

                    if (type.GetCustomAttribute(typeof(CompilerGeneratedAttribute), true) == null)
                    {
                        data.Add([type]);
                    }
                }

                return data;
            }
        }

        private void TestObjectsPropertiesAreNotNull(object obj)
        {
            PropertyInfo[] propertyInfo = obj.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (PropertyInfo property in propertyInfo)
            {
                Assert.NotNull(property.GetValue(obj));
            }
        }
    }
}
