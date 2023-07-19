using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.DTO
{
	public class RegisterDTO
	{
		[Required(ErrorMessage = "Name cannot be blank")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Email cannot be blank")]
		[EmailAddress(ErrorMessage = "Email should be in a proper format")]
		[DataType(DataType.EmailAddress)]
		// remote validation
		// asyncrhonous, no need refresh the browser
		[Remote("IsEmailRegistered", "Account", ErrorMessage = "Email is already taken")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Phonenumber cannot be blank")]
		[RegularExpression("^[0-9]*$", ErrorMessage = "Phonenumber should contain numbers only")]
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Password cannot be blank")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password cannot be blank")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirm password do not match")]
		public string ConfirmPassword { get; set; }

		public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
	}
}
