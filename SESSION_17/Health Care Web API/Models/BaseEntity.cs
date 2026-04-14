namespace Health_Care_Web_API.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; } // as a user id reference is now int due to the nature of the User entity in this project
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; } // as a user id reference is now int due to the nature of the User entity in this project
    }
}
