using RabbitMQ.Client;
using System;
using System.Text;
using WordSearcher.Repository.RabbitMQ.Interfaces;

namespace WordSearcher.Repository.RabbitMQ
{
	public class RabbitProducer : IRabbitProducer
	{
		public void SendMessage(string rabbitMQHost, int rabbitMQPort, string queueName, string messageToSend)
		{
			var factory = new ConnectionFactory() { Endpoint = new AmqpTcpEndpoint(rabbitMQHost, rabbitMQPort) };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: queueName,
									 durable: false,
									 exclusive: false,
									 autoDelete: false,
									 arguments: null);

				var body = Encoding.UTF8.GetBytes(messageToSend);

				channel.BasicPublish(exchange: "",
									 routingKey: queueName,
									 basicProperties: null,
									 body: body);
				Console.WriteLine($" {DateTime.Now} - Sent Message: {messageToSend}");
			}
		}
	}
}
