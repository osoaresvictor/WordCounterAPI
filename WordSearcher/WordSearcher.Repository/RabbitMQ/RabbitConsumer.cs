using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using WordSearcher.Repository.RabbitMQ.Interfaces;

namespace WordSearcher.Repository.RabbitMQ
{
	public class RabbitConsumer : IRabbitConsumer
	{
		public event EventHandler EvtReceive;

		public void ReceiveMessage(string rabbitMQHost, int rabbitMQPort, string queueName)
		{
			var factory = new ConnectionFactory() { Endpoint = new AmqpTcpEndpoint(rabbitMQHost, rabbitMQPort) };

			using (var autoResetEvent = new AutoResetEvent(false))
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: queueName,
									 durable: false,
									 exclusive: false,
									 autoDelete: false,
									 arguments: null);

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, eventArgs) =>
				{
					var body = eventArgs.Body;
					var receivedMessage = Encoding.UTF8.GetString(body);
					Console.WriteLine($" {DateTime.Now} - Received: {receivedMessage}");

					this.EvtReceive.Invoke(receivedMessage, EventArgs.Empty);
				};

				channel.BasicConsume(queue: queueName,
									 autoAck: true,
									 consumer: consumer);

				autoResetEvent.WaitOne();
			}
		}
	}
}
