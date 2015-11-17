using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace AzureConstructionsProgressTracker.Common
{
    internal class ServiceBusManager
    {
        private readonly string _serviceBusNamespaceConnectionString;
        private string QueueName = "resizepicturesqueue";

        public ServiceBusManager(string serviceBusNamespaceConnectionString)
        {
            _serviceBusNamespaceConnectionString = serviceBusNamespaceConnectionString;
        }


        public void CreateQueue()
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(_serviceBusNamespaceConnectionString);

            //TODO ex3: Create queue if not exists
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }
        }

        public async Task Enqueue(object message)
        {
            var brokeredMessage = new BrokeredMessage(message);

            var topicClient = QueueClient.CreateFromConnectionString(_serviceBusNamespaceConnectionString, QueueName);

            //TODO ex3: Send message to Queue
            topicClient.Send(brokeredMessage);
        }
    }
}