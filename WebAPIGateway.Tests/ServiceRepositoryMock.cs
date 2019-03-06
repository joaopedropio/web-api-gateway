using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIGateway.Domain;

namespace WebAPIGateway.Tests
{
    public class ServiceRepositoryMock : IServiceRepository
    {
        private IList<IService> services;

        public ServiceRepositoryMock()
        {
            this.services = new List<IService>();
        }

        public ServiceRepositoryMock(IList<IService> services)
        {
            this.services = services;
        }

        public async Task RemoveAsync(string serviceName)
        {
            var service = services.FirstOrDefault(s => s.Name == serviceName);
            await Task.FromResult(services.Remove(service));
        }

        public async Task<IService> RetrieveAsync(string serviceName)
        {
            return await Task.FromResult(services.FirstOrDefault(s => s.Name == serviceName));
        }

        public async Task StoreAsync(IService service)
        {
            await Task.Run(() => services.Add(service));
        }
    }
}
