using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RealTalk.Api.Data;
using RealTalk.Api.Endpoints;
using RealTalk.Shared.Dto;

namespace RealTalk.Api.Features.Posts
{
    public class GetPosts : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet("/posts", HandleAsync)
            .WithName("GetPosts")
            .RequireAuthorization();

        private static async Task<Ok<List<PostDto>>> HandleAsync(RealTalkDbContext db)
        {
            var posts = await db.Posts
                .Include(p => p.User)
                .Select(p => p.ToDto())
                .ToListAsync();

            return TypedResults.Ok(posts);
        }
    }
}
