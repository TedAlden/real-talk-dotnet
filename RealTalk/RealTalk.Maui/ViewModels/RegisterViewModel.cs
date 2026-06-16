using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RealTalk.Maui.Services;
using RealTalk.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealTalk.Maui.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty] public partial string Email { get; set; }

        [ObservableProperty] public partial string Username { get; set; }

        [ObservableProperty] public partial string Password { get; set; }

        private readonly ApiService _apiService;
        private readonly AuthService _authService;

        public RegisterViewModel()
        {
            _apiService = Application.Current.Handler.GetRequiredService<ApiService>();
            _authService = Application.Current.Handler.GetRequiredService<AuthService>();
        }

        [RelayCommand]
        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                return;
            }

            var request = new RegisterRequest(Email, Username, Password);

            var res = await _apiService.RegisterAsync(request);

            if (res.Success)
            {
                await Application.Current.MainPage.DisplayAlertAsync("Register success", $"User ID: {res.Data?.Id}", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlertAsync("Register failed", $"", "OK");
            }
        }
    }
}
