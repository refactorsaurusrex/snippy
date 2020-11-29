using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace Snippy
{
    public class WorkspaceNameCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            var organizer = new SnippetOrganizer(SnippyOptions.Instance.Value, FileAssociations.Instance.Value);
            var files = organizer.GetAllWorkspaceFiles();
            return string.IsNullOrEmpty(wordToComplete)
                ? files.Select(x => new CompletionResult(x))
                : files.Where(x => x.StartsWith(wordToComplete)).Select(x => new CompletionResult(x));
        }
    }
}