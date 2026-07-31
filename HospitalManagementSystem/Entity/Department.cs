using System.ComponentModel.DataAnnotations;

namespace Hospital_ManagementSystem_Api.Entity
{
    public class Department : BaseEntity
    {
        [Key]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
