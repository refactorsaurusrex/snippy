using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Refit;
using Snippy.Infrastructure;
using Snippy.Models;

namespace Snippy.Api
{
    internal class GitHub
    {
        private readonly IGitHubApi _gitHub;
        private static readonly CacheItemPolicy Policy = new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(15) };

        public GitHub(string accessToken)
        {
            var client = new HttpClient(new AuthenticatedHttpClientHandler(accessToken))
            {
                BaseAddress = new Uri("https://api.github.com")
            };

            _gitHub = RestService.For<IGitHubApi>(client);
        }

        public async Task<List<GistResponse>> ListGists(bool noCache)
        {
            if (!noCache && MemoryCache.Default[nameof(ListGists)] is List<GistResponse> cachedList)
                return cachedList;

            var gists = new List<GistResponse>();
            var page = 1;
            while (true)
            {
                var response = await _gitHub.ListGists(page++);

                if (response.Any())
                    gists.AddRange(response);
                else
                    break;
            }

            MemoryCache.Default.Set(nameof(ListGists), gists, Policy);
            return gists;
        }

        public async Task<string> CreateGist(string description, IEnumerable<SnippetFile> files, bool isPublic)
        {
            var request = new CreateGistRequest
            {
                Public = isPublic,
                Description = description,
                Files = files.Select(x => new GistFile { FileName = x.Name, Content = x.ReadAllText() }).ToList()
            };

            var response = await _gitHub.CreateGist(request.ToJObject());
            return response.Url;
        }

        public async Task<string> UpdateGist(string id, string description, IEnumerable<SnippetFile> files)
        {
            var request = new UpdateGistRequest
            {
                Description = description,
                Files = files.Select(x => new GistFile { FileName = x.Name, Content = x.ReadAllText() }).ToArray()
            };

            var response = await _gitHub.UpdateGist(id, request.ToJObject());
            return response.Url;
        }
    }
}