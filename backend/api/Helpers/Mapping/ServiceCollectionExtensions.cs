using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pims.Dal;

namespace Pims.Api.Helpers.Mapping
{
    /// <summary>
    /// ServiceCollectionExtensions static class, provides extension methods for IServiceCollection.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Mapster to the DI service collection.
        /// By default this will scan the assembly for all mappers that inhert IRegister.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddMapster(this IServiceCollection services, Action<TypeAdapterConfig> options = null)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetAssembly(typeof(Startup)));

            options?.Invoke(config);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }

        /// <summary>
        /// Add Mapster to the DI service collection.
        /// By default this will scan the assembly for all mappers that inherit IRegister.
        /// Also resolves mappers that require custom constructor arguments.
        /// ################################################
        /// Regrettably unable to discover how to use dependency injection correctly with Mapster.
        /// Which requires manually creating instances of each mapper instead of dynamically creating them with the normal service provider.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serializerOptions"></param>
        /// <param name="pimsOptions"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddMapster(this IServiceCollection services, JsonSerializerOptions serializerOptions, PimsOptions pimsOptions, Action<TypeAdapterConfig> options = null)
        {
            var config = TypeAdapterConfig.GlobalSettings;

            var optionsSerializer = Options.Create(serializerOptions);
            var optionsPims = Options.Create(pimsOptions);
            var assemblies = new[] { Assembly.GetAssembly(typeof(Startup)) };
            var registers = assemblies.Select(assembly => assembly.GetTypes()
                .Where(x => typeof(IRegister).GetTypeInfo().IsAssignableFrom(x.GetTypeInfo()) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract))
                .SelectMany(registerTypes =>
                    registerTypes.Select(registerType =>
                    {
                        var constructor = registerType.GetConstructor(Type.EmptyTypes);
                        if (constructor != null)
                        {
                            return (IRegister)Activator.CreateInstance(registerType);
                        }

                        constructor = registerType.GetConstructor(new[] { typeof(IOptions<JsonSerializerOptions>), typeof(IOptions<PimsOptions>) });
                        if (constructor != null)
                        {
                            return (IRegister)Activator.CreateInstance(registerType, new object[] { optionsSerializer, optionsPims });
                        }

                        // Default to providing serializer options.
                        return (IRegister)Activator.CreateInstance(registerType, new[] { optionsSerializer });
                    })).ToList();

            config.Apply(registers);

            options?.Invoke(config);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}
