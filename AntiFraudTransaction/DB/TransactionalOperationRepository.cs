using Dapper;
using Domain;
using Domain.Enums;
using Interfaces.Repositories;
using System.Data;

namespace DB
{
    public class TransactionalOperationRepository : ITransactionalOperationRepository
    {
        private readonly IDbConnection _connection;

        public TransactionalOperationRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task CreateTransactionalOperation(TransactionalOperation operation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", operation.Id);
            parameters.Add("@id_state", (int)TransactionStatuses.Pending);

            await _connection.ExecuteAsync(
                "usp_transaction_op_insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateTransactionalOperation(TransactionalOperation operation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", operation.Id);
            parameters.Add("@id_state", operation.IdState);

            await _connection.ExecuteAsync(
                "usp_transaction_op_update",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

