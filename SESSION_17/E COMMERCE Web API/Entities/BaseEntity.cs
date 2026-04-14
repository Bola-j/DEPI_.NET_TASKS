namespace E_COMMERCE_Web_API.Entities
{
    public abstract class BaseEntity
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; } // as a user id reference is now int due to the nature of the User entity in this project
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; } // as a user id reference is now int due to the nature of the User entity in this project
    }
}
