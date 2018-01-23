using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Giftlist.DataAccess
{
	public class DAUsers
	{
		public static User GetUserByUsernamePassword(string username, string password)
		{
			var db = new GiftlistEntities();
			User validateduser = db.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();
			db.Dispose();
			db = null;

			return validateduser;
		}

		public static List<User> GetUsersByFamilyId(int familyid)
		{
			using (var db = new GiftlistEntities())
			{
				return db.Families.Where(f => f.FamilyId == familyid).SelectMany(f => f.Users).ToList();
			}

		}

		public static User GetUserById(int userid)
		{
			using (var db = new GiftlistEntities())
			{
				return db.Users.Where(u => u.UserId == userid).FirstOrDefault();
			}
		}

		public static List<User> GetCurrentUsersFamilyMembers()
		{
			List<User> retval = new List<User>();

			if (!HttpContext.Current.Session.IsNewSession)
			{
				User user = GetUserById((int)HttpContext.Current.Session["CurrentUserId"]);

				using (var db = new GiftlistEntities())
				{
					var families = db.Users.Where(u => u.UserId == user.UserId).First().Families.ToList();
					retval = db.Users.AsEnumerable().Where(u => u.Families.Any(fam => families.Contains(fam))).ToList();
				}
			}

			return retval;
		}

		public static void ChangePassword(int userid, string oldpassword, string newpassword)
		{
			string error = "";
			var db = new GiftlistEntities();
			User user = db.Users.Where(u =>
				u.UserId == userid &&
				u.Password == oldpassword
				).FirstOrDefault();
			if (null != user)
			{
				try
				{
					user.Password = newpassword;
					db.SaveChanges();
				}
				catch (DbEntityValidationException e)
				{
					foreach (var eve in e.EntityValidationErrors)
					{
						error += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
							eve.Entry.Entity.GetType().Name, eve.Entry.State);
						foreach (var ve in eve.ValidationErrors)
						{
							error += string.Format("- Property: \"{0}\", Error: \"{1}\"",
								ve.PropertyName, ve.ErrorMessage);
						}
					}
					throw new Exception(error);
				} 
			}
			else
			{
				throw new Exception("User not found.");
			}
			db.Dispose();
		}
	}
}