namespace BusinessLogic.Común
{
    public interface IAuditable
    {
        // Auditoría de creación
        DateTime CreatedAt { get; set; }
        string? CreatedBy { get; set; }
        string? CreatedLocation { get; set; }

        // Auditoría de actualización
        DateTime? UpdatedAt { get; set; }
        string? UpdatedBy { get; set; }
        string? UpdatedLocation { get; set; }

        // Auditoría de borrado lógico (soft delete)
        DateTime? DeletedAt { get; set; }
        string? DeletedBy { get; set; }
        string? DeletedLocation { get; set; }
    }
}
