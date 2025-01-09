using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Blog.DataContext
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        // private readonly PasswordHasher<User> _passwordHasher = new();
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     base.OnConfiguring(optionsBuilder);
        //     optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        // }
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);

        //     // add unique constraint to email
        //     modelBuilder.Entity<User>()
        //         .HasIndex(u => u.Email)
        //         .IsUnique();

        //     // add unique constraint to username
        //     modelBuilder.Entity<User>()
        //         .HasIndex(u => u.Username)
        //         .IsUnique();

        //     // establish one to many relationship with posts 
        //     modelBuilder.Entity<User>()
        //         .HasMany(u => u.Posts)
        //         .WithOne(p => p.User)
        //         .HasForeignKey(p => p.UserId)
        //         .OnDelete(DeleteBehavior.Cascade);

        //     // seed data
        //     modelBuilder.Entity<User>()
        //         .HasData(
        //          new User()
        //          {
        //              Id = 1,
        //              Email = "user1@gmail.com",
        //              Username = "user1",
        //              PasswordHash = _passwordHasher.HashPassword(new User(), "password"),
        //              JoinedOn = DateTime.UtcNow
        //          },

        //         new User()
        //         {
        //             Id = 2,
        //             Email = "user2@gmail.com",
        //             Username = "usertwo",
        //             PasswordHash = _passwordHasher.HashPassword(new User(), "password"),
        //             JoinedOn = DateTime.UtcNow
        //         }
        //     ); // End of user seed data

        //     modelBuilder.Entity<Post>()
        //         .HasOne(u => u.User)
        //         .WithMany(p => p.Posts)
        //         .HasForeignKey(p => p.UserId);

        //     modelBuilder.Entity<Post>()
        //         .HasData(
        //             new Post()
        //                 {
        //                     Id = 3,
        //                     Title = "Third Post",
        //                     Content = "This is the third post",
        //                     ImageUrl = "https://www.google.com",
        //                     Likes = 0,
        //                     UserId = 2,
        //                     CreatedAt = DateTime.UtcNow,
        //                     LastUpdatedAt = DateTime.UtcNow
        //                 },
        //                 new Post()
        //                 {
        //                     Id = 4,
        //                     Title = "Fourth Post",
        //                     Content = "This is the fourth post",
        //                     ImageUrl = "https://www.google.com",
        //                     Likes = 0,
        //                     UserId = 2,
        //                     CreatedAt = DateTime.UtcNow,
        //                     LastUpdatedAt = DateTime.UtcNow
        //                 },
        //                 new Post()
        //                 {
        //                     Id = 1,
        //                     Title = "First Post",
        //                     Content = "This is the first post",
        //                     ImageUrl = "https://www.google.com",
        //                     Likes = 0,
        //                     UserId = 1,
        //                     CreatedAt = DateTime.UtcNow,
        //                     LastUpdatedAt = DateTime.UtcNow
        //                 },
        //                 new Post()
        //                 {
        //                     Id = 2,
        //                     Title = "Second Post",
        //                     Content = "This is the second post",
        //                     ImageUrl = "https://www.google.com",
        //                     Likes = 0,
        //                     UserId = 1,
        //                     CreatedAt = DateTime.UtcNow,
        //                     LastUpdatedAt = DateTime.UtcNow
        //                 }

        //         );
            
        // } // End of onModel Creating method

    }
}