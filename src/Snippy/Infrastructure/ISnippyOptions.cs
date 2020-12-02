namespace Snippy.Infrastructure
{
    internal interface ISnippyOptions
    {
        string WorkspacePath { get; set; }
        string SnippetPath { get; set; }
        void Save();
        bool IsValid();
    }
}