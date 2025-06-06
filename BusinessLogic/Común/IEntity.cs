namespace BusinessLogic.Dominio
{
    public interface IEntity<TData>
    {
        int Id { get; set; }
        void Validate();
        void Update(TData data);
        
    }
}
