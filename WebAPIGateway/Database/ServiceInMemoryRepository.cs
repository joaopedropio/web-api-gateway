using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIGateway.Domain
{
    public class ServiceInMemoryRepository : IServiceRepository
    {
        private IList<IService> services;

        public ServiceInMemoryRepository()
        {
            this.services = new List<IService>();
        }

        public ServiceInMemoryRepository(IList<IService> services)
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
