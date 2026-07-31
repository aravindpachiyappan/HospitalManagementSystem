namespace Hospital_ManagementSystem_Api.DTOs.UsersDTO
{
    public class GetByIdResponseDTO
    {
        public int UsertId { get; set; }
        public string UsertName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //public string Role { get; set; }
    }
}
