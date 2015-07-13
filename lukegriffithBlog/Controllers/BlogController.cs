using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using lukegriffithBlog.Models;
using lukegriffithBlog.ViewModels;
using System.Data.Entity.Infrastructure;


namespace lukegriffithBlog.Controllers
{
    public class BlogController : Controller
    {

        private lukegriffithBlogContext db = new lukegriffithBlogContext();

        
        // GET: Blog
        public ActionResult Index()
        {
            //var posts = db.Posts.OrderByDescending(i => i.dateCreated);
            var posts = db.Posts;
            
            return View(posts);
        }

        public ActionResult Post(int id, string urlSlug)
        {

            Posts posts = db.Posts.Find(id);
            PopulateCategories(posts);

            if (posts == null)
            {
                
                return HttpNotFound();
            }
            return View(posts);
        }

        public ActionResult category(int id)
        {
            var posts = db.Category.Find(id).posts;
            ViewBag.category = db.Category.Find(id).title.ToString();
            return View(posts);
        }

        private void PopulateCategories(Posts post)
        {
            var allCategories = db.Category;
            var assignedCategories = new HashSet<int>(post.category.Select(i => i.id));
            var viewModel = new List<linkedCategories>();

            foreach (var category in allCategories)
            {
                viewModel.Add(new linkedCategories
                {
                    id = category.id,
                    title = category.title,
                    linked = assignedCategories.Contains(category.id)
                });
            }
            ViewBag.Categories = viewModel;
        }
    }
}