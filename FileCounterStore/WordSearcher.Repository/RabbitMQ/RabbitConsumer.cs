using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
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
			var conncetionFactory = new ConnectionFactory() { Endpoint = new AmqpTcpEndpoint(rabbitMQHost, rabbitMQPort) };
			var connection = default(IConnection);

			while (connection == null)
			{
				try
				{
					Console.WriteLine($"Connecting to {rabbitMQHost}:{rabbitMQPort}");
					connection = conncetionFactory.CreateConnection();
				}
				catch (BrokerUnreachableException ex)
				{
					Console.WriteLine($"Error: {ex.Message} - Reconnect in 5s");
					Thread.Sleep(5000);
				}
			}

			Console.WriteLine($"Connection established!");

			using (var autoResetEvent = new AutoResetEvent(false))
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

			connection.Dispose();
		}
	}
}
