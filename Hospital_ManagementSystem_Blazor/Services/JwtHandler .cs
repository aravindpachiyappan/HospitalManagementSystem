using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace Hospital_ManagementSystem_Blazor.Services
{
    public class JwtHandler : DelegatingHandler
    {
        private readonly ILocalStorageService storage;

        public JwtHandler(ILocalStorageService storage)
        {
            this.storage = storage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token =
                await storage.GetItemAsync<string>("token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }

            return await base.SendAsync(
                request,
                cancellationToken);
        }
    }
}
