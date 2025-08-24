namespace KiwiLib.DAL
{
    public interface IRepository<T>
    {
        Task<int> AddAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(T item);
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> SearchAsync(string name);
    }
}
