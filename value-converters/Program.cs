using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using Microsoft.Extensions.Logging;

namespace Demos
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BloggingContext())
            {
                foreach(var count in db.Themes
                    .GroupBy(t => t.TitleColor)
                    .Select(g => new { Color = g.Key, Count = g.Count() }))
                {
                    Console.WriteLine($"Color = {count.Color}, Count = {count.Count}");
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
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.ValueConverters;Trusted_Connection=True;ConnectRetryCount=0")
                .UseLoggerFactory(
                    new LoggerFactory().AddConsole(
                        (s, l) =>
                            l == LogLevel.Information && !s.EndsWith("Connection")));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Theme>()
                .SeedData(
                    new Theme { ThemeId = 1, Name = "MSDN", TitleColor = Color.Red.Name },
                    new Theme { ThemeId = 2, Name = "TechNet", TitleColor = Color.Red.Name },
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
            public uint ThemeId { get; set; }
            public string Name { get; set; }
            public string TitleColor { get; set; }
        }
    }
}
