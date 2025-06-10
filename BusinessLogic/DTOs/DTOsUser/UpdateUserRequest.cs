namespace BusinessLogic.DTOs.DTOsUser
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public bool IsEnabled { get; set; }
        public bool needsPasswordChange { get; set; }
        public int RoleId { get; set; }
    }
}
