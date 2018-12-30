using System;
using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2.Repositiories
{
    interface IActorRepository
    {
        IEnumerable<Actor> GetActors();
        IEnumerable<Actor> GetActorsWhere(Func<Actor, bool> predicate);
        Actor GetActorById(int id);
        void InsertActor(Actor actor);
        Actor DeleteActorById(int id);
        void Save();
    }
}
