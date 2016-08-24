using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BlogSite_v1.Models
{
    public class PostCategoryContext : DbContext
    {
        public List<Category> Categories { get; set; }
        public List<Post> Posts { get; set; }
        public List<PostCategory> PostCategories { get; set; }

        public virtual Post Post { get; set; }
        public virtual Category Category { get; set; }
        public virtual PostCategory PostCategory { get; set; }
    }
}