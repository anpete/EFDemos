using Microsoft.EntityFrameworkCore;

namespace Demos
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var context = new BloggingContext();

            context.Database.ExecuteSqlCommand($"select * from {args[1]}");
        }

        public class BloggingContext : DbContext
        {
            public DbSet<Blog> Blogs { get; set; }
        }

        public class Blog
        {
            public int BlogId { get; set; }
            public string Url { get; set; }
        }
    }
}
