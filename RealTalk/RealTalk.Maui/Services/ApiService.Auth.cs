using RealTalk.Maui.Models;
using RealTalk.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealTalk.Maui.Services
{
    public partial class ApiService
    {
        public Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest) =>
            ExecuteApiCallAsync<RegisterResponse>(HttpMethod.Post, "auth/register", registerRequest);

        public Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest loginRequest) =>
            ExecuteApiCallAsync<LoginResponse>(HttpMethod.Post, "auth/login", loginRequest);

        public Task<ApiResponse<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest) =>
            ExecuteApiCallAsync<RefreshTokenResponse>(HttpMethod.Post, "auth/refresh", refreshTokenRequest);
    }
}
