using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Api;
using Snippy.Infrastructure;
using Snippy.Models;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsData.Publish, "Snippet")]
    public class PublishSnippetCmdlet : CmdletBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "Snippet")]
        public SnippetIndexEntry Snippet { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Directory")]
        public string Directory { get; set; } = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = "File")]
        public string File { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public string Id { get; set; } = string.Empty;

        protected override void Run()
        {
            var token = GetGitHubToken();
            string url;
            switch (ParameterSetName)
            {
                default:
                    throw new NotSupportedException($"The parameter set {ParameterSetName} is not currently supported.");

                case "Snippet":
                    url = PublishSnippet(token);
                    break;

                case "Directory":
                    url = PublishDirectory(token);
                    break;

                case "File":
                    url = PublishFile(token);
                    break;
            }

            WriteObject(url);
        }

        private string PublishFile(string token)
        {
            var gh = new GitHub(token);
            var snippetFiles = new List<SnippetFile> { new SnippetFile(File) };
            return Id.IsNullOrWhiteSpace() ? 
                gh.CreateGist(Description, snippetFiles, isPublic: true).Result : 
                gh.UpdateGist(Id, Description, snippetFiles).Result;
        }

        private string PublishDirectory(string token)
        {
            var gh = new GitHub(token);
            var snippetFiles = new SnippetFiles(Directory);
            return Id.IsNullOrWhiteSpace() ? 
                gh.CreateGist(Description, snippetFiles, isPublic: true).Result : 
                gh.UpdateGist(Id, Description, snippetFiles).Result;
        }

        private string PublishSnippet(string token)
        {
            var gh = new GitHub(token);
            var directory = Path.Combine(Options.SnippetPath, Snippet.Directory);

            var snippetFiles = new SnippetFiles(directory);
            if (Uri.IsWellFormedUriString(snippetFiles.Meta.GistUrl, UriKind.Absolute))
            {
                var id = new Uri(snippetFiles.Meta.GistUrl).Segments.Last();
                return gh.UpdateGist(id, Description, snippetFiles).Result;
            }

            return gh.CreateGist(Description, snippetFiles, isPublic: true).Result;
        }
    }
}