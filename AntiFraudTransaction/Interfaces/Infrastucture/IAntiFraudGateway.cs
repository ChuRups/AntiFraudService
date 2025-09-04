using Domain;

namespace Interfaces.Infrastucture
{
    public interface IAntiFraudGateway
    {
        Task<int> ValidTransaction(TransactionalOperation operation);
    }
}
