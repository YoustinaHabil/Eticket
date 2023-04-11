using MvcProject2.Migrations;
using MvcProject2.Models;

namespace MvcProject2.Repository
{
    public class ActorsReprosatory:IActorReposatory
    {
        // IEntityBase context;
        private readonly AppDbContext _context;

        public ActorsReprosatory(AppDbContext context)
        {
            _context = context;

            //context = new IEntityBase();
        }
        public List<Actor> GetAll()
        {
            return _context.Actors.ToList();

        }
        public Actor GetByID(int id)
        {
            return _context.Actors.FirstOrDefault(e => e.Id == id);

        }
        public void Insert(Actor actor)
        {
            _context.Actors.Add(actor);
            _context.SaveChanges();
        }
        public void Edit(int id, Actor actor)
        {
            Actor oldActor = GetByID(id);
            oldActor.Id = actor.Id;
            oldActor.FullName = actor.FullName;
            oldActor.Bio = actor.Bio;
            if (actor.ProfilePictureURL == null)
            {
                oldActor.ProfilePictureURL = oldActor.ProfilePictureURL;
            }
            else
            { oldActor.ProfilePictureURL = actor.ProfilePictureURL; }
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            Actor act = GetByID(id);
            _context.Actors.Remove(act);
            _context.SaveChanges();
        }
    }
}
