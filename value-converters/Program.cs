// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            }
        }
    }

    public class BloggingContext : DbContext
    {
        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory()
            .AddConsole((s, l) => l == LogLevel.Information && s.EndsWith("Command"));

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Theme> Themes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.ValueConverters;Trusted_Connection=True;ConnectRetryCount=0")
                .UseLoggerFactory(_loggerFactory);
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
