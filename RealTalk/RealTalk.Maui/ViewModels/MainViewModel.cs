using CommunityToolkit.Mvvm.Input;
using RealTalk.Maui.Services;
using RealTalk.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealTalk.Maui.ViewModels
{
    public partial class MainViewModel
    {
        public ObservableRangeCollection<PostDto> Posts { get; set; } = new();

        public string PostBody { get; set; } = string.Empty;

        private readonly ApiService _apiService;
        private readonly AuthService _authService;

        public MainViewModel()
        {
            _apiService = Application.Current.Handler.GetRequiredService<ApiService>();
            _authService = Application.Current.Handler.GetRequiredService<AuthService>();
        }

        [RelayCommand]
        public async Task CreatePost()
        {
            var post = new PostDto()
            {
                UserId = new Guid(),
                Content = PostBody
            };

            var response = await _apiService.CreatePostAsync(post);

            PostResponseDto? res = response.Data;

            await Application.Current.MainPage.DisplayAlertAsync(response.Success ? "Success" : "Failure", res?.Id.ToString(), "OK");
        }

        [RelayCommand]
        public async Task GetPosts()
        {
            var result = await _apiService.GetPostsAsync();

            if (result.Success && result.Data is List<PostDto> posts)
            {
                Posts.ReplaceRange(posts);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlertAsync($"Error {result.ErrorType}", result.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task Logout()
        {
            await _authService.LogoutAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Application.Current?.Windows.Count > 0)
                {
                    Application.Current.Windows[0].Page = new LoginShell();
                }
            });
        }
    }
}
