using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Refit;
using Snippy.Models;

namespace Snippy.Api
{
    [Headers("Authorization: Bearer", "Accept: application/vnd.github.v3+json", "Content-Type: application/json", "User-Agent: refactorsaurusrex")]
    public interface IGitHubApi
    {
        [Post("/gists")]
        Task<GistResponse> CreateGist([Body]JObject request);

        [Get("/gists?per_page=100&page={page}")]
        Task<List<GistResponse>> ListGists(int page);

        [Patch("/gists/{id}")]
        Task<GistResponse> UpdateGist(string id, [Body]JObject request);

        [Get("/gists/{id}")]
        Task<GistResponse> GetGist(string id);
    }
}
