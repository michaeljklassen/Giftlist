using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Giftlist.DataAccess
{
	public class DALists
	{
		public static List<Item> GetListByUserId(int userid)
		{
			var db = new GiftlistEntities();
			return db.Items.Where(i => i.UserId == userid && i.IsActive == true)
				.OrderBy(i => i.PriorityId)
				.ThenByDescending(i => i.DateCreated)
				.ToList();
		}

		public static List<ItemClaim> GetClaimsByUserId(int userid)
		{
			var db = new GiftlistEntities();
			return db.ItemClaims.Where(ic => ic.UserId == userid && ic.Item.IsActive == true)
				.OrderByDescending(ic => ic.DateCreated)
				.ToList();
		}

		public static IEnumerable<SelectListItem> ClaimTypes
		{
			get {
				var db = new GiftlistEntities();
					return db.ClaimTypes.Select(ct => new SelectListItem
					{
						Text = ct.Title,
						Value = ct.ClaimTypeId.ToString()
					});
			}
		}

		public static IEnumerable<SelectListItem> Priorities
		{
			get
			{
				var db = new GiftlistEntities();
				return db.Priorities.Select(ct => new SelectListItem
				{
					Text = ct.Title,
					Value = ct.PriorityId.ToString()
				});
			}
		}



		public static void NewClaim(ItemClaim ic)
		{
			var db = new GiftlistEntities();
			db.ItemClaims.Add(ic);
			db.SaveChanges();
		}

		public static Item GetListItemById(int id)
		{
			var db = new GiftlistEntities();
			return db.Items.Where(i => i.ItemId == id).FirstOrDefault();
		}

		public static void AddItem(Item newitem)
		{
			var db = new GiftlistEntities();
			db.Items.Add(newitem);
			db.SaveChanges();
			db.Dispose();
		}

		public static void EditItem(Item editeditem)
		{
			var db = new GiftlistEntities();
			Item orig = db.Items.Where(i => i.ItemId == editeditem.ItemId).First();
			orig.Title = editeditem.Title;
			orig.Description = editeditem.Description;
			orig.PriorityId = editeditem.PriorityId;
			orig.Url = editeditem.Url;
			orig.ImagePath = editeditem.ImagePath;
			orig.DateLastModified = DateTime.Now;
			orig.IsActive = editeditem.IsActive;
			db.SaveChanges();
			db.Dispose();
		}

		public static void DeleteItem(int id)
		{
			var db = new GiftlistEntities();
			Item i = db.Items.Where(item => item.ItemId == id).FirstOrDefault();
			if (null != i)
			{
				var claims = i.ItemClaims.ToList();
				foreach (var item in claims)
				{					
					db.ItemClaims.Remove(item);
				}
				db.Items.Remove(i);
				db.SaveChanges();
			}
			db.Dispose();
		}
	}

}

namespace Giftlist
{
	public partial class Item
	{
		public bool NonNullableIsActive { get; set; }
	}

}