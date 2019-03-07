using NUnit.Framework;
using System.Collections.Generic;
using static System.Net.HttpStatusCode;
using System.Threading.Tasks;
using WebAPIGateway.Domain;
using WebAPIGateway.JSON;
using Newtonsoft.Json;

namespace WebAPIGateway.Tests
{
    public class ServiceActionsTests
    {
        [Test]
        public void Should_GetAdmin_When_AdminExists()
        {
            // Arrange
            var serviceName = "servico";
            var service = new Service(serviceName, "http://servico");
            var services = new List<IService>() { service };
            var serviceRepo = new ServiceInMemoryRepository(services);
            var actions = new ServiceActions(serviceRepo);
            var jsonExpected = new JsonResponse()
            {
                Data = service,
                StatusCode = OK
            };

            // Act
            var jsonResult = Task.Run(() => actions.GetService(serviceName)).Result;

            // Assert
            Assert.IsInstanceOf<IService>(jsonResult.Data);
            Assert.AreEqual(jsonExpected.StatusCode, jsonResult.StatusCode);
            Assert.AreEqual(jsonExpected.Data, jsonResult.Data);
        }

        [Test]
        public void Should_NotGetAdmin_When_AdminDoesNotExist()
        {
            var serviceName = "servico";
            var services = new List<IService>();
            var serviceRepo = new ServiceInMemoryRepository(services);
            var actions = new ServiceActions(serviceRepo);
            var jsonExpected = new JsonResponse()
            {
                Data = new ErrorJsonData(Messages.ServiceNotFound),
                StatusCode = NotFound
            };

            // Act
            var jsonResult = Task.Run(() => actions.GetService(serviceName)).Result;

            // Assert
            Assert.IsInstanceOf<ErrorJsonData>(jsonResult.Data);
            Assert.AreEqual(jsonExpected.StatusCode, jsonResult.StatusCode);
            Assert.AreEqual(jsonExpected.Data, jsonResult.Data);
        }

        [Test]
        public void Should_NotGetAdmin_When_NameIsEmpty()
        {
            var serviceName = string.Empty;
            var services = new List<IService>();
            var serviceRepo = new ServiceInMemoryRepository(services);
            var actions = new ServiceActions(serviceRepo);
            var jsonExpected = new JsonResponse()
            {
                Data = new ErrorJsonData(Messages.ServiceNotProvided),
                StatusCode = BadRequest
            };

            // Act
            var jsonResult = Task.Run(() => actions.GetService(serviceName)).Result;

            // Assert
            Assert.IsInstanceOf<ErrorJsonData>(jsonResult.Data);
            Assert.AreEqual(jsonExpected.StatusCode, jsonResult.StatusCode);
            Assert.AreEqual(jsonExpected.Data, jsonResult.Data);
        }

        [Test]
        public void Should_PostAdmin_When_AdminIsValid()
        {
            var service = new Service("servico", "http://servico");
            var serviceRepo = new ServiceInMemoryRepository();
            var actions = new ServiceActions(serviceRepo);
            var jsonExpected = new JsonResponse()
            {
                Data = new SuccessJsonData(Messages.ServiceAdded),
                StatusCode = Created
            };

            // Act
            var jsonResult = Task.Run(() => actions.PostService(service)).Result;
            var serviceJsonResult = Task.Run(() => actions.GetService(service.Name)).Result;

            // Assert
            Assert.IsInstanceOf<SuccessJsonData>(jsonResult.Data);
            Assert.AreEqual(jsonExpected.StatusCode, jsonResult.StatusCode);
            Assert.AreEqual(jsonExpected.Data, jsonResult.Data);
            Assert.AreEqual(service, serviceJsonResult.Data);
        }

        [Test]
        public void Should_NotPostAdmin_When_AdminNameIsInValid()
        {
            var service = new Service("", "http://servico");
            var serviceRepo = new ServiceInMemoryRepository();
            var actions = new ServiceActions(serviceRepo);
            var jsonExpected = new JsonResponse()
            {
                Data = new ErrorJsonData(Messages.ServiceNameCantBeEmpty),
                StatusCode = BadRequest
            };

            // Act
            var jsonResult = Task.Run(() => actions.PostService(service)).Result;

            // Assert
            Assert.IsInstanceOf<ErrorJsonData>(jsonResult.Data);
            Assert.AreEqual(jsonExpected.StatusCode, jsonResult.StatusCode);
            Assert.AreEqual(jsonExpected.Data, jsonResult.Data);
        }

        [Test]
        public void Should_NotPostAdmin_When_AdminURLIsInValid()
        {
            var service = new Service("servico", "");
            var serviceRepo = new ServiceInMemoryRepository();
            var actions = new ServiceActions(serviceRepo);
            var jsonExpected = new JsonResponse()
            {
                Data = new ErrorJsonData(Messages.ServiceURLCantBeEmpty),
                StatusCode = BadRequest
            };

            // Act
            var jsonResult = Task.Run(() => actions.PostService(service)).Result;

            // Assert
            Assert.IsInstanceOf<ErrorJsonData>(jsonResult.Data);
            Assert.AreEqual(jsonExpected.StatusCode, jsonResult.StatusCode);
            Assert.AreEqual(jsonExpected.Data, jsonResult.Data);
        }

        [Test]
        public void Should_NotPostAdmin_When_AdminIsInValid()
        {
            Service service = null;
            var serviceRepo = new ServiceInMemoryRepository();
            var actions = new ServiceActions(serviceRepo);
            var jsonExpected = new JsonResponse()
            {
                Data = new ErrorJsonData(Messages.ServiceNameCantBeEmpty),
                StatusCode = BadRequest
            };

            // Act
            var jsonResult = Task.Run(() => actions.PostService(service)).Result;

            // Assert
            Assert.IsInstanceOf<ErrorJsonData>(jsonResult.Data);
            Assert.AreEqual(jsonExpected.StatusCode, jsonResult.StatusCode);
            Assert.AreEqual(jsonExpected.Data, jsonResult.Data);
        }

        [Test]
        public void Should_DeleteAdmin_When_AdminIsValid()
        {
            var serviceName = "servico";
            var service = new Service(serviceName, "http://servico");
            var services = new List<IService>() { service };
            var serviceRepo = new ServiceInMemoryRepository(services);
            var actions = new ServiceActions(serviceRepo);
            var jsonExpected = new JsonResponse()
            {
                Data = new SuccessJsonData(Messages.ServiceDeleted),
                StatusCode = NoContent
            };

            // Act
            var jsonResult = Task.Run(() => actions.DeleteService(serviceName)).Result;

            // Assert
            Assert.IsInstanceOf<SuccessJsonData>(jsonResult.Data);
            Assert.AreEqual(jsonExpected.StatusCode, jsonResult.StatusCode);
            Assert.AreEqual(jsonExpected.Data, jsonResult.Data);
        }

        [Test]
        public void Should_NotDeleteAdmin_When_AdminDoesNotExist()
        {
            var serviceName = "servico";
            var serviceRepo = new ServiceInMemoryRepository();
            var actions = new ServiceActions(serviceRepo);
            var jsonExpected = new JsonResponse()
            {
                Data = new ErrorJsonData(Messages.ServiceNotFound),
                StatusCode = NotFound
            };

            // Act
            var jsonResult = Task.Run(() => actions.DeleteService(serviceName)).Result;

            // Assert
            Assert.IsInstanceOf<ErrorJsonData>(jsonResult.Data);
            Assert.AreEqual(jsonExpected.StatusCode, jsonResult.StatusCode);
            Assert.AreEqual(jsonExpected.Data, jsonResult.Data);
        }

        [Test]
        public void Should_NotDeleteAdmin_When_AdminIsInvalid()
        {
            var serviceName = "";
            var serviceRepo = new ServiceInMemoryRepository();
            var actions = new ServiceActions(serviceRepo);
            var jsonExpected = new JsonResponse()
            {
                Data = new ErrorJsonData(Messages.ServiceNotProvided),
                StatusCode = BadRequest
            };

            // Act
            var jsonResult = Task.Run(() => actions.DeleteService(serviceName)).Result;

            // Assert
            Assert.IsInstanceOf<ErrorJsonData>(jsonResult.Data);
            Assert.AreEqual(jsonExpected.StatusCode, jsonResult.StatusCode);
            Assert.AreEqual(jsonExpected.Data, jsonResult.Data);
        }
    }
}
