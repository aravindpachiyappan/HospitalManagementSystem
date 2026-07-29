namespace Hospital_ManagementSystem_Api.DTOs.AuthDTO
{
    public class LoginResposeDTO
    {
        public string Token { get; set; } = string.Empty;
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
