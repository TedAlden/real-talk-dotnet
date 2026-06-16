using RealTalk.Api.Endpoints;
using System.Reflection;

namespace RealTalk.Api.Extensions
{
    public static class EndpointExtensions
    {
        public static void MapEndpoints(this IEndpointRouteBuilder app)
        {
            var endpointTypes = typeof(Program).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(IEndpoint)));

            foreach (var type in endpointTypes)
            {
                var method = type.GetMethod(nameof(IEndpoint.Map), BindingFlags.Public | BindingFlags.Static);
                method?.Invoke(null, new object[] { app });
            }
        }
    }
}
