namespace Hospital_ManagementSystem_Api.DTOs.Departments
{
    public class GetAllListDepartmentsRequestListDTO
    {
        public string SearchString { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
