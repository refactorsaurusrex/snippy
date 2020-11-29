using System;
using System.Management.Automation;

namespace Snippy.Cmdlets
{
    [Cmdlet(VerbsData.Publish, "Snippet")]
    public class PublishSnippetCmdlet : CmdletBase
    {
        // Publish to gist
        // allow snippet or any random file or files
        // store gist in snippet metadata

        // will be different if repo is public or not. if private, but publish to gist first.
        protected override void Run()
        {
            throw new NotImplementedException();
        }
    }
}