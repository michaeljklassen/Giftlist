using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Giftlist.Models;
using Giftlist.DataAccess;

namespace Giftlist.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            User CurrentUser = new User();
            if (ModelState.IsValid)
            {
                CurrentUser = DataAccess.DAUsers.GetUserByUsernamePassword(model.Username, model.Password);
            }

            if (null != CurrentUser)
            {
                Session["CurrentUserId"] = CurrentUser.UserId;
                return RedirectToAction("Index", "List");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View(model);
        }

		public ActionResult Logout()
		{
			HttpContext.Session.Abandon();
			return RedirectToAction("Index", "Home");
		}

		public ActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (ModelState.IsValid)
			{
				//try
				//{
					DAUsers.ChangePassword((int)HttpContext.Session["CurrentUserId"], model.OldPassword, model.NewPassword);
					return RedirectToAction("Index", "Home");
				//}
				//catch (Exception e)
				//{
				//	ModelState.AddModelError("ChangePasswordError", e);
				//	return View(model);
				//}
			}
			else {
				return View(model);
			}
		}
	}
}
