namespace Hospital_ManagementSystem_Api.DTOs.UsersDTO
{
    public class AddUserResponseDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
    }
}
