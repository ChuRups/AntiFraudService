namespace Manager.Producer
{
    public class OperationEvent
    {
        public OperationEvent(Guid transactionExternalId, DateTime createdAt)
        {
            TransactionExternalId = transactionExternalId;
            CreatedAt = createdAt;
        }

        public Guid TransactionExternalId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
