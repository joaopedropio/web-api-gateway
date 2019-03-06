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
            // Arrange
            var serviceName = "servico";
            var service = new Service(serviceName, "http://servico");
            var services = new List<IService>() { service };
            var serviceRepo = new ServiceRepositoryMock(services);
            var jsonExpected = new JsonResponse()
            {
                Data = service,
                StatusCode = HttpStatusCode.OK
            };

            // Act
            var jsonResult = Task.Run(() => ServiceValidations.GetService(serviceRepo, serviceName)).Result;

            // Assert
            Assert.AreEqual(jsonExpected.StatusCode, jsonResult.StatusCode);
            Assert.AreEqual(jsonExpected.Data, jsonResult.Data);
        }
    }
}
