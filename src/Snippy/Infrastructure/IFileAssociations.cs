namespace Snippy.Infrastructure
{
    internal interface IFileAssociations
    {
        string Lookup(string fileNameOrPath);
        string FileAssociationsPath { get; }
    }
}
