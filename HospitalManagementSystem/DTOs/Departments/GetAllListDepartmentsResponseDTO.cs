namespace Hospital_ManagementSystem_Api.DTOs.Departments
{
    public class GetAllListDepartmentsResponseDTO
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
    public class PaginatedDepartmentListResponseDTO
    {
        public List<GetAllListDepartmentsResponseDTO> Departments { get; set; } = new();

        public int TotalRecords { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }
    }
}
