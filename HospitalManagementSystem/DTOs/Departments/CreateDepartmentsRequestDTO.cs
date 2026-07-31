namespace Hospital_ManagementSystem_Api.DTOs.Departments
{
    public class CreateDepartmentsRequestDTO
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
