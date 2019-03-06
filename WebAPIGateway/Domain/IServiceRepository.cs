using System.Threading.Tasks;

namespace WebAPIGateway.Domain
{
    public interface IServiceRepository
    {
        Task StoreAsync(IService service);
        Task<IService> RetrieveAsync(string serviceName);
        Task RemoveAsync(string serviceName);
    }
}
