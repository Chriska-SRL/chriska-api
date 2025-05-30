namespace BusinessLogic.DTOs.DTOsRole
{
    public class AddRoleRequest
    {
        public string Name { get; set; }
        public List<int> PermissionsIds { get; set; }

    }
}
    