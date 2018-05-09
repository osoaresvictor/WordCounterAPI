using FileCounterStore.Application.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using WordSearcher.Repository.Redis.Interfaces;

namespace FileCounterStore.Application
{
	public class QueueRequestHandler : IQueueRequestHandler
	{
		public IFileHandler FileHandler;
		public IRedisRepository RedisRepository;

		public QueueRequestHandler(IFileHandler fileHandler, IRedisRepository redisRepository)
		{
			this.FileHandler = fileHandler;
			this.RedisRepository = redisRepository;
		}

		public void OnReceiveFilepathInQueue(object sender, EventArgs e)
		{
			var filepath = sender.ToString();
			if (this.FileHandler.IsCSVFile(filepath) == false)
			{
				Console.WriteLine("Invalid File!");
				return;
			}

			Console.WriteLine($" {DateTime.Now} - Processing: {sender.ToString()}");

			using (var redisConnection = this.RedisRepository.GetConnection(Environment.GetEnvironmentVariable("RedisAddress")))
			{
				var fileContent = this.FileHandler.ReadFileContent(filepath);
				this.StoreValuesInDatabase(redisConnection, fileContent);
			}
		}

		private void StoreValuesInDatabase(ConnectionMultiplexer redisConnection, List<string> lines)
		{
			foreach (var keyword in lines)
			{
				if (this.RedisRepository.GetValue(redisConnection, keyword) == default(RedisValue))
					this.RedisRepository.SetValue(redisConnection, keyword, "1");
				else
					this.RedisRepository.IncrementValue(redisConnection, keyword, 1.0);
			}
		}
	}
}
