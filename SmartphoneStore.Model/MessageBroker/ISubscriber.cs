namespace SmartphoneStore.Model.MessageBroker;

public interface ISubscriber
{
    Task SubscribeAsync();
    List<string> Data { get; }
}