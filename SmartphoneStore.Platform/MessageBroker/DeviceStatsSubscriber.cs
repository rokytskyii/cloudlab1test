using Azure.Messaging.ServiceBus;
using SmartphoneStore.Model.MessageBroker;

namespace SmartphoneStore.Platform.MessageBroker;

public class DeviceStatsSubscriber : ISubscriber
{
    private readonly ServiceBusClient _client;
    protected virtual string TopicName => "device-audit-queue";

    private readonly List<string> _data = new List<string>();
    public List<string> Data => _data;
    private ServiceBusProcessor? _processor;

    public DeviceStatsSubscriber(ServiceBusClient client)
    {
        _client = client;
    }

    public async Task SubscribeAsync()
    {
        _processor = _client.CreateProcessor(TopicName);
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        await _processor.StartProcessingAsync();
    }

    private async Task ErrorHandler(ProcessErrorEventArgs arg)
    {
        Console.WriteLine(arg.Exception);
        await Task.CompletedTask;
    }

    private async Task MessageHandler(ProcessMessageEventArgs arg)
    {
        var message = arg.Message.Body.ToString();
        _data.Add(message);

        await arg.CompleteMessageAsync(arg.Message);
    }
}