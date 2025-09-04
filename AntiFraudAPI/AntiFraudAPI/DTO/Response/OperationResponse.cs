namespace AntiFraudAPI.DTO.Response
{
    public class OperationResponse
    {
        public Guid TransactionExternalId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
