using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogSite_v1.Models
{
    public class CategoriesViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool CategoryChecked { get; set; }
    }
}