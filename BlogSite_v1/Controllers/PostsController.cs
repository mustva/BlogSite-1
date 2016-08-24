using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSite_v1.Models;

namespace BlogSite_v1.Controllers
{
    public class PostsController : Controller
    {
        BlogSiteEntities db = new BlogSiteEntities();
        // GET: Post
        public ActionResult Index()
        {
            PostCategoryContext pcc = new PostCategoryContext();

            pcc.Categories = db.Category.ToList();
            pcc.Posts = db.Post.ToList();
            pcc.PostCategories = db.PostCategory.ToList();




            return View(pcc);
        }
    }
}