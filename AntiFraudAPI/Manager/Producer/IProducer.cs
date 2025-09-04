namespace Manager.Producer
{
    public interface IProducer
    {
        Task SendAsync(string topic, OperationEvent customerEvent);
    }
}
