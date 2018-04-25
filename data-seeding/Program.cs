using System;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace Demos
{
    public class Program
    {
        private static void Main()
        {
            using (var db = new BloggingContext())
            {
                foreach (var theme in db.Themes)
                {
                    Console.WriteLine(
                        $"Id = {theme.ThemeId}, Name = {theme.Name}, Color = {theme.TitleColor}");
                }
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string BlogUrl { get; set; }
        public Theme Theme { get; set; }
    }

    public class Theme
    {
        public int ThemeId { get; set; }
        public string Name { get; set; }
        public string TitleColor { get; set; }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Theme> Themes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.DataSeeding;Trusted_Connection=True;ConnectRetryCount=0");
    }
}
