using DietPlanner.Helpers;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace DietPlanner.Controllers
{
    public class NotificationController : Controller
    {
        private ApplicationUserManager _userManager;

        public NotificationController() { }

        public NotificationController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendEmail()
        {
            var userID = AccountHelper.GetLoggedUserId();
            UserManager.SendEmailAsync(userID, "shaskdha", "ajshdkadhasha");
            return View();
        }
    }
}