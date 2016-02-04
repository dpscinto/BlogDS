using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogDS.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace BlogDS.Controllers
{
    [RequireHttps]
    public class BlogPostsController : Controller
    {
        //[Authorize (Roles="Admin")] Every single class can only be accessed by an Admin

        private ApplicationDbContext db = new ApplicationDbContext();

        
        // GET: BlogPosts
        public ActionResult Index(int? page, string query)
        { 
            ViewBag.Query = query;
            //var qposts = db.Posts.AsQueryable();
            var qposts = from p in db.Posts
                           select p;
            if (!string.IsNullOrWhiteSpace(query))
            {
                qposts = qposts.Where(p => p.Title.Contains(query)
                    //|| p.Created.Contains(query)
                    //|| p.Updated.Contains(query)
                    || p.Body.Contains(query)
                    || p.MediaURL.Contains(query)
                    || p.Categories.Contains(query));
            }

            var posts = qposts.OrderByDescending(p => p.Created).ToPagedList(page ?? 1, 5);
            return View(posts);
        }


        // GET: BlogPosts
        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {

            return View(db.Posts.OrderByDescending(p => p.Created).ToList());
        }

        // GET: BlogPosts/Details/5
        public ActionResult Details(string slug)
        {
            if (String.IsNullOrWhiteSpace(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.FirstOrDefault(p => p.Slug == slug);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body,MediaURL")] BlogPost blogPost, HttpPostedFileBase image)
        {
            blogPost.Created = new DateTimeOffset(DateTime.Now);
            if (ModelState.IsValid)
            {
                var Slug = StringUtilities.URLFriendly(blogPost.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid Title");
                    return View(blogPost);
                }
                if (db.Posts.Any(p => p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique.");
                    return View(blogPost);
                }
                if(ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/images/Blog/"), fileName));
                    blogPost.MediaURL = "~/images/Blog/" + fileName;
                }

                blogPost.Slug = Slug;

                blogPost.Created = new DateTimeOffset(DateTime.Now);
                db.Posts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        // POST: Comments/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult createComment([Bind(Include = "PostId,Body")]Comment comment)
        {
            var slug = db.Posts.Find(comment.PostId).Slug;
            if (ModelState.IsValid)
            {
                comment.AuthorId = User.Identity.GetUserId();
                comment.Created = System.DateTimeOffset.Now;

                db.Comments.Add(comment);
                db.SaveChanges();
                //return RedirectToAction("Details", new {Slug = slug});
            }
            return RedirectToAction("Details", new { Slug = slug });

        }

        // GET: BlogPosts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }


        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Created,Updated,Title,Body,MediaURL")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.Updated = System.DateTimeOffset.Now;
                db.Posts.Attach(blogPost);

                db.Entry(blogPost).Property("Body").IsModified = true;
                db.Entry(blogPost).Property("Updated").IsModified = true;
                db.Entry(blogPost).Property("MediaURL").IsModified = true;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult editComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment blogComment = db.Comments.Find(id);
            if (blogComment == null)
            {
                return HttpNotFound();
            }
            return View(blogComment);
        }


        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [ValidateAntiForgeryToken]
        public ActionResult editComment([Bind(Include = "Id,PostId,Body")] Comment blogComment)
        {
            var slug = db.Posts.Find(blogComment.PostId).Slug;
            if (ModelState.IsValid)
            {
                db.Comments.Attach(blogComment);

                db.Entry(blogComment).Property("Body").IsModified = true;

                db.SaveChanges();

            }
            return RedirectToAction("Details", "BlogPosts", new { Slug = slug });
        }

        // GET: BlogPosts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.Posts.Find(id);
            db.Posts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult deleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment blogComment = db.Comments.Find(id);
            if (blogComment == null)
            {
                return HttpNotFound();
            }
            return View(blogComment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("deleteComment")]
        [Authorize(Roles = "Admin, Moderator")]
        [ValidateAntiForgeryToken]
        public ActionResult deleteCommentConfirmed(int id)
        {
            Comment blogComment = db.Comments.Find(id);
            var slug = blogComment.Post.Slug;           
            db.Comments.Remove(blogComment);
            db.SaveChanges();
            return RedirectToAction("Details", "BlogPosts", new { Slug = slug });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
