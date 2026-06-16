using RealTalk.Maui.Models;
using RealTalk.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace RealTalk.Maui.Services
{
    public partial class ApiService
    {
        public Task<ApiResponse<PostResponseDto>> CreatePostAsync(PostDto newPost) =>
            ExecuteApiCallAsync<PostResponseDto>(HttpMethod.Post, "posts", newPost);

        public Task<ApiResponse<List<PostDto>>> GetPostsAsync() =>
            ExecuteApiCallAsync<List<PostDto>>(HttpMethod.Get, "posts");

        public Task<ApiResponse<PostDto>> GetPostAsync(Guid id) =>
            ExecuteApiCallAsync<PostDto>(HttpMethod.Get, $"post/{id}");
    }

    public class PostResponseDto
    {
        // Use [JsonPropertyName] if your API sends "id" (lowercase) 
        // and your JSON options aren't set to CaseInsensitive.
        public Guid Id { get; set; }
    }
}
