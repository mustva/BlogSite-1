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
    
    public partial class PostCategory
    {
        public int PostId { get; set; }
        public int CategoryId { get; set; }
        public int PostCategoryId { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Post Post { get; set; }
    }
}