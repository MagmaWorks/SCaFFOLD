using Scaffold.Core.Abstract;
using Scaffold.Core.Interfaces;

namespace Scaffold.Console
{
    internal class Program
    {
        private static void TestZipReader()
        {
            var zipStream = File.OpenRead(@"C:\Users\d.growns\Documents\Repos\Web\Scaffold.App\Scaffold.App\LocalDependencies\8d7d3c91-d326-4546-8ca1-014de467d444-Scaffold-XUnitTests-dll-1-0-0-1.zip");
        
            var reader = new AssemblyFromZipReader("Scaffold.XUnitTests.dll");

            var assembly = reader.Get(zipStream);
            zipStream.Dispose();

            var instance = (CalculationBase) assembly.Assembly.CreateInstance("Scaffold.XUnitTests.Core.AdditionCalculation");
            instance?.LoadIoCollections();

            ReadInstance(instance);
        }

        private static void TestBinariesReader()
        {
            var reader = new BinariesAssemblyReader(@"C:\Users\d.growns\Documents\Repos\ScaffoldForVsTesting\VsTesting");
            var assembly = reader.GetAssembly();
  
            var instance = (CalculationBase)assembly.CreateInstance("VsTesting.Core.AdditionCalculation");
            instance?.LoadIoCollections();
            
            ReadInstance(instance);
        }

        private static void ReadInstance(CalculationBase instance)
        {
            if (instance == null)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("FAILED: Could not read instance.");
                System.Console.ResetColor();
                return;
            }
            
            WriteCalcValueList(instance.GetInputs(), "INPUTS");
            WriteCalcValueList(instance.GetOutputs(), "OUTPUTS");
        }

        private static void WriteCalcValueList(IEnumerable<ICalcValue> values, string name)
        {
            System.Console.WriteLine($"----- {name} -----");
            foreach (var value in values)
            {
                System.Console.WriteLine($"{value.DisplayName}: {value.GetValue()}");
            }
            
            System.Console.WriteLine("-----");
        }
        
        static void Main(string[] args)
        {
            TestZipReader();
            TestBinariesReader();
            
            System.Console.ReadLine();
        }
    }
}
