using Domain;
using Interfaces.Infrastucture;
using Interfaces.Manager;
using Interfaces.Repositories;

namespace Manager
{
    public class TransactionalOperationManager : ITransactionalOperationManager
    {
        private readonly ITransactionalOperationRepository _tansactionalOperationRepository;
        private readonly IAntiFraudGateway _antiFraudGateway;

        public TransactionalOperationManager(
            ITransactionalOperationRepository tansactionalOperationRepository,
            IAntiFraudGateway antiFraudGateway)
        {
            _tansactionalOperationRepository = tansactionalOperationRepository;
            _antiFraudGateway = antiFraudGateway;
        }

        public async Task<TransactionalOperation> CreateTransactionalOperation(TransactionalOperation operation)
        {
            await _tansactionalOperationRepository.CreateTransactionalOperation(operation);

            var status = await _antiFraudGateway.ValidTransaction(operation);
            operation.IdState = status;

            await _tansactionalOperationRepository.UpdateTransactionalOperation(operation);

            return operation;
        }
    }
}
