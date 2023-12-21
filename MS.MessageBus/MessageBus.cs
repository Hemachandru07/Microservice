using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;


namespace MS.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly string _connectionString = "Endpoint=sb://sbns-msweb.servicebus.windows.net/;SharedAccessKeyName=SharedAccessKey;SharedAccessKey=nYJJ6IOrbdBOfz5ROMAFtfeMabGLJdpCO+ASbO+WF44=;EntityPath=emailshoppingcart";
        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(_connectionString);

            ServiceBusSender sender = client.CreateSender(topic_queue_Name);

            var jsonMessage = JsonConvert.SerializeObject(message);

            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
