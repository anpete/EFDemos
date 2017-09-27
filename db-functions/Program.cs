using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demos
{
    public class Program
    {
        private static void Main()
        {
            SetupDatabase();

            using (var db = new BloggingContext())
            {
                var counts = db.Blogs.Select(b => BloggingContext.ComputePostCount(b.BlogId));

                foreach (var count in counts)
                {
                    Console.WriteLine(count);
                }
            }
        }

        private static void SetupDatabase()
        {
            using (var db = new BloggingContext())
            {
                if (db.Database.EnsureCreated())
                {
                    db.Database.ExecuteSqlCommand(
                        @"CREATE FUNCTION [dbo].[ComputePostCount] (@blogId INT) RETURNS INT 
                          AS
                          BEGIN
                            DECLARE @ret AS INT
                            SELECT @ret = COUNT(*) FROM dbo.Posts WHERE BlogId = @blogId
                            RETURN @ret
                          END");

                    var fishBlog = new Blog { Url = "http://sample.com/blogs/fish" };
                    fishBlog.Posts.Add(new Post { Title = "First Post!" });
                    db.Blogs.Add(fishBlog);

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
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.DbFunctions;Trusted_Connection=True;ConnectRetryCount=0")
                .UseLoggerFactory(new LoggerFactory().AddConsole((s, l) => l == LogLevel.Information && !s.EndsWith("Connection")));
        }

        [DbFunction(Schema = "dbo")]
        public static int ComputePostCount(int blogId)
        {
            return 0;
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();
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
