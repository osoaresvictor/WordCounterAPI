using System;

namespace WordSearcher.Repository.RabbitMQ.Interfaces
{
	public interface IRabbitConsumer
	{
		event EventHandler EvtReceive;

		void ReceiveMessage(string rabbitMQHost, int rabbitMQPort, string queueName);
	}
}