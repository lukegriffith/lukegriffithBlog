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
    public class PostsController : Controller
    {
        private lukegriffithBlogContext db = new lukegriffithBlogContext();

        // GET: Posts
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Find(id);
            PopulateCategories(posts);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            var posts = new Posts();
            posts.category = new List<Category>();
            PopulateCategories(posts);
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "id,title,urlSlug,subTitle,body,published,dateCreated,timePosted")] Posts posts, string[] linkedCategories)
        {

            if (linkedCategories != null)
            {

                posts.category = new List<Category>();
                foreach (var category in linkedCategories)
                {
                    var categoryToLink = db.Category.Find(int.Parse(category));
                    posts.category.Add(categoryToLink);
                }
            }
            if (ModelState.IsValid)
            {
                posts.dateCreated = System.DateTime.Now;
                posts.timePosted = System.DateTime.Now;
                posts.urlSlug = posts.title.Replace(" ", "-").ToString();
                db.Posts.Add(posts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(posts);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Posts posts = db.Posts
                .Include(i => i.category)
                .Where(i => i.id == id)
                .Single();
            PopulateCategories(posts);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, string[] selectedCategories)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var postToUpdate = db.Posts
                .Include(i => i.category)
                .Where(i => i.id == id)
                .Single();

            if (TryUpdateModel(postToUpdate, "",
                new string[] { "title", "subTitle", "body", "published", "dateCreated", "timePosted" }))
            {
                try
                {
                    updateCategories(selectedCategories, postToUpdate);
                    postToUpdate.urlSlug = postToUpdate.title.Replace(" ", "-").ToString();
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changed. Try again");
                }

            }
            PopulateCategories(postToUpdate);
            return View(postToUpdate);
        }

        public void updateCategories(string[] selectedCategories, Posts postToUpdate)
        {
            if (selectedCategories == null)
            {
                postToUpdate.category = new List<Category>();
                return;
            }

            var hsSelectedCategories = new HashSet<string>(selectedCategories);
            var postCategory = new HashSet<int>
                (postToUpdate.category.Select(i => i.id));
            foreach ( var category in db.Category)
            {
                if (hsSelectedCategories.Contains(category.id.ToString()))
                {
                    if (!postCategory.Contains(category.id))
                    {
                        postToUpdate.category.Add(category);
                    }
                }
                else
                {
                    if (postCategory.Contains(category.id))
                    {
                        postToUpdate.category.Remove(category);
                    }
                }
            }

        }



        // GET: Posts/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = db.Posts.Find(id);
            db.Posts.Remove(posts);
            db.SaveChanges();
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