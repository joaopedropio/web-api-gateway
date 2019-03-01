using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAPIGateway.Tests
{
    public class ServiceTests
    {
        [TestCase(null)]
        [TestCase("")]
        public void Should_BeEmpty_When_NoServiceIsProvided(string service)
        {
            Assert.AreEqual(new List<Service>(), Service.ParseServices(service));
        }

        [Test]
        public void Should_ParseCorrectly_When_OneValidServiceIsProvided()
        {
            var service = "servico,http://servico";

            var expected = new List<Service>()
            {
                new Service("servico", "http://servico")
            };

            var actual = Service.ParseServices(service).ToList();

            Assert.AreEqual(expected, actual);
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

        [TestCase("servico1;servico2")]
        [TestCase("servico1,http://servico1;servico2")]
        [TestCase("servico1")]
        [TestCase(";;;")]
        [TestCase(",,")]
        [TestCase("servico1,servico2,servico3")]
        public void Should_ThrowException_When_ServiceIsInvalid(string service)
        {
            Assert.Throws<Exception>(() => Service.ParseServices(service), "Services provided are not valid");
        }
    }
}
