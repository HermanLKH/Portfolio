using DbContext;
using ServiceContracts.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
	/// <summary>
	/// Act as DTO to insert a new person
	/// </summary>
	public class PersonAddRequest
	{
		[Required(ErrorMessage = "Person name cannot be blank")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Email cannot be blank")]
		[EmailAddress(ErrorMessage = "Email should be in a valid format")]
		// datatype -> for html input type
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }
		[DataType(DataType.Date)]
		public DateTime? DateOfBirth { get; set; }

		[Required(ErrorMessage = "Gender cannot be blank")]
		public GenderOptions? Gender { get; set; }

		[Required(ErrorMessage = "Please select a country")]
		public Guid? CountryID { get; set; }
		public string? Address { get; set; }
		public bool ReceiveNewsLetter { get; set; }

		/// <summary>
		/// Convert current PersonAddRequest object to a new Person object
		/// </summary>
		/// <returns></returns>
		public Person ToPerson()
		{
			return new Person() 
			{ 
				Name = Name,
				Email = Email,
				DateOfBirth = DateOfBirth,
				Gender = Gender.ToString(),
				CountryID = CountryID,
				Address = Address,
				ReceiveNewsLetter = ReceiveNewsLetter,
			};
		}
	}
}
