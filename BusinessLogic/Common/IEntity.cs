namespace BusinessLogic.Domain
{
    public interface IEntity<TData>
    {
        int Id { get; set; }
        void Validate();
        public class UpdatableData;
        void Update(TData data);
        
    }
}
