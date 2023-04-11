using MvcProject2.Migrations;
using MvcProject2.Models;

namespace MvcProject2.Repository
{
    public class CinemasReposatory:ICinemasReposatory
    {
        private readonly AppDbContext _context;

        public CinemasReposatory (AppDbContext context)
        {
            _context = context;

        }
        public List<Cinema> GetAll()
        {
            return _context.Cinemas.ToList();
        }
        public Cinema GetByID(int id)
        {
            return _context.Cinemas.FirstOrDefault(e => e.Id == id);

        }
        public void Insert(Cinema cinema)
        {
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
        }
        public void Edit(int id, Cinema cinema)
        {
            Cinema oldActor = GetByID(id);
            oldActor.Id = cinema.Id;
            oldActor.Name = cinema.Name;
            oldActor.Description = cinema.Description;
            if (cinema.Logo == null)
            {
                oldActor.Logo = oldActor.Logo;
            }
            else
            { oldActor.Logo = cinema.Logo; }
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            Cinema cinema = GetByID(id);
            _context.Cinemas.Remove(cinema);
            _context.SaveChanges();
        }
    }
}
