namespace BusinessLogic.DTOs.DTOsRole
{
    public class RoleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> PermissionsIds { get; set; }
    }
}
