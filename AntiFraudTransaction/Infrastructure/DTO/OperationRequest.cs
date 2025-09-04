namespace Infrastructure.DTO
{
    public class OperationRequest
    {
        public Guid TargetAccountId { get; set; }
        public int TranferTypeId { get; set; }
        public decimal Value { get; set; }
    }
}
