using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealTalk.Api.Data;
using RealTalk.Api.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using RealTalk.Shared.Dto;

namespace RealTalk.Api.Endpoints.Auth
{
    public class LoginUser : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost("/auth/login", HandleAsync)
            .AllowAnonymous();

        public class Validator : AbstractValidator<LoginRequest>
        {
            public Validator()
            {
                RuleFor(x => x.Username).NotEmpty().EmailAddress();
            }
        }

        private static async Task<IResult> HandleAsync(LoginRequest request, RealTalkDbContext context, IConfiguration config)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user is null)
            {
                return Results.Unauthorized();
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.Unauthorized();
            }

            var authToken = AuthHelpers.GenerateAuthToken(user, config);
            var refreshToken = AuthHelpers.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();

            return TypedResults.Ok(new LoginResponse(authToken, refreshToken));
        }
    }
}
