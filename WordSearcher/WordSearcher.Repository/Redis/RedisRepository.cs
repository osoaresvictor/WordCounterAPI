using StackExchange.Redis;
using WordSearcher.Repository.Redis.Interfaces;

namespace WordSearcher.Repository.Redis
{
	public class RedisRepository : IRedisRepository
	{
		public ConnectionMultiplexer GetConnection(string host)
		{
			return ConnectionMultiplexer.Connect(host);
		}

		public string GetValue(ConnectionMultiplexer redisConnection, string key)
		{
			return redisConnection.GetDatabase().StringGet(key);
		}

		public bool SetValue(ConnectionMultiplexer redisConnection, string key, string value)
		{
			return redisConnection.GetDatabase().StringSet(key, value);
		}

		public void IncrementValue(ConnectionMultiplexer redisConnection, string key, double increment)
		{
			redisConnection.GetDatabase().StringIncrement(key, increment);
		}
	}
}
