using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace EFCore101
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BloggingContext())
            {
                // Create the database...

                db.Database.EnsureCreated();

                // Add some data...

                var blog = new Blog { Name = "The Dog Blog", Url = "http://sample.com/dogs" };

                db.Blogs.Add(blog);

                db.SaveChanges();

                // Query the data...

                var blogs = db.Blogs.OrderBy(b => b.Name).ToList();

                foreach (var item in blogs)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.EFCore101;Trusted_Connection=True;")
                .UseLoggerFactory(new LoggerFactory().AddConsole());
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
