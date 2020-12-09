using System;
using System.Linq;
using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Api;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "Gists")]
    public class GetGistsCmdlet : CmdletBase
    {

        [Parameter]
        public SwitchParameter NoCache { get; set; }

        protected override void Run()
        {
            var token = GetGitHubToken();
            var gh = new GitHub(token);
            var gists = gh.ListGists(NoCache).Result;

            WriteObject(gists.Select(g => new
            {
                g.Id,
                Description = g.Description.Substring(0, Math.Min(g.Description.Length, 100)),
                g.Created,
                g.Updated
            }), true);
        }
    }
}