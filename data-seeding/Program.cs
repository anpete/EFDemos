using System;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Demos
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BloggingContext())
            {
                foreach(var theme in db.Themes)
                {
                    Console.WriteLine($"Id = {theme.ThemeId}, Name = {theme.Name}, Color = {theme.TitleColor}");
                }
            }
        }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Theme> Themes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.DataSeeding;Trusted_Connection=True;ConnectRetryCount=0");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Theme>().SeedData(
                new Theme { ThemeId = 1, Name = "MSDN", TitleColor = Color.Magenta.Name },
                new Theme { ThemeId = 2, Name = "TechNet", TitleColor = Color.DarkCyan.Name },
                new Theme { ThemeId = 3, Name = "Personal", TitleColor = Color.LightBlue.Name });
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
    }
}
