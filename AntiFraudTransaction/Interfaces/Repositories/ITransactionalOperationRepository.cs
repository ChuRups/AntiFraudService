using Domain;

namespace Interfaces.Repositories
{
    public interface ITransactionalOperationRepository
    {
        Task CreateTransactionalOperation(TransactionalOperation operation);
        Task UpdateTransactionalOperation(TransactionalOperation operation);
    }
}
