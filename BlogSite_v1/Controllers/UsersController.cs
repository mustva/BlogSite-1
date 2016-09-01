using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSite_v1.Models;

namespace BlogSite_v1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        BlogSiteEntities db = new BlogSiteEntities();
        // GET: Users
        public ActionResult Index()
        {
            UserRoleView urv = new UserRoleView();

            urv.MyUsers = db.AspNetUsers.ToList();
            urv.MyRoles = db.AspNetRoles.ToList();
            urv.MyUserRoles = db.AspNetUserRoles.ToList();

            return View(urv);
        }

        public ActionResult ChangeRole(int id)
        {
            var urv = new UserRoleView();

            urv.MyRoles = db.AspNetRoles.ToList();
            urv.MyUser = db.AspNetUsers.Find(id);

            urv.MyUserRoles = (from role in db.AspNetUserRoles
                               where role.UserId == urv.MyUser.Id
                               select role).ToList();

            foreach (var item in urv.MyUserRoles)
            {
                if (item.UserId == id)
                {
                    var role = from r in urv.MyRoles
                               where r.Id == item.RoleId
                               select r;

                    foreach (var it in role)
                    {
                        it.IsChecked = true;
                    }

                }
            }


            return View(urv);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeRole(UserRoleView urv)
        {


            AspNetUserRoles userRole = new AspNetUserRoles();

            if (ModelState.IsValid)
            {
                var checkedRole = from x in urv.MyRoles
                                  where x.IsChecked == true
                                  select x;

                var currentRoles = from cr in db.AspNetUserRoles
                                   where cr.UserId == urv.MyUser.Id
                                   select cr;

                foreach (var ro in currentRoles)
                {
                    db.AspNetUserRoles.Remove(ro);
                }


                foreach (var item in checkedRole)
                {

                    userRole.RoleId = item.Id;
                    userRole.UserId = urv.MyUser.Id;
                    db.AspNetUserRoles.Add(userRole);
                    db.SaveChanges();

                }

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}