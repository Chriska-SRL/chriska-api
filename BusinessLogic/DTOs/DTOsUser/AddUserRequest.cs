namespace BusinessLogic.DTOs.DTOsUser
{
    public class AddUserRequest
    {
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool IsEnabled { get; set; }
        public int RoleId { get; set; }
    }
}
