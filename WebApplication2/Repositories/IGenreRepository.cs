﻿using System;
using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2.Repositiories
{
    interface IGenreRepository
    {
        IEnumerable<Genre> GetGenres();
        IEnumerable<Genre> GetGenresWhere(Func<Genre, bool> predicate);
        void InsertGenre(Genre genre);
        Genre DeleteGenreById(int id);
        void Save();
    }
}