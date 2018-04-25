using System;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

    public class BloggingContext : DbContext
    {
        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory()
            .AddConsole((s, l) => l == LogLevel.Information && s.EndsWith("Command"));

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Theme> Themes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Theme>()
                .HasData(
                    new Theme { ThemeId = 1, Name = "MSDN", TitleColor = Color.AliceBlue.Name },
                    new Theme { ThemeId = 2, Name = "TechNet", TitleColor = Color.DarkCyan.Name },
                    new Theme { ThemeId = 3, Name = "EF", TitleColor = Color.Purple.Name },
                    new Theme { ThemeId = 4, Name = "Personal", TitleColor = Color.LightBlue.Name });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.DataSeeding;Trusted_Connection=True;ConnectRetryCount=0")
                .UseLoggerFactory(_loggerFactory);
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string BlogUrl { get; set; }
        public Theme Theme { get; set; }
    }

    public class Theme
    {
        public ushort ThemeId { get; set; }
        public string Name { get; set; }
        public string TitleColor { get; set; }
    }
}
