using StackExchange.Redis;

namespace WebAPIGateway.Domain
{
    public class Redis
    {
        public Redis()
        {
            this.Instance = this.Instance ?? ConnectionMultiplexer.Connect(Configuration.CacheConnectionString).GetDatabase();
        }

        public IDatabase Instance { get; private set; }
    }
}
