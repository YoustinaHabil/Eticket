using MvcProject2.Models;

namespace MvcProject2.Repository
{
    public interface ICinemasReposatory
    {
        List<Cinema> GetAll();
        Cinema GetByID(int id);
        void Insert(Cinema cinema);
        void Edit(int id, Cinema cinema);
        void Delete(int id);
    }
}
