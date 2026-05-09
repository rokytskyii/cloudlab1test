using Azure.Messaging.ServiceBus;
using SmartphoneStore.Model.MessageBroker;

namespace SmartphoneStore.Platform.MessageBroker;

public class DeviceStatsPublisher : IPublisher
{
    private readonly ServiceBusClient _client;
    protected virtual string TopicName => "device-audit-queue";
    private readonly ServiceBusSender _publisher;

    public DeviceStatsPublisher(ServiceBusClient client)
    {
        _client = client;
        _publisher = _client.CreateSender(TopicName);
    }

    public async Task PublishAsync(string message)
    {
        await _publisher.SendMessageAsync(new ServiceBusMessage(message));
    }
}