using BlogDS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogDS.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactMessage contact)
        {
            var Emailer = new EmailService();
            var mail = new IdentityMessage
            {
                Subject = "Message",
                Destination = ConfigurationManager.AppSettings["ContactEmail"],
                Body = "You have recieved a new ContactForm Submission from: " + "(" + contact.c_email + ") with the following contents. \n\n" + contact.c_message
            };
            Emailer.SendAsync(mail);

            return RedirectToAction("Index", "BlogPosts");
        }
    }
}