namespace Snippy.Infrastructure
{
    public interface ISnippyOptions
    {
        string WorkspacePath { get; set; }
        string SnippetPath { get; set; }
        void Save();
        bool IsValid();
    }
}