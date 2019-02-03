using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication2.Models;
using System.Data.Entity;

namespace WebApplication2.Repositories
{
    public class ActorRepository : IRepository<Actor>, IDisposable
    {
        private IVideoClubContext _context;

        public ActorRepository(IVideoClubContext context)
        {
            _context = context;
        }

        public IEnumerable<Actor> GetAll()
        {
            return _context.Actors.ToList();
        }

        public IEnumerable<Actor> GetWhere(Func<Actor, bool> predicate)
        {
            return _context.Actors.Where(predicate).ToList();
        }

        public Actor Get(int id)
        {
            return _context.Actors.Include(a => a.Country).FirstOrDefault(a => a.ActorId == id);
        }

        public IEnumerable<Actor> FindByIds(IEnumerable<int> ids)
        {
            return _context.Actors.Where(x => ids.Contains(x.ActorId)).ToList();
        }

        public void Insert(Actor actor)
        {
            _context.Actors.Add(actor);
        }

        public Actor Delete(int id)
        {
            Actor actor = _context.Actors.FirstOrDefault(a => a.ActorId == id);
            return _context.Actors.Remove(actor);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;
        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}