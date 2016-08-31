using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSite_v1.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Net;

namespace BlogSite_v1.Controllers
{
    public class PostsController : Controller
    {
        BlogSiteEntities db = new BlogSiteEntities();


        public ActionResult Index()
        {
            PostCategoryView pcc = new PostCategoryView();

            pcc.Categories = db.Category.ToList();
            pcc.Posts = db.Post.OrderByDescending(x=> x.PostDate).ToList();
            pcc.PostCategories = db.PostCategory.ToList();
            return View(pcc);
        }


        [Authorize(Roles = "Admin")]
        #region Edit Post
        // GET: x/Edit/5
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", post.UserId);
            return View(post);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "PostId,PostTitle,PostDate,PostContext,UserId")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", post.UserId);
            return View(post);
        }

        #endregion
        
        #region Details of Posts
        public ActionResult DetailsPost(int? id)
        {

            PostCommentView postCommentView = new PostCommentView();

            postCommentView.Post = db.Post.Find(id);


            var comments = from x in db.Comment
                           where (x.PostId == postCommentView.Post.PostId)
                           select x;

            postCommentView.Comments = comments.ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (postCommentView == null)
            {
                return HttpNotFound();
            }


            return View(postCommentView);
        }
        #endregion

        [Authorize(Roles = "Admin")]
        #region Add Post
        public ActionResult AddPost()
        {
            PostCategoryView pc = new PostCategoryView();
            pc.Categories = db.Category.ToList();

            return View(pc);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost(PostCategoryView postcc)
        {
            bool checkTitle = true, checkContext = true, checkCategory = true;
            PostCategory postCategory = new PostCategory();
            if (ModelState.IsValid)
            {


                if (postcc.Post.PostTitle == null)
                {
                    ModelState.AddModelError("", "A post cannot be without a title!");
                    checkTitle = false;                    
                }
                else
                {
                    checkTitle = true;
                }

                if (postcc.Post.PostContext == null)
                {
                    ModelState.AddModelError("", "A post cannot be without a content!");
                    checkContext = false;
                }
                else
                {
                    checkContext = true;
                }

                if (postcc.Categories.All(x=> x.IsChecked == false))
                {
                    ModelState.AddModelError("", "A post cannot be without a category!");
                    checkCategory = false;
                }
                else
                {
                    checkCategory = true;
                }




                Post post = new Post();
                post.UserId = Convert.ToInt32(User.Identity.GetUserId());
                post.PostDate = DateTime.Now.Date;
                post.PostTitle = postcc.Post.PostTitle;
                post.PostContext = postcc.Post.PostContext;
                db.Post.Add(post);



                var checkedCat = from x in postcc.Categories
                                 where x.IsChecked == true
                                 select x;

                
                    foreach (var item in checkedCat)
                    {

                        postCategory.CategoryId = item.CategoryId;
                        postCategory.PostId = post.PostId;
                        db.PostCategory.Add(postCategory);
                        db.SaveChanges();
                        
                    }
                    
                

                if(checkTitle && checkContext && checkCategory)
                {
                    return RedirectToAction("Index");
                }
                

            }


            postcc.Categories = db.Category.ToList();
            return View(postcc);
        }
        #endregion

        [Authorize(Roles = "Admin")]
        #region Delete Post
        public ActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }


        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var deletePC = new List<PostCategory>();
            var deleteComment = new List<Comment>();

            Post post = db.Post.Find(id);


            deletePC = db.PostCategory.Where(x => x.PostId == post.PostId).ToList();
            deleteComment = db.Comment.Where(x => x.PostId == post.PostId).ToList();

            foreach (var cat in deletePC)
            {
                db.PostCategory.Remove(cat);
            }

            foreach (var com in deleteComment)
            {
                db.Comment.Remove(com);
            }

            db.Post.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        [Authorize(Roles = "Admin, User")]
        #region Add Comment
        public ActionResult AddComment()
        {

            Comment comment = new Comment();

            return View(comment);
        }

        [HttpPost]
        public ActionResult AddComment(Comment com, int id)
        {

            Comment comment = new Comment();

            if (ModelState.IsValid)
            {
                comment.PostId = db.Post.Find(id).PostId;
                comment.CommentContext = com.CommentContext;
                comment.CommentDate = DateTime.Now.Date;
                comment.UserId = Convert.ToInt32(User.Identity.GetUserId());

                db.Comment.Add(comment);
                db.SaveChanges();

            }
            return RedirectToAction("DetailsPost", new { id = comment.PostId });
        }
        #endregion

        [Authorize(Roles = "Admin, User")]
        #region Delete Comment
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comment.Find(id);
            ViewBag.UserID = Convert.ToInt32(User.Identity.GetUserId());
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComment(int id)
        {

            

            var deleteComment = new Comment();
            deleteComment = db.Comment.Find(id);

            if ((User.IsInRole("User") && deleteComment.UserId == Convert.ToInt32(User.Identity.GetUserId())) || User.IsInRole("Admin"))
            {


            db.Comment.Remove(deleteComment);
            db.SaveChanges();
            }

            return RedirectToAction("DetailsPost", new { id = deleteComment.PostId });
        }
        #endregion
    }
}