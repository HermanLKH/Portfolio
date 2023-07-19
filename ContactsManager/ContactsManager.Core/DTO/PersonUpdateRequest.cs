using DbContext;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
	/// <summary>
	/// Represent the DTO class to update an existing person
	/// </summary>
	public class PersonUpdateRequest
	{
		[Required(ErrorMessage = "Person ID cannot be blank")]
		public Guid PersonID { get; set; }
		[Required(ErrorMessage = "Person name cannot be blank")]
		public string? Name { get; set; }
		[Required(ErrorMessage = "Email cannot be blank")]
		[EmailAddress(ErrorMessage = "Email should be in a valid format")]
		public string? Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public GenderOptions? Gender { get; set; }
		public Guid? CountryID { get; set; }
		public string? Address { get; set; }
		public bool ReceiveNewsLetter { get; set; }

		/// <summary>
		/// Convert current PersonAddRequest object to a new Person object
		/// </summary>
		/// <returns>Returns an updated Person object</returns>
		public Person ToPerson()
		{
			return new Person()
			{
				PersonID = PersonID,
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
