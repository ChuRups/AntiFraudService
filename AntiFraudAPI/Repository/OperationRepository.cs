using Dapper;
using Domain;
using Interfaces.Repositories;
using System.Data;

namespace Repository
{
    public class OperationRepository : IOperationRepository
    {
        private readonly IDbConnection _connection;

        public OperationRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> GetOperationsSumAsync(Operation operation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id_customer", operation.IdCustomer);
            parameters.Add("@day", operation.OperationDate);

            return await _connection.ExecuteScalarAsync<int>(
                "usp_operation_sum_by_customer_and_day",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task InsertAsync(Operation operation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", operation.Id);
            parameters.Add("@id_customer", operation.IdCustomer);
            parameters.Add("@amount", operation.Amount);
            parameters.Add("@operation_date", operation.OperationDate);

            await _connection.ExecuteScalarAsync<Guid>(
                "usp_operation_insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
