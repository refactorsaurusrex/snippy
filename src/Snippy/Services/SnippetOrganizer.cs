﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Snippy.Infrastructure;
using Snippy.Models;

namespace Snippy.Services
{
    internal class SnippetOrganizer
    {
        private readonly ISnippyOptions _options;
        private readonly IFileAssociations _fileAssociations;
        private List<Snippet> _snippets;

        public SnippetOrganizer(ISnippyOptions options, IFileAssociations fileAssociations)
        {
            _options = options;
            _fileAssociations = fileAssociations;
            Load();
        }

        public ICollection<SnippetIndexEntry> CreateIndex(OrderBy orderBy, SortDirection sortDirection)
        {
            var index = _snippets.Select(x => new SnippetIndexEntry
            {
                Created = x.Meta.CreatedUtc.FromUtcToLocal(),
                Description = x.Meta.Description,
                Directory = new DirectoryInfo(x.DirectoryPath).Name,
                Files = x.Files.Select(Path.GetFileName).ToList(),
                Tags = x.Meta.Tags,
                Title = x.Meta.Title
            });

            var ascending = sortDirection == SortDirection.Ascending;
            var ordered = orderBy switch
            {
                OrderBy.Alphabetical => ascending ? index.OrderBy(x => x.Title) : index.OrderByDescending(x => x.Title),
                OrderBy.Created => ascending ? index.OrderBy(x => x.Created) : index.OrderByDescending(x => x.Created),
                _ => throw new NotSupportedException($"Unable to sort by {nameof(orderBy)}")
            };

            return ordered.ToList();
        }

        public IEnumerable<string> GetUniqueTags() => _snippets.SelectMany(x => x.Meta.Tags).Distinct().OrderBy(x => x);

        public IEnumerable<string> GetUniqueLanguages() => _snippets.SelectMany(x => x.Files).Select(x => _fileAssociations.Lookup(x)).Distinct().OrderBy(x => x);

        public IEnumerable<string> GetAllWorkspaceFiles() =>
            new DirectoryInfo(_options.WorkspacePath)
                .GetFiles()
                .Select(x => x.Name)
                .Where(x => x.EndsWith(Constants.WorkspaceFileExtension))
                .OrderBy(x => x);

        public void UpdateWorkspaces(IEnumerable<WorkspaceDefinition> definitions, OrderBy orderBy, SortDirection sortDirection, bool resetSettings, ICollection<string> workspaces)
        {
            var serializer = new Serializer();
            foreach (var definition in definitions.Where(x => workspaces.Contains(x.FileName)))
                UpdateWorkspace(definition, resetSettings, sortDirection, orderBy, serializer);
        }

        public void UpdateAllWorkspaces(IEnumerable<WorkspaceDefinition> definitions, OrderBy orderBy, SortDirection sortDirection, bool resetSettings)
        {
            var serializer = new Serializer();
            foreach (var definition in definitions)
                UpdateWorkspace(definition, resetSettings, sortDirection, orderBy, serializer);
        }

        public string CreateNewSnippet(string title, string description, ICollection<string> tags, ICollection<string> files)
        {
            var invalid = Path.GetInvalidFileNameChars();
            if (files.SelectMany(f => f).Any(c => invalid.Contains(c)))
                throw new InvalidOperationException("One or more files names included invalid characters.");

            var folder = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            var directory = Path.Combine(_options.SnippetPath, folder);
            Directory.CreateDirectory(directory);

            foreach (var file in files)
            {
                using var _ = File.CreateText(Path.Combine(directory, file));
            }

            var meta = new Meta
            {
                CreatedUtc = DateTime.UtcNow,
                Title = title,
                Description = description,
                Tags = tags?.ToList()
            };

            var metaPath = Path.Combine(directory, Meta.FileName);
            new Serializer().SerializeToYaml(meta, metaPath);

            _snippets.Add(new Snippet
            {
                DirectoryPath = directory,
                Files = files.ToList(),
                Meta = meta
            });

            return directory;
        }

        public ICollection<WorkspacePackage> CreateWorkspacesByLanguage(OrderBy orderBy, SortDirection sortDirection, bool hideMetaFiles)
        {
            var languageMap = new Dictionary<string, List<Snippet>>();

            foreach (var snippet in _snippets)
            {
                var files = snippet.Files;
                foreach (var file in files)
                {
                    var language = _fileAssociations.Lookup(file);
                    if (languageMap.ContainsKey(language))
                    {
                        languageMap[language].Add(snippet);
                    }
                    else
                    {
                        languageMap.Add(language, new List<Snippet> { snippet });
                    }
                }
            }

            var packages = new List<WorkspacePackage>();
            foreach (var (language, snippets) in languageMap)
            {
                var package = CreatePackage(orderBy, sortDirection, snippets, $"{language}-lang", hideMetaFiles, tags: new List<string>(), languages: new List<string> { language });
                packages.Add(package);
            }

            return packages;
        }

        public ICollection<WorkspacePackage> CreateWorkspacesByTag(OrderBy orderBy, SortDirection sortDirection, bool hideMetaFiles)
        {
            var tagMap = new Dictionary<string, List<Snippet>>();

            foreach (var snippet in _snippets)
            {
                var tags = snippet.Meta.Tags;
                foreach (var tag in tags)
                {
                    if (tagMap.ContainsKey(tag))
                    {
                        tagMap[tag].Add(snippet);
                    }
                    else
                    {
                        tagMap.Add(tag, new List<Snippet> { snippet });
                    }
                }
            }

            var packages = new List<WorkspacePackage>();
            foreach (var (tag, snippets) in tagMap)
            {
                var package = CreatePackage(orderBy, sortDirection, snippets, tag, hideMetaFiles, tags: new List<string> { tag }, languages: new List<string>());
                packages.Add(package);
            }

            return packages;
        }

        public WorkspacePackage CreateUnpartitionedWorkspace(OrderBy orderBy, SortDirection sortDirection, bool hideMetaFiles) => 
            CreatePackage(orderBy, sortDirection, _snippets, "all-snippets", hideMetaFiles, tags: new List<string>(), languages: new List<string>());

        public WorkspacePackage CreateCustomWorkspace(string name, ICollection<string> tags, ICollection<string> languages, OrderBy orderBy, SortDirection sortDirection, SwitchParameter hideMetaFiles)
        {
            bool IsMatch(Snippet s) => s.Meta.Tags.Any(tags.Contains) && s.Files.Select(f => _fileAssociations.Lookup(f)).Any(languages.Contains);
            var filteredSnippets = _snippets.Where(IsMatch).ToList();
            return CreatePackage(orderBy, sortDirection, filteredSnippets, name, hideMetaFiles, tags, languages);
        }

        private WorkspacePackage CreatePackage(OrderBy orderBy, SortDirection sortDirection, IEnumerable<Snippet> snippets, string name, bool hideMetaFiles, ICollection<string> tags, ICollection<string> languages)
        {
            var ascending = sortDirection == SortDirection.Ascending;
            var orderedSnippets = orderBy switch
            {
                OrderBy.Alphabetical => ascending ? snippets.OrderBy(x => x.Meta.Title) : snippets.OrderByDescending(x => x.Meta.Title),
                OrderBy.Created => ascending ? snippets.OrderBy(x => x.Meta.CreatedUtc) : snippets.OrderByDescending(x => x.Meta.CreatedUtc),
                _ => throw new NotSupportedException($"Unable to sort by {nameof(orderBy)}")
            };

            var workspace = new Workspace();
            workspace.Settings.FilesExclude.MetaJson = hideMetaFiles;

            foreach (var snippet in orderedSnippets)
            {
                var folder = new Folder(snippet.Meta.Title, Path.GetRelativePath(_options.WorkspacePath, snippet.DirectoryPath));
                workspace.Add(folder);
            }

            return new WorkspacePackage($"{name}{Constants.WorkspaceFileExtension}", workspace, tags, languages);
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void UpdateWorkspace(WorkspaceDefinition definition, bool resetSettings, SortDirection sortDirection, OrderBy orderBy, Serializer serializer)
        {
            Settings settings;
            var workspaceFilePath = Path.Combine(_options.WorkspacePath, definition.FileName);
            if (resetSettings)
            {
                settings = new Settings();
            }
            else
            {
                var currentWorkspace = serializer.DeserializeFromJson<Workspace>(workspaceFilePath);
                settings = currentWorkspace.Settings;
            }

            IEnumerable<Snippet> filtered = _snippets.ToList();
            if (definition.Languages.Any())
                filtered = filtered.Where(x => x.Files.Any(f => definition.Languages.Contains(_fileAssociations.Lookup(f))));

            if (definition.Tags.Any())
                filtered = filtered.Where(x => x.Meta.Tags.Any(t => definition.Tags.Contains(t)));

            if (!filtered.Any())
            {
                File.Delete(workspaceFilePath);
                return;
            }

            var ascending = sortDirection == SortDirection.Ascending;
            var ordered = orderBy switch
            {
                OrderBy.Alphabetical => ascending ? filtered.OrderBy(x => x.Meta.Title) : filtered.OrderByDescending(x => x.Meta.Title),
                OrderBy.Created => ascending ? filtered.OrderBy(x => x.Meta.CreatedUtc) : filtered.OrderByDescending(x => x.Meta.CreatedUtc),
                _ => throw new NotSupportedException($"Unable to sort by {nameof(orderBy)}")
            };

            var workspace = new Workspace { Settings = settings };

            foreach (var snippet in ordered)
            {
                var folder = new Folder(snippet.Meta.Title, Path.GetRelativePath(_options.WorkspacePath, snippet.DirectoryPath));
                workspace.Add(folder);
            }

            serializer.SerializeToJson(workspace, workspaceFilePath);
        }

        private void Load()
        {
            _snippets = new List<Snippet>();
            var serializer = new Serializer();
            foreach (var dir in new DirectoryInfo(_options.SnippetPath).GetDirectories())
            {
                var snippetFiles = dir.GetFiles().Where(x => x.Name != Meta.FileName).Select(x => x.FullName).ToList();
                var metadataPath = dir.GetFiles().Single(x => x.Name == Meta.FileName).FullName;
                var snippet = new Snippet
                {
                    DirectoryPath = dir.FullName,
                    Files = snippetFiles,
                    Meta = serializer.DeserializeFromYaml<Meta>(metadataPath)
                };

                _snippets.Add(snippet);
            }
        }
    }
}
