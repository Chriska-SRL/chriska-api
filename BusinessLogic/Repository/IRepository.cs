namespace BusinessLogic.Repository
{
    public interface IRepository<T>
    {
        T? GetById(int id);
        List<T> GetAll();
        T Add(T entity);
        T Update(T entity);
        T? Delete(int id);

        //De necesitar mas metodos de escritura o obtencion de datos, agregar en la interfaz correspondiente que herede de esta.
    }
}