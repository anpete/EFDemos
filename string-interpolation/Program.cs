using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demos
{
    internal class Program
    {
        private static void Main()
        {
            Console.Write("Enter a search term: ");

            var term = Console.ReadLine();

            SetupDatabase();

            using (var db = new BloggingContext())
            {
                // 1) FromSql with traditional format string.

                // 2) FromSql with naive interpolation

                // 3) FromSql with interpolation support

                Console.WriteLine();

//                foreach (var blog in blogs)
//                {
//                    Console.WriteLine(blog);
//                }

                Console.WriteLine();
            }
        }

        private static void SetupDatabase()
        {
            using (var db = new BloggingContext())
            {
                if (db.Database.EnsureCreated())
                {
                    db.Database.ExecuteSqlCommand(
                        "CREATE FUNCTION [dbo].[SearchBlogs] (@term nvarchar(200)) RETURNS TABLE AS RETURN (SELECT * FROM dbo.Blogs WHERE Url LIKE '%' + @term + '%')");

                    db.Blogs.Add(new Blog { Url = "http://sample.com/blogs/fish" });
                    db.Blogs.Add(new Blog { Url = "http://sample.com/blogs/catfish" });
                    db.Blogs.Add(new Blog { Url = "http://sample.com/blogs/cats" });
                    db.SaveChanges();
                }
            }
        }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.StringInterpolation;Trusted_Connection=True;ConnectRetryCount=0")
                .UseLoggerFactory(new LoggerFactory().AddConsole((s, l) => l == LogLevel.Information && !s.EndsWith("Connection")));
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}