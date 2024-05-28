using FluentAssertions;
using Scaffold.Core.Abstract;
using Scaffold.Shared;

namespace Scaffold.XUnitTests.UnitTests;

public class AssemblyReaderTests
{
    [Fact]
    public void FromZip_Ok()
    {
        var zipStream = File.OpenRead(
            @"C:\Users\d.growns\Documents\Repos\Web\Scaffold.App\Scaffold.App\LocalDependencies\8d7d3c91-d326-4546-8ca1-014de467d444-Scaffold-XUnitTests-dll-1-0-0-1.zip");
        
        var reader = new AssemblyFromZipReader("Scaffold.XUnitTests.dll");

        var assembly = reader.Get(zipStream);
        zipStream.Dispose();

        var instance = (CalculationBase)assembly.Assembly.CreateInstance("Scaffold.XUnitTests.Core.AdditionCalculation");
        instance?.LoadIoCollections();

        assembly.CalculationQualifiedNames.Count.Should().Be(5);
        instance.Should().NotBeNull();
        instance?.GetInputs().Count.Should().Be(2);
    }

    [Fact]
    public void FromBinaries_Ok()
    {
        var reader = new BinariesAssemblyReader(@"C:\Users\d.growns\Documents\Repos\ScaffoldForVsTesting\VsTesting");
        var assembly = reader.GetAssembly();
  

        var instance = (CalculationBase)assembly.CreateInstance("VsTesting.Core.AdditionCalculation");
        instance.Should().NotBeNull();
        instance?.LoadIoCollections();

        instance.Should().NotBeNull();
        instance?.GetInputs().Count.Should().Be(2);
    }
}