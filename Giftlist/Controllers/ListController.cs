using Giftlist.DataAccess;
using Giftlist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Giftlist.Controllers
{
	public class ListController : Controller
	{
		//
		// GET: /List/

		public ActionResult Index()
		{
			ListModel listmodel = new ListModel();
			return View(listmodel);
		}

		[HttpPost]
		public ActionResult Index(ListModel model)
		{
			if (ModelState.IsValid)
			{
				ItemClaim ic = new ItemClaim();
				ic.ItemId = int.Parse(Request["hdnItemId"]);
				ic.UserId = (int)HttpContext.Session["CurrentUserId"];
				ic.ClaimTypeId = int.Parse(Request["itemClaim.ClaimType"]);
				ic.Comment = Request["itemClaim.Comment"];
				ic.DateCreated = DateTime.Now;
				DataAccess.DALists.NewClaim(ic);
			}
			return View(model);
		}

		public ActionResult GetListByUserId(int userId = 0)
		{
			if (null == HttpContext.Session["CurrentUserId"])
			{
				List<Item> emptyList = new List<Item>();
				emptyList.Add(new Item { ItemId = 0, Title = "", Description = "Session has expired. <a href=\"/\">Log in again.</a>" });
				return Json(
					new { Items = emptyList,
							IsUser = false});
			}
			ListModel listmodel = new ListModel(userId);
			HttpContext.Session["CurrentListUserId"] = userId;
			int UserFromSession = (int)(null == HttpContext.Session["CurrentUserId"] ? 0 : HttpContext.Session["CurrentUserId"]);

			if (listmodel.ListItems.Count == 0)
			{
				return Json(
				new { IsUser = (userId == UserFromSession) });
			}
			return Json(
				new
					{
						Items = listmodel.ListItems.Select(li => new
						{
							ItemId = li.ItemId,
							Title = li.Title,
							Description = (li.Description == null ? "" : li.Description),
							Priority = li.Priority.Title,
							Url = li.Url,
							ImagePath = li.ImagePath,
							ItemClaimCount = li.ItemClaims.Count(),
							ItemClaims = li.ItemClaims.Select(ic => new
							{
								ClaimId = ic.ItemClaimId,
								ClaimType = ic.ClaimType.Title,
								DateClaimed = ic.DateCreated.ToShortDateString(),
								ClaimedBy = ic.User.FName,
								Comment = ic.Comment
							})
						}),
						IsUser = (userId == UserFromSession)
					});
		}

		public ActionResult New()
		{
			return View();
		}

		[HttpPost]
		public ActionResult New(Item i)
		{
			if (ModelState.IsValid)
			{
				try
				{
					i.UserId = (int)HttpContext.Session["CurrentUserId"];
					i.DateCreated = DateTime.Now;
					i.IsActive = true;// i.NonNullableIsActive;
					DALists.AddItem(i);
				}
				catch (Exception e)
				{
					ModelState.AddModelError("AddItem", e);
					return View(i);
				}
			}
			return RedirectToAction("Index");
		}

		public ActionResult Edit(int id)
		{
			Item i = new Item();
			try
			{
				i = DALists.GetListItemById(id);
				i.NonNullableIsActive = i.IsActive ?? false;
			}
			catch (Exception e)
			{
				ModelState.AddModelError("EditItem", e);
			}
			return View(i);
		}

		[HttpPost]
		public ActionResult Edit(Item i)
		{
			if (ModelState.IsValid)
			{
				try
				{
					i.IsActive = i.NonNullableIsActive;
					DALists.EditItem(i);
				}
				catch (Exception e)
				{
					ModelState.AddModelError("EditItem", e);
					return View(i);
				}
			}
			return RedirectToAction("Index");
		}

		public ActionResult Delete(int id)
		{
			try
			{
				DALists.DeleteItem(id);
			}
			catch (Exception e)
			{
				ModelState.AddModelError("EditItem", e);
			}

			return RedirectToAction("Index");
		}

		public ActionResult GetClaimsByUserId(int userId = 0)
		{
			if (null == HttpContext.Session["CurrentUserId"])
			{
				List<ItemClaim> emptyList = new List<ItemClaim>();
				emptyList.Add(new ItemClaim { ItemClaimId = 0, ItemId = 0, Comment = "Session has expired. <a href=\"/\">Log in again.</a>" });
				return Json(
					new
					{
						ItemClaims = emptyList,
						IsUser = false
					});
			}
			ListModel listmodel = new ListModel(userId);
			HttpContext.Session["CurrentListUserId"] = userId;
			int UserFromSession = (int)(null == HttpContext.Session["CurrentUserId"] ? 0 : HttpContext.Session["CurrentUserId"]);

			if (listmodel.ClaimItems.Count == 0)
			{
				return Json(
				new { IsUser = (userId == UserFromSession) });
			}
			return Json(
				new
				{
					ItemClaims = listmodel.Items.Select(li => new
					{
						ItemId = li.ItemId,
						Title = li.Title,
						Description = (li.Description == null ? "" : li.Description),
						Priority = li.Priority.Title,
						Url = li.Url,
						ImagePath = li.ImagePath,
						ItemClaimCount = li.ItemClaims.Count(),
						ItemClaims = li.ItemClaims.Select(ic => new
						{
							ClaimId = ic.ItemClaimId,
							ClaimType = ic.ClaimType.Title,
							DateClaimed = ic.DateCreated.ToShortDateString(),
							ClaimedBy = ic.User.FName,
							Comment = ic.Comment
						})
					}),
					IsUser = (userId == UserFromSession)
				});
		}
	}
}
