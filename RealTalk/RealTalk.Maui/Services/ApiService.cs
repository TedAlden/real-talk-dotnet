using RealTalk.Maui.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace RealTalk.Maui.Services
{
    public partial class ApiService
    {
        protected readonly HttpClient _httpClient;
        protected readonly JsonSerializerOptions _jsonSerializerOptions;

        //public ApiService(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //    _jsonSerializerOptions = new JsonSerializerOptions
        //    {
        //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //        WriteIndented = true,
        //    };
        //}

        public ApiService()
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };

            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://192.168.0.107:5123"),
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        protected async Task<ApiResponse<T>> ExecuteApiCallAsync<T>(HttpMethod method, string endpoint, object? data = null, CancellationToken ct = default, bool isRetry = false)
        {
            try
            {
                using var request = new HttpRequestMessage(method, endpoint);

                var token = await SecureStorage.Default.GetAsync("auth_token");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                if (data != null)
                {
                    request.Content = JsonContent.Create(data, options: _jsonSerializerOptions);
                }

                var response = await _httpClient.SendAsync(request, ct).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions, ct).ConfigureAwait(false);
                    return new ApiResponse<T> { Data = result };
                }

                // If unauthorised, the auth token may need refreshing. Try this once.
                if (response.StatusCode == HttpStatusCode.Unauthorized && !isRetry)
                {
                    AuthService _authService = Application.Current!.Handler.GetRequiredService<AuthService>();

                    if (await _authService.RefreshTokensAsync())
                    {
                        return await ExecuteApiCallAsync<T>(method, endpoint, data, ct, isRetry: true);
                    }
                }

                var errorType = response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => ErrorType.Unauthorized,
                    HttpStatusCode.InternalServerError => ErrorType.ServerError,
                    _ => ErrorType.ServerError
                };

                return new ApiResponse<T> { Success = false, ErrorType = errorType };
            }
            catch (HttpRequestException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
            {
                return new ApiResponse<T> { Success = false, ErrorType = ErrorType.NoInternet };
            }
            //catch (TaskCanceledException)
            //{
            //    return new ApiResponse<T> { Success = false, ErrorType = ErrorType.Timeout };
            //}
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                return new ApiResponse<T> { Success = false, ErrorType = ErrorType.Cancelled, Message = "Request cancelled by user." };
            }
            catch (OperationCanceledException)
            {
                return new ApiResponse<T> { Success = false, ErrorType = ErrorType.Timeout };
            }
            catch (JsonException ex)
            {
                return new ApiResponse<T> { Success = false, ErrorType = ErrorType.SerializationError, Message = ex.Message };
            }
            catch (Exception ex)
            {
                return new ApiResponse<T> { Success = false, ErrorType = ErrorType.Unknown, Message = ex.Message };
            }
        }
    }
}
