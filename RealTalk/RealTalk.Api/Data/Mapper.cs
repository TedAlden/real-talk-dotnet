using RealTalk.Shared.Dto;
using RealTalk.Api.Entities;

namespace RealTalk.Api.Data
{
    public static class Mapper
    {
        public static UserDto ToDto(this User user) => new UserDto
        {
            Id = user.Id,
            Username = user.Username
        };

        //public static User FromDto(this UserDto userDto) => new User
        //{
        //    Id = userDto.Id,
        //    Username = userDto.Username,
        //    Email = userDto.Email
        //};

        public static PostDto ToDto(this Post post) => new PostDto
        {
            Id = post.Id,
            UserId = post.UserId,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            User = post.User.ToDto()
        };

        //public static Post FromDto(this PostDto postDto) => new Post
        //{
        //    Id = postDto.Id,
        //    UserId = postDto.UserId,
        //    Content = postDto.Content,
        //    CreatedAt = postDto.CreatedAt
        //};
    }
}