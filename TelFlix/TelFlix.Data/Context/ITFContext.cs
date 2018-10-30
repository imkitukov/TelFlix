using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TelFlix.Data.Models;

namespace TelFlix.Data.Context
{
    public interface ITFContext
    {
        int SaveChanges();

        EntityEntry<T> Entry<T>(T entity) where T : class;

        DbSet<T> Set<T>() where T : class;

        DbSet<Movie> Movies { get; set; }
        DbSet<Actor> Actors { get; set; }
        DbSet<Director> Directors { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<MoviesActors> MoviesActors { get; set; }
        DbSet<MoviesGenres> MoviesGenres { get; set; }
        DbSet<MoviesDirectors> MoviesDirectors { get; set; }
        DbSet<MoviesUsers> MoviesUsers { get; set; }
    }
}
