namespace Hospital_ManagementSystem_Api.DTOs.UsersDTO
{
    public class UpdateUserResponseDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
