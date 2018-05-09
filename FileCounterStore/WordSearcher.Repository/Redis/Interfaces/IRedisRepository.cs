using StackExchange.Redis;

namespace WordSearcher.Repository.Redis.Interfaces
{
	public interface IRedisRepository
	{
		ConnectionMultiplexer GetConnection(string host);
		string GetValue(ConnectionMultiplexer redisConnection, string key);
		bool SetValue(ConnectionMultiplexer redisConnection, string key, string value);
		void IncrementValue(ConnectionMultiplexer redisConnection, string key, double increment);
	}
}