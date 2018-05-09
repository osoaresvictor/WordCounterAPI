namespace WordSearcher.Repository.RabbitMQ.Interfaces
{
	public interface IRabbitProducer
	{
		void SendMessage(string rabbitMQHost, int rabbitMQPort, string queueName, string messageToSend);
	}
}