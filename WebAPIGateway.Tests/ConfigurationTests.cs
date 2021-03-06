using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using WebAPIGateway;
using System.Linq;
using WebAPIGateway.Domain;

namespace WebAPIGateway.Tests
{
    public class ConfigurationTests
    {
        IConfigurationRoot emptyConfiguration;
        IConfigurationRoot filledConfiguration;

        [SetUp]
        public void Setup()
        {
            this.emptyConfiguration = new ConfigurationBuilder().AddInMemoryCollection().Build();

            var variables = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("API_DOMAIN", "meusite"),
                new KeyValuePair<string, string>("API_PORT", "12345"),
                new KeyValuePair<string, string>("CACHE_CONFIG", "cachedomain"),
                new KeyValuePair<string, string>("SERVICES", "talservico,http://talservico;outroservico,http://outroservico"),
                new KeyValuePair<string, string>("USE_REDIS", "true"),
            };

            this.filledConfiguration = new ConfigurationBuilder().AddInMemoryCollection(variables).Build();
        }

        [Test]
        public void Should_GetDefaultSettings_When_NoVariableIsProvided()
        {
            Configuration.Build(this.emptyConfiguration);
            Assert.AreEqual(false, Configuration.UseRedisCache);
            Assert.AreEqual("localhost", Configuration.CacheConnectionString);
            Assert.AreEqual("http://*:80", Configuration.URL);
            Assert.AreEqual(new List<IService>(), Configuration.Services);
        }

        [Test]
        public void Should_GetCustomSettings_When_VariablesAreProvided()
        {
            Configuration.Build(this.filledConfiguration);
            Assert.AreEqual(true, Configuration.UseRedisCache);
            Assert.AreEqual("cachedomain", Configuration.CacheConnectionString);
            Assert.AreEqual("http://meusite:12345", Configuration.URL);
            Assert.AreEqual(new List<IService>()
            {
                new Service("talservico", "http://talservico"),
                new Service("outroservico", "http://outroservico")

            }, Configuration.Services);
        }
    }
}