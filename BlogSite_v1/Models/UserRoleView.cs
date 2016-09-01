using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogSite_v1.Models
{
    public class UserRoleView
    {

        public virtual AspNetUsers MyUser { get; set; }
        public virtual AspNetRoles MyRole { get; set; }
        public virtual AspNetUserRoles MyUserRole { get; set; }

        public virtual List<AspNetUsers> MyUsers { get; set; }
        public virtual List<AspNetRoles> MyRoles { get; set; }
        public virtual List<AspNetUserRoles> MyUserRoles { get; set; }
    }
}