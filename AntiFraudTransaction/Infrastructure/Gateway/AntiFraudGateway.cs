using Domain;
using Domain.Enums;
using Infrastructure.DTO;
using Interfaces.Infrastucture;
using System.Net.Http.Json;

namespace Infrastructure.Gateway
{
    public class AntiFraudGateway : IAntiFraudGateway
    {
        private readonly HttpClient _httpClient;

        public AntiFraudGateway(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:44360/");
        }

        public async Task<int> ValidTransaction(TransactionalOperation operation)
        {
            var request = new OperationRequest
            {
                TranferTypeId = operation.TranferTypeId,
                TargetAccountId = operation.targetAccountId,
                Value = operation.Value
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/operations", request);

                response.EnsureSuccessStatusCode();
                return (int)TransactionStatuses.Approved;
            }
            catch (Exception)
            {
                return (int)TransactionStatuses.Rejected;
            }
        }

    }
}
