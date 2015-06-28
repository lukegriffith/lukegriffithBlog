using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lukegriffithBlog.Controllers;
using lukegriffithBlog.Models;
using lukegriffithBlog.ViewModels;

namespace lukegriffithBlog.Controllers
{
    public class BlogController : Controller
    {

        private lukegriffithBlogContext db = new lukegriffithBlogContext();

        
        // GET: Blog
        public ActionResult Index()
        {
            var posts = db.Posts;
            return View(posts);
        }


    }
}