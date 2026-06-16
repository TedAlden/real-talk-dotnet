using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RealTalk.Api.Data;
using RealTalk.Api.Entities;
using RealTalk.Shared.Dto;

namespace RealTalk.Api.Endpoints.Auth
{
    public class RegisterUser : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost("/auth/register", HandleAsync)
            .AllowAnonymous();

        public class Validator : AbstractValidator<RegisterRequest>
        {
            public Validator()
            {
                RuleFor(x => x.Username).NotEmpty().MaximumLength(20);
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            }
        }

        private static async Task<IResult> HandleAsync(RegisterRequest request, RealTalkDbContext context)
        {
            if (await context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return Results.Conflict("A user with this email already exists.");
            }

            if (await context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return Results.Conflict("A user with this username already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return Results.Created($"/users/{user.Id}", new RegisterResponse(user.Id));
        }
    }
}
