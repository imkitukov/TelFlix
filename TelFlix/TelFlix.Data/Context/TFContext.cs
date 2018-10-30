using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TelFlix.Data.Models;
using TelFlix.Data.Models.Contracts;

namespace TelFlix.Data.Context
{
    public class TFContext : IdentityDbContext<User>//, ITFContext
    {
        private DbContextOptions options;

        public TFContext()
        {
        }

        public TFContext(DbContextOptions options)
            : base(options)
        {
            this.options = options;
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MoviesDirectors> MoviesDirectors { get; set; }
        public DbSet<MoviesUsers> MoviesUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder
            //        .UseSqlServer(@"Server=tcp:telflix.database.windows.net,1433;Initial Catalog=TelFlixDB;Persist Security Info=False;User ID=TelFlixAdmin;Password=MasterNinja!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure default schema
            modelBuilder.HasDefaultSchema("Admin");

            // Map entities to tables
            //modelBuilder
            //    .Entity<Actor>()
            //    .ToTable("Actors");

            //modelBuilder
            //    .Entity<Director>()
            //    .ToTable("Directors");

            //modelBuilder
            //    .Entity<Genre>()
            //    .ToTable("Genres");

            //modelBuilder
            //    .Entity<Movie>()
            //    .ToTable("Movies");

            //modelBuilder
            //    .Entity<Review>()
            //    .ToTable("Reviews");

            //modelBuilder
            //    .Entity<User>()
            //    .ToTable("Users");

            // Configure composite primary keys for the many-to-many tables
            modelBuilder.Entity<MoviesActors>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            modelBuilder.Entity<MoviesGenres>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            modelBuilder.Entity<MoviesDirectors>()
                .HasKey(mp => new { mp.MovieId, mp.DirectorId });

            modelBuilder.Entity<MoviesUsers>()
                .HasKey(mu => new { mu.MovieId, mu.UserId });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            var newlyCreatedEntities = this.ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditable && ((e.State == EntityState.Added) || (e.State == EntityState.Modified)));

            foreach (var entry in newlyCreatedEntities)
            {
                var entity = (IAuditable)entry.Entity;

                if (entry.State == EntityState.Added && entity.CreatedOn == null)
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }
    }
}
