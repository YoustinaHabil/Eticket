using MvcProject2.Models;

namespace MvcProject2.Repository
{
    public interface IActorReposatory
    {
        List<Actor> GetAll();
        Actor GetByID(int id);
        void Insert(Actor actor);
        void Edit(int id, Actor actor);
        void Delete(int id);

    }
}
