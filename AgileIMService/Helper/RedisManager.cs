using StackExchange.Redis;

namespace AgileIM.Service.Helper
{
    public class RedisManager
    {

        public static ConnectionMultiplexer redisManager;

        IDatabase? _db;

        public void InitConnect(IConfiguration configuration)
        {
            try
            {
                var redisConnection = configuration.GetConnectionString("RedisConnectionString");
                redisManager = ConnectionMultiplexer.Connect(redisConnection);
                _db = redisManager.GetDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                redisManager = null;
                _db = null;
            }
        }


    }
}
