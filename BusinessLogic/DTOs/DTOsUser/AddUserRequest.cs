namespace BusinessLogic.DTOs.DTOsUser
{
    public class AddUserRequest
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public bool IsEnabled { get; set; }
        public int RoleId { get; set; }
    }
}
