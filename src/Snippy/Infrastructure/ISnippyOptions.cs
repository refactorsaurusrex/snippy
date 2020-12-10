namespace Snippy.Infrastructure
{
    public interface ISnippyOptions
    {
        string WorkspacePath { get; set; }
        string SnippetPath { get; set; }
        string SecretVault { get; set; }
        string GitHubTokenSecretName { get; set; }
        bool AutoSync { get; set; }
        void Save();
        bool IsValid();
    }
}