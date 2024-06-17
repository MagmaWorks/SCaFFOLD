using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using FluentAssertions;
using Scaffold.Core.Enums;
using Scaffold.XUnitTests.Core;

namespace Scaffold.XUnitTests.UnitTests;

/// <summary>
/// The host application normally calls LoadIoCollections. When it is not called, the default values are expected.
/// </summary>
public class AdditionCalculationTests
{
    private const string typeName = "Add values";
    private const string title = "Core library tester";

    [Fact]
    public void Xml_Test()
    {
        var file = @"C:\Users\d.growns\Documents\Repos\ScaffoldForVsTesting\VsTesting\VsTesting.csproj";
        
        var projectFile = File.ReadAllText(file);
        var projectFileInfo = new FileInfo(file);
        
        var hintPathRegex = new Regex("<HintPath>.*</HintPath>");
        var hintPaths = hintPathRegex.Matches(projectFile);

        foreach (Match hintPath in hintPaths)
        {
            var path = hintPath.Value.Replace("<HintPath>", "").Replace("</HintPath>", "");
            var hintMatches = Regex.Matches(path, Regex.Escape("..\\")).Count;
            var hintStr = new StringBuilder();
            
            DirectoryInfo parentDirectory = null;
            
            for (var i = 0; i < hintMatches; i++)
            {
                hintStr.Append(@"..\");
                parentDirectory = projectFileInfo.Directory?.Parent;
            }

            parentDirectory = parentDirectory?.Parent; // 1 additional step required for the path but not for the hint str.

            if (parentDirectory == null)
                throw new ArgumentException("Failed to convert hint path to a realised path.");
            
            var newPath = path.Replace(hintStr.ToString(), $@"{parentDirectory.FullName}\");
            projectFile = projectFile.Replace(path, newPath);
        }

        File.WriteAllText(file, projectFile);
    }
    
    [Fact]
    public void DefaultSetup_Unassigned_Expected()
    {
        var calc = new AdditionCalculation();
        calc.Type.Should().Be(typeName);
        calc.Title.Should().Be(title);
        
        calc.LeftAssignment.Should().BeNull();
        calc.RightAssignment.Should().BeNull();
        calc.Result.Should().BeNull();
    }
    
    [Fact]
    public void DefaultSetup_Ok()
    {
        var calc = new AdditionCalculation();
        calc.Type.Should().Be(typeName);
        calc.Title.Should().Be(title);
        
        calc.LoadIoCollections();
        calc.GetInputs().ToList().Count.Should().Be(2);
        calc.GetOutputs().ToList().Count.Should().Be(1);
        
        calc.LeftAssignment.Value.Should().Be(2);
        calc.RightAssignment.Value.Should().Be(3);
        calc.Result.Value.Should().Be(5);
        calc.Status.Should().Be(CalcStatus.None);
        
        // Hits return statement in LoadIoCollections
        calc.LoadIoCollections();
    }
    
    [Fact]
    public void Updated_FromUI_Ok()
    {
        var calc = new AdditionCalculation();
        calc.Type.Should().Be(typeName);
        calc.Title.Should().Be(title);
        
        calc.LoadIoCollections();

        calc.LeftAssignment.Value = 5;
        calc.RightAssignment.Value = 4;
        calc.Result.Value.Should().Be(5, because: "result has not changed yet through the update method.");

        calc.Update();
        calc.Result.Value.Should().Be(9);

        var formulae = calc.GetFormulae().ToList();
        formulae.Count.Should().Be(3);
        formulae[0].Expression.Count.Should().Be(1);
        formulae[0].Expression[0].Should().Be("x &=& a + b");
    }
}