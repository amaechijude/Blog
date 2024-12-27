using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Model;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataContext
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Post> Posts { get; set; }
    }
}