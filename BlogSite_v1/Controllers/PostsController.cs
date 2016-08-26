using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSite_v1.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace BlogSite_v1.Controllers
{
    public class PostsController : Controller
    {
        BlogSiteEntities db = new BlogSiteEntities();

        // GET: Post
        public ActionResult Index()
        {
            PostCategoryView pcc = new PostCategoryView();

            pcc.Categories = db.Category.ToList();
            pcc.Posts = db.Post.ToList();
            pcc.PostCategories = db.PostCategory.ToList();
            return View(pcc);
        }

        public ActionResult Details(int? id)
        {
            Post post = new Post();
            post = db.Post.Find(id);

            var comments = from x in db.Comment
                           where (x.PostId == post.PostId)
                           select x;

            post.Comment = comments.ToList();



            if (id == null)
            {
                //
            }

            if (post == null)
            {
                //
            }
            return View(post);
        }



        Post post = new Post();
        PostCategoryView pc = new PostCategoryView();

        public ActionResult CreatePost()
        {
            pc.Categories = db.Category.ToList();

            return View(pc);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost(PostCategoryView postcc)
        {
            if (ModelState.IsValid)
            {

#warning Saves Without UserID

                post.UserId = 1;
                post.PostDate = DateTime.Now.Date;
                post.PostTitle = postcc.Post.PostTitle;
                post.PostContext = postcc.Post.PostContext;

#warning Saves Without Category

                db.Post.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(postcc);
        }
    }
}