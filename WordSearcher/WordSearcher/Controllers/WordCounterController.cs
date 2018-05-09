using Microsoft.AspNetCore.Mvc;
using System;
using WordSearcher.Repository.Redis.Interfaces;

namespace WordSearcher.Controllers
{
	[Produces("application/json")]
	public class WordCounterController : Controller
	{
		public IRedisRepository RedisRepository { get; set; }

		public WordCounterController(IRedisRepository redisRepository)
		{
			this.RedisRepository = redisRepository;
		}

		[HttpGet("counter/{word}", Name = "Get")]
		public string Get(string word)
		{
			try
			{
				var redisAddress = Environment.GetEnvironmentVariable("RedisAddress");

				using (var redisConnection = this.RedisRepository.GetConnection(redisAddress))
				{
					var count = this.RedisRepository.GetValue(redisConnection, word);

					if (string.IsNullOrWhiteSpace(count)) return "0";
					else return count;
				}
			}
			catch
			{
				throw;
			}
		}

	}
}
