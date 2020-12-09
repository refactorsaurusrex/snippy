using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Snippy.Infrastructure
{
    internal class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        private readonly string _token;

        public AuthenticatedHttpClientHandler(string token) => _token = token;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var auth = request.Headers.Authorization;
            if (auth != null) request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, _token);
            return base.SendAsync(request, cancellationToken);
        }
    }
}