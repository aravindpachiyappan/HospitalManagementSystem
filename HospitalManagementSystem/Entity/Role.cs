using System.ComponentModel.DataAnnotations;

namespace Hospital_ManagementSystem_Api.Entity
{
    public class Role : BaseEntity
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
