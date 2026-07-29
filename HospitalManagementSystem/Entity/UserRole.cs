using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_ManagementSystem_Api.Entity
{
    public class UserRole :BaseEntity
    {
        [Key]
        public int? UserRoleId { get; set; }
        public int? RoleId { get; set; }
        public int? UserId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }

        [ForeignKey(nameof (UserId))]
        public User? User { get; set; }
    }
}
