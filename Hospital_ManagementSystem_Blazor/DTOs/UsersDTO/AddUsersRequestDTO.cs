namespace Hospital_ManagementSystem_Blazor.DTOs.UsersDTO
{
    public class AddUsersRequestDTO
    {
        public int UsertId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;        
    }
}
