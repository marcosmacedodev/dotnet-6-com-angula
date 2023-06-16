namespace ProEventos.Persistence.Contracts
{
    public interface IRepository
    {
        void Add<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        void DeteleRange<T>(T[] entity) where T: class;
        Task<bool> SaveChangesAsync();
    }
}