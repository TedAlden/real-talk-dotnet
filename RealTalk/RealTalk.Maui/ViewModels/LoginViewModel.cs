using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RealTalk.Maui.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealTalk.Maui.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty] public partial string Username { get; set; }

        [ObservableProperty] public partial string Password { get; set; }

        private readonly AuthService _authService;

        public LoginViewModel()
        {
            _authService = Application.Current.Handler.GetRequiredService<AuthService>();
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                return;
            }

            bool success = await _authService.LoginAsync(Username, Password);

            if (success)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (Application.Current?.Windows.Count > 0)
                    {
                        Application.Current.Windows[0].Page = new AppShell();
                    }
                });
            }
            else
            {
                await Application.Current.MainPage.DisplayAlertAsync("Login failed", "", "OK");
            }
        }
    }
}
