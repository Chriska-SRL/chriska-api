namespace BusinessLogic.DTOs.DTOsShelve
{
    public class UpdateShelveRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WarehouseId { get; set; }
    }
}
