using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using Snippy.Infrastructure;

namespace Snippy.Services
{
    public class LanguageCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            var organizer = new SnippetOrganizer(SnippyOptions.Instance.Value, FileAssociations.Instance.Value);
            var languages = organizer.GetUniqueLanguages();
            return string.IsNullOrEmpty(wordToComplete)
                ? languages.Select(x => new CompletionResult(x))
                : languages.Where(x => x.StartsWith(wordToComplete)).Select(x => new CompletionResult(x));
        }
    }
}