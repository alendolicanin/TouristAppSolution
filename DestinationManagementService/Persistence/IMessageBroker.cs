namespace DestinationManagementService.Persistence
{
    public interface IMessageBroker
    {
        void Publish<T>(T message);
    }
}
