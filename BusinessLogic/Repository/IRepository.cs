namespace BusinessLogic.Repository
{
    public interface IRepository<T>
    {
        T GetById(int id);
        List<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);

        //De necesitar mas metodos de escritura o obtencion de datos, agregar en la interfaz correspondiente que herede de esta.
    }
}