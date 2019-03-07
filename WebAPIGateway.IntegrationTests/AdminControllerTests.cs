using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPIGateway.JSON;

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

        public string ReadBody(HttpResponseMessage httpResponse)
        {
            return Task.Run(() => httpResponse.Content.ReadAsStringAsync()).Result;
        }

        [Test]
        public void Should_GetAdmin_When_AdminExists()
        {
            var httpResponse = client.GetAsync("/admin/servico").Result;
            var body = ReadBody(httpResponse);
            var jsonResult = JsonConvert.DeserializeObject<ErrorJsonData>(body);

            Assert.AreEqual(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.AreEqual(new ErrorJsonData(Messages.ServiceNotFound), jsonResult);
        }
    }
}