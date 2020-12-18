using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Api;
using Snippy.Infrastructure;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Copy, "Gist")]
    public class CopyGistCmdlet : CmdletBase
    {
        [Parameter(Mandatory = true)]
        public string Id { get; set; }

        [Parameter(Mandatory = true)]
        public string Out { get; set; }

        protected override void Run()
        {
            var token = GetGitHubToken();
            var gh = new GitHub(token);
            var gist = gh.GetGist(Id).Result;
            gist.SaveFiles(Out);
        }
    }
}