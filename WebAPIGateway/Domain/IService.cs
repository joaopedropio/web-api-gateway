using WebAPIGateway.JSON;

namespace WebAPIGateway.Domain
{
    public interface IService : IJsonData
    {
        string Name { get; set; }
        string URL { get; set; }
    }
}
