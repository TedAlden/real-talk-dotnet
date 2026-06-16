using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RealTalk.Api.Data;
using RealTalk.Api.Endpoints;
using RealTalk.Shared.Dto;

namespace RealTalk.Api.Features.Users
{
    public class GetUsers : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet("/users", HandleAsync)
            .WithName("GetUsers");

        private static async Task<Ok<List<UserDto>>> HandleAsync(RealTalkDbContext db)
        {
            var users = await db.Users
                .Select(u => u.ToDto())
                .ToListAsync();

            return TypedResults.Ok(users);
        }
    }
}
