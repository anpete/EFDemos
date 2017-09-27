using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Demos
{
    public class BlogService
    {
        private readonly BloggingContext _db;

        public BlogService(BloggingContext db)
        {
            _db = db;
        }

        public IEnumerable<Blog> SearchBlogs(string term)
        {
            var likeExpression = $"%{term}%";

            return _db.Blogs.Where(b => EF.Functions.Like(b.Url, likeExpression));
        }
    }
}
