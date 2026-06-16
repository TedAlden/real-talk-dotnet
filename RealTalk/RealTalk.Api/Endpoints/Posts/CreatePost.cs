using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using RealTalk.Api.Data;
using RealTalk.Api.Endpoints;
using RealTalk.Api.Entities;
using RealTalk.Shared.Dto;

namespace RealTalk.Api.Features.Posts
{
    public class CreatePost : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost("/posts", HandleAsync);

        public class Validator : AbstractValidator<CreatePostRequest>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty();
                RuleFor(x => x.Content).NotEmpty().MaximumLength(5000);
            }
        }

        private static async Task<Created<CreatePostResponse>> HandleAsync(CreatePostRequest request, RealTalkDbContext context)
        {
            var entity = new Post
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
            };

            context.Posts.Add(entity);
            await context.SaveChangesAsync();

            var response = new CreatePostResponse(entity.Id);

            return TypedResults.Created($"/posts/{entity.Id}", response);
        }
    }
}
