namespace Domain
{
    public class TransactionalOperation
    {
        public Guid Id { get; set; }
        public Guid targetAccountId { get; set; }
        public int IdState { get; set; }
        public int TranferTypeId { get; set; }
        public decimal Value { get; set; }
    }
}
