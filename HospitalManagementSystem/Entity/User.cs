using System.ComponentModel.DataAnnotations;

namespace Hospital_ManagementSystem_Api.Entity
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
