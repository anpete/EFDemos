using System;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demos
{
    internal class Program
    {
        private static void Main()
        {
            SetupDatabase();

            using (var db = new BloggingContext())
            {
                var blog = new Blog { Name = "Rowan's Blog" };
                
                blog.SetUrl("http://romiller.com");

                db.Blogs.Add(blog);
                db.SaveChanges();

                var blogs = db.Blogs
                    .OrderBy(b => EF.Property<string>(b, "Url"))
                    .ToList();

                foreach (var b in blogs)
                {
                    Console.WriteLine(b.Name);
                }
            }
        }

        private static void SetupDatabase()
        {
            using (var db = new BloggingContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.FlexibleMapping;Trusted_Connection=True;")
                .UseLoggerFactory(new LoggerFactory().AddConsole());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .Property<string>("Url")
                .HasField("_url");
        }
    }

    public class Blog
    {
        private string _url;

        public int BlogId { get; set; }
        public string Name { get; set; }

        public string GetUrl()
        {
            return _url;
        }

        public void SetUrl(string url)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
            }

            _url = url;
        }
    }
}