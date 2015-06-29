using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace lukegriffithBlog.Models
{
    public class Posts
    {

        public int id { get; set; }
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Post title")]
        [RegularExpression(@"^[a-zA-Z1-9$--_.+!*'(), ]*$")]
        public string title { get; set; }
        [Editable(false)]
        public string urlSlug { get; set; }
        [Display(Name = "SubTitle")]
        public string subTitle { get; set; }
        [Display(Name = "Content")]
        [AllowHtml]
        [MinLength(100)]
        public string body { get; set; }
        public bool published { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dateCreated { get; set; }
        [DataType(DataType.Time)]
        public DateTime timePosted { get; set; }


        public virtual ICollection<Category> category { get; set; }

    }
}