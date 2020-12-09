using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Snippy.Api;
using Snippy.Models;
using Xunit;

namespace SnippyTests
{
    public class CreateGistRequestTests
    {
        [Fact]
        public void ToJObject_CreatesValidJsonObject()
        {
            var request = new CreateGistRequest
            {
                Description = "Here is a gist I created.",
                Public = true,
                Files = new List<GistFile>
                {
                    new GistFile
                    {
                        FileName = "blah.cs",
                        Content = "Console.WriteLine(\"This is a test!\");"
                    },
                    new GistFile
                    {
                        FileName = "readme.md",
                        Content = "# This is a header"
                    }
                }
            };

            const string expected = @"{
                                        ""public"": true,
                                        ""description"": ""Here is a gist I created."",
                                        ""files"": {
                                            ""0"" : {
                                                ""filename"": ""blah.cs"",
                                                ""content"": ""Console.WriteLine(\""This is a test!\"");""
                                            },
                                            ""1"": {
                                                ""filename"": ""readme.md"",
                                                ""content"": ""# This is a header""
                                            }
                                        }
                                    }";

            var jo = request.ToJObject();
            var e = JObject.Parse(expected);
            jo.Should().BeEquivalentTo(e);
        }
    }
}
