using Hospital_ManagementSystem_Blazor.DTOs.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Hospital_ManagementSystem_Blazor.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly JwtAuthenticationStateProvider _provider;

        public AuthService(
            HttpClient http,
            AuthenticationStateProvider provider)
        {
            _http = http;
            _provider = (JwtAuthenticationStateProvider)provider;
        }

        public async Task<bool> Login(LoginRequestDTO request)
        {
            var response = await _http.PostAsJsonAsync(
                "/api/auth/login",
                request);

            if (!response.IsSuccessStatusCode)
                return false;

            var result =
                await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

            if (result == null || string.IsNullOrWhiteSpace(result.Token))
                return false;

            await _provider.NotifyUserAuthentication(result.Token);

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    result.Token);

            return true;
        }

        public async Task<bool> Register(RegisterRequestDTO model)
        {
            var response = await _http.PostAsJsonAsync("api/user/add-user", model);
            return response.IsSuccessStatusCode;
        }

        public async Task Logout()
        {
            await _provider.NotifyUserLogout();
        }
    }
}
