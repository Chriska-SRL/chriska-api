using BusinessLogic.Common;

namespace BusinessLogic.Repository
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync(QueryOptions options);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);

        // Si necesitas métodos de escritura/lectura adicionales, agregarlos en las interfaces específicas que hereden de esta.
    }
}
