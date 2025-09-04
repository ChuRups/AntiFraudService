using Domain;

namespace Interfaces.Managers
{
    public interface IOperationManager
    {
        Task<Operation> CreateOperation(Operation operation);
    }
}
