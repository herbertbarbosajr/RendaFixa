namespace FixedIncome.Infrastructure.Interfaces
{
    public interface IMessagePublisher
    {
        void Publish(string queueName, string message);
    }
}