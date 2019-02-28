using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace WebAPIGateway.Tests
{
    public class ServiceTests
    {
        [Test]
        public void Should_ParseCorrectly_When_OneValidServiceIsProvided()
        {
            var service = "servico,http://servico";

            var expected = new List<Service>()
            {
                new Service("servico", "http://servico")
            };

            var atual = Service.ParseServices(service).ToList();

            Assert.AreEqual(expected, atual);
        }


        [Test]
        public void Should_ParseCorrectly_When_MultipleValidServicesAreProvided()
        {
            var services = "servico1,http://servico1;servico2,http://servico2";

            Assert.AreEqual(new List<Service>()
            {
                new Service("servico1", "http://servico1"),
                new Service("servico2", "http://servico2")

            }, Service.ParseServices(services).ToList());
        }
    }
}
