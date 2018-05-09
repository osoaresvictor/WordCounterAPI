using Autofac;
using FileCounterStore.Application.Interfaces;
using System;
using Telemetry.App.IoC;
using WordSearcher.Repository.RabbitMQ.Interfaces;

namespace FileCounterStore
{
	class Program
	{
		public static void Main(string[] args)
		{
			var rabbitMQHost = Environment.GetEnvironmentVariable("RabbitMQHost");
			var rabbitMQPort = int.Parse(Environment.GetEnvironmentVariable("RabbitMQPort"));
			var queueName = Environment.GetEnvironmentVariable("QueueName");

			var iocContainer = new ContainerFactory().Create();

			Console.WriteLine($"{DateTime.Now} Start queue listening...");

			var queueListener = iocContainer.Resolve<IRabbitConsumer>();
			queueListener.EvtReceive += iocContainer.Resolve<IQueueRequestHandler>().OnReceiveFilepathInQueue;
			queueListener.ReceiveMessage(rabbitMQHost, rabbitMQPort, queueName);

			Console.WriteLine("Stop listening...");
		}
	}
}
