//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlogSite_v1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public int CommentID { get; set; }
        public string CommentContext { get; set; }
        public System.DateTime CommentDate { get; set; }
        public int UserID { get; set; }
        public int PostID { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Post Post { get; set; }
    }
}
