using Domain;

namespace Interfaces.Manager
{
    public interface ITransactionalOperationManager
    {
        Task<TransactionalOperation> CreateTransactionalOperation(TransactionalOperation operation);
    }
}
