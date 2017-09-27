using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Demos
{
    public class Tests
    {
        [Fact]
        public void BlogService_BasicSearch()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("BlogService_BasicSearch")
                .Options;

            using (var db = new BloggingContext(options))
            {
                db.Blogs.Add(new Blog { Url = "aaa" });
                db.Blogs.Add(new Blog { Url = "baaab" });
                db.Blogs.Add(new Blog { Url = "ccc" });
                db.SaveChanges();
            }

            using (var db = new BloggingContext(options))
            {
                var service = new BlogService(db);
                var blogs = service.SearchBlogs("aaa");

                Assert.Equal(2, blogs.Count());
            }
        }
    }
}
