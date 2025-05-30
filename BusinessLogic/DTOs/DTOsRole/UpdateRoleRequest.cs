namespace BusinessLogic.DTOs.DTOsRole
{
    public class UpdateRoleRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> PermissionsIds { get; set; }
    }
}
