using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Hospital_ManagementSystem_Blazor.Services
{
    public class JwtAuthenticationStateProvider
        : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public JwtAuthenticationStateProvider(
            ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("token");

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(
                    new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = new ClaimsIdentity(ParseClaims(token), "jwt");

            return new AuthenticationState(
                new ClaimsPrincipal(identity));
        }

        public async Task NotifyUserAuthentication(string token)
        {
            await _localStorage.SetItemAsync("token", token);

            var authenticatedUser =
                new ClaimsPrincipal(
                    new ClaimsIdentity(ParseClaims(token), "jwt"));

            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    new AuthenticationState(authenticatedUser)));
        }

        public async Task NotifyUserLogout()
        {
            await _localStorage.RemoveItemAsync("token");

            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    new AuthenticationState(
                        new ClaimsPrincipal(
                            new ClaimsIdentity()))));
        }

        private IEnumerable<Claim> ParseClaims(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();

            var token = handler.ReadJwtToken(jwt);

            return token.Claims;
        }
    }
}
