using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace lukegriffithBlog.Models
{
    public class Category
    {
        public int id { get; set; }
        [Display(Name = "Category Name")]
        public string title { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime dateCreated { get; set; }

        public virtual ICollection<Posts> posts { get; set; }
    }
}
