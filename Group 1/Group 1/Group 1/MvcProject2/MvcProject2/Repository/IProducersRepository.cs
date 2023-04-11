using MvcProject2.Models;

namespace MvcProject2.Repository
{
    public interface IProducersRepository
    {
        Task AddAsync(Producer p);
        //Task DeleteAsync(int id);
        Task<List<Producer>> GetAllAsync();
        Task<Producer> GetByIdAsync(int id);
        //Task UpdateAsync(int id, Producer entity);
        void Delete(int id);
        void Add(Producer p);
        void Update(int id, Producer p);

    }
}