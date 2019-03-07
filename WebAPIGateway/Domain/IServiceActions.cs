using System.Threading.Tasks;
using WebAPIGateway.JSON;

namespace WebAPIGateway.Domain
{
    public interface IServiceActions
    {
        Task<IJsonResponse> GetService(string serviceName);
        Task<IJsonResponse> PostService(IService service);
        Task<IJsonResponse> DeleteService(string serviceName);
    }
}
