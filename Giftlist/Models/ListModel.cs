using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Giftlist.DataAccess;

namespace Giftlist.Models
{
	public class ListModel
	{
		public List<User> FamilyMembers;
		public List<Item> ListItems;
		public List<ItemClaim> ClaimItems;
		public ItemClaim itemClaim;
		public string Error = string.Empty;
		public string CurrentUserName;

		public ListModel()
		{
			FamilyMembers = DAUsers.GetCurrentUsersFamilyMembers();
			User u = DAUsers.GetUserById((int)HttpContext.Current.Session["CurrentUserId"]);
			CurrentUserName = u.FName + ' ' + u.LName;
			HttpContext.Current.Session["CurrentUserName"] = CurrentUserName;
		}

		public ListModel(int UserId)
		{
			ListItems = DALists.GetListByUserId(UserId);
			ClaimItems = DALists.GetClaimsByUserId(UserId);
		}
	}

}