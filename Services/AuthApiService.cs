using HongDucFashion.ModelDTO;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace HongDucFashion.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _nav;

        public AuthApiService(HttpClient http, NavigationManager nav)
        {
            _http = http;
            _nav = nav;
        }

        private string ApiUrl(string path) => new Uri(new Uri(_nav.BaseUri), $"api/auth/{path}").ToString();

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync(ApiUrl("login"), request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<AuthResponse>();
            return null;
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            var response = await _http.PostAsJsonAsync(ApiUrl("register"), request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<AuthResponse>();
            return null;
        }

        public async Task<AuthResponse?> GetUserAsync(int userId)
        {
            return await _http.GetFromJsonAsync<AuthResponse>(ApiUrl($"me/{userId}"));
        }

        public async Task LogoutAsync()
        {
            await _http.PostAsync(ApiUrl("logout"), null); // Đảm bảo backend có endpoint /api/auth/logout
            // Nếu dùng JWT/localStorage, xóa token ở đây
        }
    }
}


