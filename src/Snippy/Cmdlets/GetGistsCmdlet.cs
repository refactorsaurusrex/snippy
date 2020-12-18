using System.Linq;
using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Api;
using Snippy.Infrastructure;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "Gists")]
    public class GetGistsCmdlet : CmdletBase
    {
        [Parameter]
        public SwitchParameter NoCache { get; set; }

        public int MaxDescriptionLength { get; set; } = 100;

        protected override void Run()
        {
            var token = GetGitHubToken();
            var gh = new GitHub(token);
            var gists = gh.ListGists(NoCache).Result;

            WriteObject(gists.Select(g => g.ToMetaData(MaxDescriptionLength)), true);
        }
    }
}