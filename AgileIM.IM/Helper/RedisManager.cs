using Microsoft.Extensions.Configuration;

using StackExchange.Redis;

namespace AgileIM.IM.Helper
{
    public static class RedisManager
    {

        public static ConnectionMultiplexer? redisManager;

        static IDatabase? _db;

        public static void InitRedisManager(this IConfiguration configuration)
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
