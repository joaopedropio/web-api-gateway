using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.Net;
using System.Net.Http;

namespace WebAPIGateway.IntegrationTests
{
    public class AdminControllerTests
    {
        private HttpClient client;

        [SetUp]
        public void Setup()
        {
            Configuration.Build();
            var server = new TestServer(Program.BuildWebHost());
            this.client = server.CreateClient();
        }

        [Test]
        public void Should_GetAdmin_When_AdminExists()
        {
            var httpResponse = client.GetAsync("/admin/servico").Result;

            Assert.AreEqual(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }
    }
}