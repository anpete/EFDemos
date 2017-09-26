using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace data_seeding
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Theme> Themes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.DataSeeding;Trusted_Connection=True;ConnectRetryCount=0")
                .UseLoggerFactory(new LoggerFactory().AddConsole());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Theme>().SeedData(
                new Theme { ThemeId = 1, Name = "MSDN", TitleColor = Color.DarkGray.ToArgb() },
                new Theme { ThemeId = 2, Name = "TechNet", TitleColor = Color.DarkCyan.ToArgb() }, 
                new Theme { ThemeId = 3, Name = "Personal", TitleColor = Color.LightBlue.ToArgb() });
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
            public int TitleColor { get; set; }
        }
    }
}
