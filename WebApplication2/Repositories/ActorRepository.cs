using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication2.Models;
using System.Data.Entity;
using WebApplication2.Logger;

namespace WebApplication2.Repositiories
{
    public class ActorRepository : IActorRepository, IDisposable
    {
        private VideoClubContext _context;
        private ILogger _logger;

        public ActorRepository(VideoClubContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Actor> GetActors()
        {
            return _context.Actors.ToList();
        }

        public IEnumerable<Actor> GetActorsWhere(Func<Actor, bool> predicate)
        {
            return _context.Actors.Where(predicate).ToList();
        }

        public Actor GetActorById(int id)
        {
            return _context.Actors.Include(a => a.Country).FirstOrDefault(a => a.ActorId == id);
        }

        public IEnumerable<Actor> FindByIds(IEnumerable<int> ids)
        {
            return _context.Actors.Where(x => ids.Contains(x.ActorId)).ToList();
        }

        public void InsertActor(Actor actor)
        {
            _context.Actors.Add(actor);
        }

        public Actor DeleteActorById(int id)
        {
            Actor actor = _context.Actors.FirstOrDefault(a => a.ActorId == id);
            return _context.Actors.Remove(actor);
        }

        public void Save()
        {
            _context.SaveChanges();
            _logger.Log(DateTime.Now.ToString() + " - [Actor] Save changes to database.");
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