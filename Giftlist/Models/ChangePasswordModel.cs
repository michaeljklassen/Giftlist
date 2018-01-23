using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Giftlist.Models
{
	public class ChangePasswordModel
	{
		[Required]
		[DataType(DataType.Password)]
		[Display(Name="Old Password")]
		public string OldPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name="New Password")]
		public string NewPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
		[Display(Name="Confirm Password")]
		public string ConfirmPassword { get; set; }
	}
}