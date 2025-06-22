using HongDucFashion.ModelDTO;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace HongDucFashion.Services
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        private readonly IJSRuntime _js;
        private readonly AuthApiService _authApiService;
        private const string UserInfoKey = "authUserInfo";

        public ApiAuthenticationStateProvider(IJSRuntime js, AuthApiService authApiService)
        {
            _js = js;
            _authApiService = authApiService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var json = await _js.InvokeAsync<string>("localStorage.getItem", UserInfoKey);
                if (string.IsNullOrWhiteSpace(json))
                    return new AuthenticationState(_anonymous);

                var userInfo = JsonSerializer.Deserialize<AuthUserInfo>(json);
                if (userInfo == null || string.IsNullOrEmpty(userInfo.UserName))
                    return new AuthenticationState(_anonymous);

                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, userInfo.UserName)
                };
                if (userInfo.Roles != null)
                    claims.AddRange(userInfo.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

                var identity = new ClaimsIdentity(claims, "apiauth_type");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch (InvalidOperationException)
            {
                // Đang prerender, không thể gọi JS interop
                return new AuthenticationState(_anonymous);
            }
        }

        public async Task MarkUserAsAuthenticated(string userName, List<string> roles)
        {
            // Lưu user info vào localStorage
            var userInfo = new AuthUserInfo { UserName = userName, Roles = roles };
            var json = JsonSerializer.Serialize(userInfo);
            await _js.InvokeVoidAsync("localStorage.setItem", UserInfoKey, json);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", UserInfoKey);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }

        public void ForceRefresh()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
