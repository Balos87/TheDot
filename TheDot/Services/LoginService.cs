using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TheDot.Models;
using TheDot.Services.IServices;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using TheDot.Models.User;

namespace TheDot.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _ContextAccess;
        private readonly IConfiguration _Config;
        private readonly string _baseUrl;

        public LoginService(HttpClient httpClient, IHttpContextAccessor contentAccess, IConfiguration config)
        {
            _httpClient = httpClient;
            _ContextAccess = contentAccess;
            _Config = config;
            _baseUrl = config["ApiConnections:UserApiBaseUrl"];
        }

        public async Task<bool> LoginAsync(LoginViewModel loginModel)
        {
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiUrl = $"{_baseUrl}login";
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenResponse.Token);

            var claims = jwtToken.Claims;
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await _ContextAccess.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = jwtToken.ValidTo
                });

            _ContextAccess.HttpContext.Response.Cookies
                .Append
                (
                    "JwtToken", 
                    tokenResponse.Token, 
                    new CookieOptions
                        {
                            HttpOnly = false,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = jwtToken.ValidTo
                        });

            return true;
        }

        public async Task LogoutAsync()
        {
            await _ContextAccess.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _ContextAccess.HttpContext.Response.Cookies.Delete("JwtToken");
        }
    }
}
