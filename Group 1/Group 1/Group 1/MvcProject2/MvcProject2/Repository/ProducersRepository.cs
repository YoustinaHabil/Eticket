using MvcProject2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace MvcProject2.Repository
{
    public class ProducersRepository : IProducersRepository
    {
        private readonly AppDbContext _context;
        public ProducersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producer>> GetAllAsync()
        {
            return _context.Producers.ToList();
        }

        public async Task<Producer> GetByIdAsync(int id)
        {
            return await _context.Producers.FirstOrDefaultAsync(n => n.Id == id);
        }
        public async Task AddAsync(Producer p)
        {
            await _context.Set<Producer>().AddAsync(p);
            await _context.SaveChangesAsync();
        }
        public void Add(Producer p)
        {
            _context.Producers.Add(p);
             _context.SaveChanges();
        }

        public void Update(int id, Producer  newp)
        {
            Producer oldp = _context.Producers.FirstOrDefault(n=>n.Id==id);
            oldp.Id = newp.Id;
            oldp.FullName = newp.FullName;
            oldp.Bio = newp.Bio;
            if (newp.ProfilePictureURL == null)
            {
                oldp.ProfilePictureURL = oldp.ProfilePictureURL;
            }
            else
            { oldp.ProfilePictureURL = newp.ProfilePictureURL; }
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            
               Producer p = _context.Producers.FirstOrDefault(n=>n.Id==id);
                _context.Producers.Remove(p);
                _context.SaveChanges();
            }
        }
     


    }

