using Domain;

namespace Interfaces.Repositories
{
    public interface IOperationRepository
    {
        Task<int> GetOperationsSumAsync(Operation operation);
        Task InsertAsync(Operation operation);
    }
}
