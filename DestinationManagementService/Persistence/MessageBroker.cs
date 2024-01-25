using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace DestinationManagementService.Persistence
{
    public class MessageBroker : IMessageBroker
    {
        private readonly IConfiguration configuration;

        public MessageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Publish<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration.GetSection("MessageBroker:HostName").Value,
                Port = int.Parse(configuration.GetSection("MessageBroker:Port").Value),
                UserName = configuration.GetSection("MessageBroker:Username").Value,
                Password = configuration.GetSection("MessageBroker:Password").Value
            };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "destinations", durable: false, exclusive: false, autoDelete: false, arguments: null);
            
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            
            channel.BasicPublish(exchange: "", routingKey: "destinations", basicProperties: null, body: body);
        }
    }
}
