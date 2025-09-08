using System.Reflection;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

namespace TigreDoMexico.Quizz.Api.Middlewares.Module;

public static class ModuleDiscovery
{
    private static readonly Type ModuleType = typeof(IModule);

    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        var currentAssembly = typeof(ModuleDiscovery).Assembly;

        var moduleTypes = GetModuleTypes(currentAssembly);

        foreach (var type in moduleTypes)
        {
            var method = GetMapEndpointMethod(type);
            method?.Invoke(null, [services, configuration]);
        }

        return services;
    }

    private static IEnumerable<Type> GetModuleTypes(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(x => ModuleType.IsAssignableFrom(x) &&
                        x is { IsInterface: false, IsAbstract: false });
    }

    private static MethodInfo? GetMapEndpointMethod(IReflect type)
    {
        return type.GetMethod(nameof(IModule.ConfigureServices),
            BindingFlags.Static | BindingFlags.Public);
    }

}