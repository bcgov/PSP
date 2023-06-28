using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Pims.Api.Models.Config;
using Pims.Core.Test;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Repositories.Mayan;
using Xunit;
using Microsoft.Extensions.Logging;

namespace Pims.Dal.Test.Repositories
{

    public class TestConfigurationProvider : ConfigurationProvider
    {
        private readonly IDictionary<string, string> _data;

        public TestConfigurationProvider(IDictionary<string, string> data)
        {
            _data = data;
        }

        public override void Load()
        {
            Data = _data;
        }
    }

    public class TestConfigurationSource : IConfigurationSource
    {
        private readonly IDictionary<string, string> _data;

        public TestConfigurationSource(IDictionary<string, string> data)
        {
            _data = data;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new TestConfigurationProvider(_data);
        }
    }

    public class TestMayanDocumentRepository : MayanDocumentRepository
    {
        public TestMayanDocumentRepository(ILogger<MayanDocumentRepository> logger, IHttpClientFactory httpClientFactory, IEdmsAuthRepository authRepository, IConfiguration configuration) : base(logger, httpClientFactory, authRepository, configuration)
        {
        }

        public MayanConfig GetMayanConfig()
        {
            return _config;
        }

    }

    public class MayanDocumentRepositoryTest
    {

        [Fact]
        public async void TryCreateDocumentTypeAsync_Success()
        {
            var helper = new TestHelper();

            var response = new HttpResponseMessage();
            var clientFactory = helper.CreateHttpClientFactory(response);

            var authRepository = new Mock<IEdmsAuthRepository>();
            authRepository.Setup(x => x.GetTokenAsync()).Returns(Task.FromResult("Test token"));

            var configurationData = new Dictionary<string, string>
            {
                {"Mayan:BaseUri", "https://example.com"},
                {"Mayan:ConnectionUser", "John Smith"},
                {"Mayan:ConnectionPassword", "password"}
            };

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.Add(new TestConfigurationSource(configurationData));

            var configuration = configurationBuilder.Build();

            var documentType = new DocumentType();

            var repository = helper.CreateRepository<TestMayanDocumentRepository>(clientFactory, configuration, authRepository);

            var result = await repository.TryCreateDocumentTypeAsync(documentType);

            repository.GetMayanConfig().BaseUri.Should().Be("https://example.com");

        }
    }
}

