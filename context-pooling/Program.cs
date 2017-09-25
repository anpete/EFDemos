// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Demos
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options) =>
            Program.ContextCreated();

        public DbSet<Blog> Blogs { get; set; }
    }

    public class BlogController
    {
        private readonly BloggingContext _context;

        public BlogController(BloggingContext context) =>
            _context = context;

        public async Task ActionAsync()
        {
            await _context.Blogs.FirstAsync();
        }
    }

    public class Startup
    {
        private readonly string _connectionString =
            @"Server=(localdb)\mssqllocaldb;Database=Demo.ContextPooling;Integrated Security=True;ConnectRetryCount=0";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BloggingContext>(c => c.UseSqlServer(_connectionString));

            #region Enable pooling
            //services.AddDbContextPool<BloggingContext>(c => c.UseSqlServer(_connectionString));
            #endregion
        }
    }

    public class Program
    {
        private const int Threads = 32;
        private const int Seconds = 10;

        private static long _contextInstances;
        private static long _requestsProcessed;

        private static void Main()
        {
            var serviceCollection = new ServiceCollection();
            new Startup().ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            SetupDatabase(serviceProvider);

            var stopwatch = new Stopwatch();

            MonitorResults(TimeSpan.FromSeconds(Seconds), stopwatch);

            var tasks = new Task[Threads];

            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = SimulateRequestsAsync(serviceProvider, stopwatch);
            }

            Task.WhenAll(tasks).Wait();
        }

        public static void ContextCreated()
        {
            Interlocked.Increment(ref _contextInstances);
        }

        private static void SetupDatabase(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<BloggingContext>();

                if (context.Database.EnsureCreated())
                {
                    context.Blogs.Add(new Blog { Name = "The Dog Blog", Url = "http://sample.com/dogs" });
                    context.Blogs.Add(new Blog { Name = "The Cat Blog", Url = "http://sample.com/cats" });
                    context.SaveChanges();
                }
            }
        }

        private static async Task SimulateRequestsAsync(IServiceProvider serviceProvider, Stopwatch stopwatch)
        {
            while (stopwatch.IsRunning)
            {
                using (var serviceScope = serviceProvider.CreateScope())
                {
                    await new BlogController(serviceScope.ServiceProvider.GetService<BloggingContext>()).ActionAsync();
                }
                Interlocked.Increment(ref _requestsProcessed);
            }
        }

        private static async void MonitorResults(TimeSpan duration, Stopwatch stopwatch)
        {
            var lastInstanceCount = 0L;
            var lastRequestCount = 0L;
            var lastElapsed = TimeSpan.Zero;

            stopwatch.Start();

            while (stopwatch.Elapsed < duration)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                var thisInstanceCount = _contextInstances;
                var thisRequestCount = _requestsProcessed;
                var thisElapsed = stopwatch.Elapsed;

                var currentElapsed = thisElapsed - lastElapsed;
                var currentRequests = thisRequestCount - lastRequestCount;

                Console.WriteLine(
                    $"[{DateTime.Now:HH:mm:ss.fff}] "
                    + $"Context creations: {thisInstanceCount - lastInstanceCount} | "
                    + $"Requests per second: {Math.Round(currentRequests / currentElapsed.TotalSeconds)}");

                lastInstanceCount = thisInstanceCount;
                lastRequestCount = thisRequestCount;
                lastElapsed = thisElapsed;
            }

            Console.WriteLine("");
            Console.WriteLine($"Total context creations: {_contextInstances}");
            Console.WriteLine(
                $"Requests per second:     {Math.Round(_requestsProcessed / stopwatch.Elapsed.TotalSeconds)}");

            stopwatch.Stop();
        }
    }
}
