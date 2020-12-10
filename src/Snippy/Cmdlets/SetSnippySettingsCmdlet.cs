using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Infrastructure;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Set, "SnippySettings")]
    public class SetSnippySettingsCmdlet : PSCmdlet
    {
        [Parameter]
        public string SnippetPath { get; set; }

        [Parameter]
        public string WorkspacePath { get; set; }

        [Parameter]
        public string GitHubTokenSecretName { get; set; }

        [Parameter]
        public string SecretVault { get; set; }

        [Parameter]
        public SwitchParameter AutoSync { get; set; }

        protected override void ProcessRecord()
        {
            var options = SnippyOptions.Instance.Value;

            if (!SnippetPath.IsNullOrWhiteSpace())
                options.SnippetPath = GetUnresolvedProviderPathFromPSPath(SnippetPath);

            if (!WorkspacePath.IsNullOrWhiteSpace())
                options.WorkspacePath = GetUnresolvedProviderPathFromPSPath(WorkspacePath);

            if (!SecretVault.IsNullOrWhiteSpace())
                options.SecretVault = SecretVault;

            if (!GitHubTokenSecretName.IsNullOrWhiteSpace())
                options.GitHubTokenSecretName = GitHubTokenSecretName;

            options.AutoSync = AutoSync;
            options.Save();
        }
    }
}