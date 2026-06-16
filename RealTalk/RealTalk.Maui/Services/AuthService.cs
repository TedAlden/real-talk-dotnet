using RealTalk.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealTalk.Maui.Services
{
    public class AuthService
    {
        private const string AuthTokenKey = "auth_token";
        private const string RefreshTokenKey = "refresh_token";

        private readonly ApiService _apiService;

        public AuthService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var request = new LoginRequest(username, password);

            var response = await _apiService.LoginAsync(request);

            if (response.Success && response.Data != null)
            {
                await SecureStorage.Default.SetAsync(AuthTokenKey, response.Data.Token);
                await SecureStorage.Default.SetAsync(RefreshTokenKey, response.Data.RefreshToken);
                return true;
            }
            return false;
        }

        public async Task<bool> RefreshTokensAsync()
        {
            var refreshToken = await SecureStorage.Default.GetAsync(RefreshTokenKey);
            if (string.IsNullOrEmpty(refreshToken))
            {
                return false;
            }

            var request = new RefreshTokenRequest(refreshToken);

            var response = await _apiService.RefreshTokenAsync(request);

            if (response.Success && response.Data != null)
            {
                await SecureStorage.Default.SetAsync(AuthTokenKey, response.Data.AuthToken);
                await SecureStorage.Default.SetAsync(RefreshTokenKey, response.Data.RefreshToken);
                return true;
            }

            // If refresh fails, tokens are likely invalid/expired
            await LogoutAsync();
            return false;
        }

        public async Task LogoutAsync()
        {
            SecureStorage.Default.Remove(AuthTokenKey);
            SecureStorage.Default.Remove(RefreshTokenKey);
        }
    }
}
