// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
