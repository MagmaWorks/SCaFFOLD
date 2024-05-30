namespace Scaffold.VisualStudio.Calculator;

public class FileChangeDetail
{
    public string FullPath { get; set; }
    public DateTime LastModified { get; set; }
    public bool HasChanged { get; set; }
}