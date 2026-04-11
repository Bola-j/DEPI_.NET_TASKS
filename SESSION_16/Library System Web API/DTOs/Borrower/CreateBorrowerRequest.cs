namespace Library_System_Web_API.DTOs.Borrower
{
    public class CreateBorrowerRequest
    {
        public string? Name { get; set; }   
        public DateOnly? MembershipDate { get; set; }
    }
}
