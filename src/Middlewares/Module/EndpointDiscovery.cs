using System.Reflection;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

namespace TigreDoMexico.Quizz.Api.Middlewares.Module;

public static class EndpointDiscovery
{
    private static readonly Type EndpointType = typeof(IEndpoint);

    public static void RegisterEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var currentAssembly = typeof(EndpointDiscovery).Assembly;

        var endpointTypes = GetEndpointTypes(currentAssembly);

        foreach (var type in endpointTypes)
        {
            var method = GetMapEndpointMethod(type);
            method?.Invoke(null, [endpoints]);
        }
    }

    private static IEnumerable<Type> GetEndpointTypes(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(x => EndpointType.IsAssignableFrom(x) &&
                        x is { IsInterface: false, IsAbstract: false });
    }

    private static MethodInfo? GetMapEndpointMethod(IReflect type)
    {
        return type.GetMethod(nameof(IEndpoint.MapEndpoint),
            BindingFlags.Static | BindingFlags.Public);
    }
}