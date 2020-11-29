namespace Snippy
{
    internal interface IFileAssociations
    {
        string Lookup(string fileNameOrPath);
        string FileAssociationsPath { get; }
    }
}
