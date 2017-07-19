using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Demos
{
    internal class Program
    {
        public static long ContextInstances;
        public static long RequestsProcessed;

        private static readonly TimeSpan _duration = TimeSpan.FromSeconds(10);
        private static readonly Stopwatch _stopwatch = new Stopwatch();

        private const int Threads = 32;

        private static void Main()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .AddDbContext<BloggingContext>(c => c.UseSqlServer(
                        @"Server=(localdb)\mssqllocaldb;Database=Demo.ContextPooling;Trusted_Connection=True;ConnectRetryCount=0;"))
                .BuildServiceProvider();

            SetupDatabase(serviceProvider);

            MonitorResults();

            var tasks = new Task[Threads];
            
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = SimulateContinualRequests(serviceProvider);
            }

            Task.WhenAll(tasks).Wait();
        }

        private static async Task SimulateContinualRequests(IServiceProvider serviceProvider)
        {
            while (_stopwatch.IsRunning)
            {
                using (var serviceScope = serviceProvider.CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<BloggingContext>();
                    
                    await context.Blogs.FirstAsync();
                }

                Interlocked.Increment(ref RequestsProcessed);
            }
        }

        private static void SetupDatabase(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<BloggingContext>();

                if (db.Database.EnsureCreated())
                {
                    db.Blogs.Add(new Blog { Name = "The Dog Blog", Url = "http://sample.com/dogs" });
                    db.Blogs.Add(new Blog { Name = "The Cat Blog", Url = "http://sample.com/cats" });
                    db.SaveChanges();
                }
            }
        }

        private static async void MonitorResults()
        {
            var lastInstanceCount = (long) 0;
            var lastRequestCount = (long) 0;
            var lastElapsed = TimeSpan.Zero;

            _stopwatch.Start();

            while (_stopwatch.Elapsed < _duration)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                var thisInstanceCount = ContextInstances;
                var thisRequestCount = RequestsProcessed;
                var thisElapsed = _stopwatch.Elapsed;

                var currentElapsed = thisElapsed - lastElapsed;
                var currentRequests = thisRequestCount - lastRequestCount;

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] "
                                  + $"Context creations per second: {thisInstanceCount - lastInstanceCount} | "
                                  + $"Requests per second: {Math.Round(currentRequests / currentElapsed.TotalSeconds)}");

                lastInstanceCount = thisInstanceCount;
                lastRequestCount = thisRequestCount;
                lastElapsed = thisElapsed;
            }

            Console.WriteLine("");
            Console.WriteLine($"Total context creations: {ContextInstances}");
            Console.WriteLine(
                $"Requests per second:     {Math.Round(RequestsProcessed / _stopwatch.Elapsed.TotalSeconds)}");

            _stopwatch.Stop();
        }
    }

    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
            Interlocked.Increment(ref Program.ContextInstances);
        }

        public DbSet<Blog> Blogs { get; set; }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}