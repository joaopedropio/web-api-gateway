using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebAPIGateway.Domain;
using WebAPIGateway.Helpers;

namespace WebAPIGateway.Tests
{
    public class ServiceValidationsTests
    {
        [Test]
        public void Should_GetAdmin_When_AdminExists()
        {
            var serviceName = "servico";
            var serviceUrl = "http://servico";
            var cache = new CacheMock();
            var jsonExpected = new JsonResponse()
            {
                Data = new { serviceName, serviceUrl },
                StatusCode = HttpStatusCode.OK
            };

            var jsonResult = Task.Run(() => ServiceValidations.GetService(cache, serviceName)).Result;

            Assert.AreEqual(jsonExpected.StatusCode, jsonResult.StatusCode);
        }
    }
}
