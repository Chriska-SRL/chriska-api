namespace BusinessLogic.Repository
{
    public interface IRepository<T>
    {
        T? GetById(int id);
        List<T> GetAll();
        List<T> GetAll(Dictionary<string, string>? filters);
        T Add(T entity);
        T Update(T entity);
        T? Delete(int id);
        T? Delete(T entity);

        //De necesitar mas metodos de escritura o obtencion de datos, agregar en la interfaz correspondiente que herede de esta.
    }
}