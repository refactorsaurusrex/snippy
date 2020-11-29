﻿using System.Management.Automation;
using JetBrains.Annotations;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "SnippySettings")]
    public class GetSnippySettingsCmdlet : CmdletBase
    {
        protected override void Run()
        {
            var options = SnippyOptions.Instance.Value;
            WriteObject(new
            {
                options.SnippetPath,
                options.WorkspacePath
            });
        }
    }
}