using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
                foreach (var blog in db.Blogs) // Load the blogs
                {
                    Console.WriteLine(blog.Url);

                    if (blog.Posts == null) // Access the Posts navigation property
                    {
                        Console.WriteLine("No posts!");
                    }
                    else
                    {
                        foreach (var post in blog.Posts)
                        {
                            Console.WriteLine($" - {post.Title}");
                        }
                    }

                    Console.WriteLine();
                }
            }
        }

        private static void SetupDatabase()
        {
            using (var db = new BloggingContext())
            {
                if (db.Database.EnsureCreated())
                {
                    db.Blogs.Add(
                        new Blog
                        {
                            Url = "http://sample.com/blogs/fish",
                            Posts = new List<Post>
                            {
                                new Post { Title = "Fish care 101" },
                                new Post { Title = "Caring for tropical fish" },
                                new Post { Title = "Types of ornamental fish" }
                            }
                        });

                    db.Blogs.Add(
                        new Blog
                        {
                            Url = "http://sample.com/blogs/cats",
                            Posts = new List<Post>
                            {
                                new Post { Title = "Cat care 101" },
                                new Post { Title = "Caring for tropical cats" },
                                new Post { Title = "Types of ornamental cats" }
                            }
                        });

                    db.Blogs.Add(
                        new Blog
                        {
                            Url = "http://sample.com/blogs/catfish",
                            Posts = new List<Post>
                            {
                                new Post { Title = "Catfish care 101" },
                                new Post { Title = "History of the catfish name" }
                            }
                        });

                    db.SaveChanges();
                }
            }
        }
    }

    public class BloggingContext : DbContext
    {
        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory()
            .AddConsole((s, l) => l == LogLevel.Debug && s.EndsWith("Command"));

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                //.UseLazyLoadingProxies()
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(_loggerFactory)
                .UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=Demo.LazyLoading;Trusted_Connection=True;ConnectRetryCount=0;");
        }
    }

    public sealed class Blog
    {
        public Blog()
        {
        }

        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        private readonly Action<object, string> _lazyLoader;

        private Blog(Action<object, string> lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private ICollection<Post> _posts;

        public ICollection<Post> Posts
        {
            get
            {
                _lazyLoader?.Invoke(this, nameof(Posts));

                return _posts;
            }

            set => _posts = value;
        }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
    }
}
