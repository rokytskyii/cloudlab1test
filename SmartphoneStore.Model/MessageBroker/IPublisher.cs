namespace SmartphoneStore.Model.MessageBroker;

public interface IPublisher
{
    Task PublishAsync(string message);
}