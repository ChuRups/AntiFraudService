namespace Domain
{
    public class Operation
    {
        public Guid Id { get; set; }
        public Guid IdCustomer { get; set; }
        public decimal Amount { get; set; }
        public DateTime OperationDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
