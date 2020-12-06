using System;
using FluentAssertions;
using Snippy.Models;
using Xunit;

namespace SnippyTests
{
    public class WorkspaceTests
    {
        [Fact]
        public void Add_IgnoresFolder_WhenDuplicateIsAdded()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var path = @$"C:\repos\my-snippets\snippets\{guid}";
            var workspace = new Workspace();
            var folder1 = new Folder("Folder 1", path);
            workspace.Add(folder1);
            var folder2 = new Folder("Folder 2", path);
            workspace.Add(folder2);
            workspace.Folders.Count.Should().Be(1);
        }

        [Fact]
        public void Add_AddsFolder_WhenFolderIsNotDuplicate()
        {
            var workspace = new Workspace();
            var guid1 = Guid.NewGuid().ToString().Replace("-", "");
            var path1 = @$"C:\repos\my-snippets\snippets\{guid1}";
            var folder1 = new Folder("Folder 1", path1);

            var guid2 = Guid.NewGuid().ToString().Replace("-", "");
            var path2 = @$"C:\repos\my-snippets\snippets\{guid2}";
            var folder2 = new Folder("Folder 2", path2);

            workspace.Add(folder1);
            workspace.Add(folder2);
            workspace.Folders.Count.Should().Be(2);
        }
    }
}
