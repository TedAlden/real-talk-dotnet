using Microsoft.EntityFrameworkCore;
using RealTalk.Api.Data;
using RealTalk.Shared.Dto;

namespace RealTalk.Api.Endpoints.Auth
{
    public class RefreshToken : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost("/auth/refresh", HandleAsync)
            .AllowAnonymous();

        private static async Task<IResult> HandleAsync(RefreshTokenRequest request, RealTalkDbContext context, IConfiguration config)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

            if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Results.Unauthorized();
            }

            var newAuthToken = AuthHelpers.GenerateAuthToken(user, config);
            var newRefreshToken = AuthHelpers.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();

            return TypedResults.Ok(new RefreshTokenResponse(newAuthToken, newRefreshToken));
        }
    }
}
