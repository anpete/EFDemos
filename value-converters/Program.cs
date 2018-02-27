using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demos
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BloggingContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.AddRange(
                    new Theme { Name = "MSDN", TitleColor = Color.Red },
                    new Theme { Name = "TechNet", TitleColor = Color.Red },
                    new Theme { Name = "Personal", TitleColor = Color.LightBlue });

                db.SaveChanges();
            }

            Console.Read();

            using (var db = new BloggingContext())
            {

            }

            Console.Read();
        }
    }

    public class BloggingContext : DbContext
    {
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

        }
    }

    public class Theme
    {
        public uint ThemeId { get; set; }
        public string Name { get; set; }
        public Color TitleColor { get; set; }
    }

    public enum Color
    {
        Red,
        LightBlue
    }
}
