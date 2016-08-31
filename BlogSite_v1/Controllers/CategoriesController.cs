﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogSite_v1.Models;

namespace BlogSite_v1.Controllers
{

    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private BlogSiteEntities db = new BlogSiteEntities();

        
        public ActionResult Index()
        {
            return View(db.Category.ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                //
            }
            return View(category);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName,IsChecked")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                //
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName,IsChecked")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            List<Post> postList = new List<Post>();
            postList = (from x in db.PostCategory
                        where (x.CategoryId == id)
                        select x.Post).ToList();

            ViewBag.postsCounter = postList.Count();
            

            if (id == null)
            {
                //
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                //
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Category.Find(id);
            List<Post> postList = new List<Post>();

            postList = (from x in db.PostCategory
                       where (x.CategoryId == id)
                       select x.Post).ToList();

            
            if (postList.Count() != 0)
            {
                // Hata, Silemem!
            }

            else { 
            db.Category.Remove(category);
            db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}