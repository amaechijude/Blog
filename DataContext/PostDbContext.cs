using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Model;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataContext
{
    public class PostDbContext(DbContextOptions<PostDbContext> options) : DbContext(options)
    {
//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);
//             modelBuilder.Entity<Post>()
//                 .HasData(
//                 new Post
//                 {
//                     Id = 1,
//                     Title = "First Post",
//                     Content = "This is the first post",
//                     ImageUrl = "https://www.google.com",
//                     Likes = 0,
//                     CreatedAt = DateTime.Now,
//                     LastUpdatedAt = DateTime.Now
//                 },
//                 new Post
//                 {
//                     Id = 2,
//                     Title = "Second Post",
//                     Content = "This is the second post",
//                     ImageUrl = "https://www.google.com",
//                     Likes = 0,
//                     CreatedAt = DateTime.Now,
//                     LastUpdatedAt = DateTime.Now
//                 },
//                 new Post
//                 {
//                     Id = 3,
//                     Title = "Third Post",
//                     Content = "This is the third post",
//                     ImageUrl = "https://www.google.com",
//                     Likes = 0,
//                     CreatedAt = DateTime.Now,
//                     LastUpdatedAt = DateTime.Now
//                 },
//                 new Post
//                 {
//                     Id = 4,
//                     Title = "Fourth Post",
//                     Content = "This is the fourth post",
//                     ImageUrl = "https://www.google.com",
//                     Likes = 0,
//                     CreatedAt = DateTime.Now,
//                     LastUpdatedAt = DateTime.Now
//                 }
//                 );
//         }
// #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public DbSet<Post> Posts { get; set; }
// #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}