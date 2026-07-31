namespace Hospital_ManagementSystem_Api.DTOs.Departments
{
    public class UpdateDepartmentsResponseDTO
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
