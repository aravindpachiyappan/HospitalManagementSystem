namespace Hospital_ManagementSystem_Blazor.DTOs.UsersDTO
{
    public class UserListRequestDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
    }
}
