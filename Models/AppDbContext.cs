

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyMoviesBackend.Models
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Relacija 1:N - 1 User, više UsersMovies

            builder.Entity<UsersMovies>()
                .HasOne(b => b.User)
                .WithMany(ba => ba.UsersMovies)
                .HasForeignKey(bi => bi.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //Relacija 1:N - 1 User, više lista 

            builder.Entity<UsersLists>()
                .HasOne(b => b.User)
                .WithMany(ba => ba.List)
                .HasForeignKey(bi => bi.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //Relacija N:N - u više lista, više filmova 

            builder.Entity<UsersLists_Movies>()
                .HasOne(b => b.List)
                .WithMany(ba => ba.UsersLists_Movies)
                .HasForeignKey(bi => bi.ListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UsersLists_Movies>()
                .HasOne(b => b.Movies)
                .WithMany(ba => ba.UsersLists_Movies)
                .HasForeignKey(bi => bi.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            //Relacija N:N - više filmova, od više usera, u favoritima 

            builder.Entity<Favorite>()
                .HasOne(b => b.User)
                .WithMany(ba => ba.Favorites)
                .HasForeignKey(bi => bi.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Favorite>()
                .HasOne(b => b.Movies)
                .WithMany(ba => ba.Favorites)
                .HasForeignKey(bi => bi.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<AppUser> DbUsers { get; set; }
        public DbSet<Favorite> DbFavorite { get; set; }
        public DbSet<Movies> DbMovies { get; set; }
        public DbSet<UsersLists> DbUsersLists { get; set; }
        public DbSet<UsersLists_Movies> DbUsersLists_Movies { get; set; }
        public DbSet<UsersMovies> DbUsersMovies { get; set; }

    }
}
