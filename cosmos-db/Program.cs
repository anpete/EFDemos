// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.Logging;

namespace Demos
{
    public class Program
    {
        public static void Main()
        {
            var blogId = 1;
            var postId = 1;

            using (var cosmosDb = new BloggingContext())
            {
                // Recreate database

                cosmosDb.Database.EnsureDeleted();
                cosmosDb.Database.EnsureCreated();

                Console.WriteLine("Database created.");

                // Add some data...
                Console.WriteLine();

                cosmosDb.Blogs.AddRange(
                    new Blog
                    {
                        BlogId = blogId++,
                        Name = "ADO.NET",
                        Url = "http://blogs.msdn.com/adonet",
                        Posts = new List<Post>
                        {
                            new Post
                            {
                                PostId = postId++,
                                Title = "Welcome to this blog!"
                            },
                            new Post
                            {
                                PostId = postId,
                                Title = "Getting Started with ADO.NET"
                            }
                        }
                    },
                    new Blog
                    {
                        BlogId = blogId++,
                        Name = "ASP.NET",
                        Url = "http://blogs.msdn.com/aspnet"
                    },
                    new Blog
                    {
                        BlogId = blogId++,
                        Name = ".NET",
                        Url = "http://blogs.msdn.com/dotnet"
                    },
                    new SpecialBlog
                    {
                        BlogId = blogId,
                        Name = "SpecialBlog",
                        Url = "http://blogs.msdn.com/special"
                    });

                var count = cosmosDb.SaveChanges();

                Console.WriteLine($"Saved {count} records to Cosmos DB", count);
            }

            using (var cosmosDb = new BloggingContext())
            {
                Console.WriteLine("Executing query for all blogs...");

                foreach (var blog in cosmosDb.Blogs)
                {
                    Console.WriteLine($"{blog.GetType()} - {blog.Name} - {blog.Url}");
                }

                Console.WriteLine("Loading posts for ADO.NET blog...");

                var blog1 = cosmosDb.Blogs.Single(b => b.Name == "ADO.NET");

                cosmosDb.Entry(blog1).Collection(b => b.Posts).Load();

                foreach (var post in blog1.Posts)
                {
                    Console.WriteLine($" - {post.Title}");
                }

                Console.Write("Modifying post of the blog...");

                blog1.Posts[0].Content = "Content Removed";

                cosmosDb.SaveChanges();

                Console.WriteLine("done");
            }
        }
    }

    public class BloggingContext : DbContext
    {
        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory()
            .AddConsole((s, l) => l == LogLevel.Debug && s.EndsWith("Command"));

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<SpecialBlog> SpecialBlogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseDocumentDb(
                    "https://localhost:8081",
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                    "SampleApp")
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(_loggerFactory);
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class SpecialBlog : Blog
    {
        public string Special { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
