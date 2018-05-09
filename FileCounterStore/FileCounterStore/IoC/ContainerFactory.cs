using Autofac;
using FileCounterStore.Application;
using FileCounterStore.Application.Interfaces;
using WordSearcher.Repository.RabbitMQ;
using WordSearcher.Repository.RabbitMQ.Interfaces;
using WordSearcher.Repository.Redis;
using WordSearcher.Repository.Redis.Interfaces;

namespace Telemetry.App.IoC
{
	public class ContainerFactory
	{
		public IContainer Create()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<FileHandler>().As<IFileHandler>();
			builder.RegisterType<QueueRequestHandler>().As<IQueueRequestHandler>();
			builder.RegisterType<RabbitConsumer>().As<IRabbitConsumer>();
			builder.RegisterType<RabbitProducer>().As<IRabbitProducer>();
			builder.RegisterType<RedisRepository>().As<IRedisRepository>();

			return builder.Build();
		}
	}
}