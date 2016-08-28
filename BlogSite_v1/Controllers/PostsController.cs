using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSite_v1.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

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

            PostCommentView postCommentView = new PostCommentView();
            
            //Post post = new Post();

            postCommentView.Post = db.Post.Find(id);

            //post = db.Post.Find(id);

            var comments = from x in db.Comment
                           where (x.PostId == postCommentView.Post.PostId)
                           select x;

            postCommentView.Comments = comments.ToList();

           

            //post.Comment = comments.ToList();



            if (id == null)
            {
                //
            }

            if (postCommentView == null)
            {
                //
            }

            
            return View(postCommentView);
        }



        
        PostCategory postCategory = new PostCategory();
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

                Post post = new Post();
                post.UserId = Convert.ToInt32(User.Identity.GetUserId());
                post.PostDate = DateTime.Now.Date;
                post.PostTitle = postcc.Post.PostTitle;
                post.PostContext = postcc.Post.PostContext;
                db.Post.Add(post);
                


                var checkedCat = from x in postcc.Categories
                                 where x.IsChecked == true
                                 select x;

                if (checkedCat == null)
                {
#warning When the category is not selected, the post won't be saved. 
#warning Kategori seçmeyince db.Save işlemiyor.
                }


                else
                {
                    foreach (var item in checkedCat)
                    {

                        postCategory.CategoryId = item.CategoryId;
                        postCategory.PostId = post.PostId;
                        db.PostCategory.Add(postCategory);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }

            return View(postcc);
        }




        
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
            return RedirectToAction("Details", new { id = comment.PostId });
        }





        public ActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                //
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                //
            }
            return View(post);
        }

        // POST: x/Delete/5
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

    }
}