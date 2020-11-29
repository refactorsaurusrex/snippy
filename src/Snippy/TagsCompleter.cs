using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace Snippy
{
    public class TagsCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            var organizer = new SnippetOrganizer(SnippyOptions.Instance.Value, FileAssociations.Instance.Value);
            var tags = organizer.GetUniqueTags();
            return string.IsNullOrEmpty(wordToComplete)
                ? tags.Select(x => new CompletionResult(x))
                : tags.Where(x => x.StartsWith(wordToComplete)).Select(x => new CompletionResult(x));
        }
    }
}