using System.Reflection;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

namespace TigreDoMexico.Quizz.Api.Middlewares.Module;

public static class ModuleDiscovery
{
    private static readonly Type ModuleType = typeof(IModule);

    public static WebApplicationBuilder AddModules(this WebApplicationBuilder builder)
    {
        var currentAssembly = typeof(ModuleDiscovery).Assembly;

        var moduleTypes = GetModuleTypes(currentAssembly);

        foreach (var type in moduleTypes)
        {
            var method = GetMapModuleMethod(type);
            method?.Invoke(null, [builder]);
        }

        return builder;
    }

    private static IEnumerable<Type> GetModuleTypes(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(x => ModuleType.IsAssignableFrom(x) &&
                        x is { IsInterface: false, IsAbstract: false });
    }

    private static MethodInfo? GetMapModuleMethod(IReflect type)
    {
        return type.GetMethod(nameof(IModule.ConfigureServices),
            BindingFlags.Static | BindingFlags.Public);
    }

}