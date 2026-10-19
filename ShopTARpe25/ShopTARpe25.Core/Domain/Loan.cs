namespace ShopTARpe25.Core.Domain
{
    public class Loan
    {
        public Guid id { get; set; }
        public int BookId { get; set; }
        public int ReaderId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
