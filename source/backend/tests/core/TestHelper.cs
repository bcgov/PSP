using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Dal;
using Pims.Dal.Configuration.Generators;

namespace Pims.Core.Test
{
    /// <summary>
    /// TestHelper class, provides a way to simplify the Arrange part of a test.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TestHelper
    {
        #region Properties

        /// <summary>
        /// get - The service provider.
        /// </summary>
        /// <value></value>
        public IServiceProvider Provider { get; private set; }

        /// <summary>
        /// get - The services collection.
        /// </summary>
        /// <value></value>
        public IServiceCollection Services { get; } = new ServiceCollection();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TestHelper class.
        /// </summary>
        public TestHelper()
        {
            var config = new TypeAdapterConfig();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("Pims"));

            var serializerOptions = Options.Create(new JsonSerializerOptions());
            var pimsOptions = Options.Create(new PimsOptions());
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
                            return (IRegister)Activator.CreateInstance(registerType, new object[] { serializerOptions, pimsOptions });
                        }
                        // Default to providing serializer options.
                        return (IRegister)Activator.CreateInstance(registerType, new[] { serializerOptions });
                    })).ToList();

            config.Apply(registers);

            config.Default.IgnoreNonMapped(true);
            config.Default.IgnoreNullValues(true);
            config.AllowImplicitDestinationInheritance = true;
            config.AllowImplicitSourceInheritance = true;
            config.Default.UseDestinationValue(member =>
                member.SetterModifier == AccessModifier.None &&
                member.Type.IsGenericType &&
                member.Type.GetGenericTypeDefinition() == typeof(ICollection<>));

            this.Services.AddSingleton<IntIdentityGenerator>();
            this.Services.AddSingleton(config);
            this.Services.AddSingleton<IMapper, ServiceMapper>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Build the required services for the provider.
        /// Once this is called you can no longer add additional services to the provider.
        /// </summary>
        /// <returns></returns>
        public IServiceProvider BuildServiceProvider()
        {
            if (this.Provider == null)
            {
                this.Provider = this.Services.BuildServiceProvider();
            }
            return this.Provider;
        }

        /// <summary>
        /// Add a singleton service to the provider, and include the mock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public Mock<T> AddSingletonWithMock<T>()
            where T : class
        {
            if (this.Provider != null)
            {
                throw new InvalidOperationException("Cannot add to the service collection once the provider has been built.");
            }

            var mock = new Mock<T>();
            this.Services.AddSingleton(mock.Object).AddSingleton(mock);
            return mock;
        }

        /// <summary>
        /// Add a singleton service to the provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public IServiceCollection AddSingleton<T>()
            where T : class
        {
            if (this.Provider != null)
            {
                throw new InvalidOperationException("Cannot add to the service collection once the provider has been built.");
            }

            return this.Services.AddSingleton<T>();
        }

        /// <summary>
        /// Add a singleton service to the provider.
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public IServiceCollection AddSingleton<T>(T item)
            where T : class
        {
            if (this.Provider != null)
            {
                throw new InvalidOperationException("Cannot add to the service collection once the provider has been built.");
            }

            return this.Services.AddSingleton<T>(item);
        }

        /// <summary>
        /// Add a singleton service to the provider.
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        public IServiceCollection AddSingleton<TService, TImplementation>(TImplementation item)
            where TService : class
            where TImplementation : class, TService
        {
            if (this.Provider != null)
            {
                throw new InvalidOperationException("Cannot add to the service collection once the provider has been built.");
            }

            return this.Services.AddSingleton<TService, TImplementation>((p) => item);
        }

        /// <summary>
        /// Add a singleton service to the provider.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public IServiceCollection AddSingleton(Type type, object item)
        {
            if (this.Provider != null)
            {
                throw new InvalidOperationException("Cannot add to the service collection once the provider has been built.");
            }

            return this.Services.AddSingleton(type, item);
        }

        /// <summary>
        /// Get the service for the specified 'T' type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>()
        {
            return this.BuildServiceProvider().GetService<T>();
        }

        /// <summary>
        /// Get the mocked service for the specified 'T' type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Mock<T> GetMock<T>()
            where T : class
        {
            return this.BuildServiceProvider().GetService<Mock<T>>();
        }

        /// <summary>
        /// Get the mapper from the service collection.
        /// </summary>
        /// <returns></returns>
        public IMapper GetMapper()
        {
            return this.BuildServiceProvider().GetService<IMapper>();
        }

        /// <summary>
        /// Creates an instance of the specified type 'T', using dependency injection for any constructor arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateInstance<T>()
        {
            return (T)ActivatorUtilities.CreateInstance(this.Provider, typeof(T));
        }

        public IFormFile GetFormFile(string text)
        {
            // Setup mock file using a memory stream
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;

            // create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            return file;
        }
        #endregion
    }
}
