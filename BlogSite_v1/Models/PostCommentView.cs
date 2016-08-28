using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogSite_v1.Models
{
    public class PostCommentView
    {
        public virtual Post Post { get; set; }
        public virtual Comment Comment { get; set; }

        public List<Comment> Comments { get; set; }
    }
}