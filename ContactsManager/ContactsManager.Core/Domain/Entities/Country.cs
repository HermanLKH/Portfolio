using System.ComponentModel.DataAnnotations;

namespace DbContext
{
	/// <summary>
	/// Domain model for Country
	/// (not exposed to presentation layer, include controller)
	/// </summary>
	public class Country
	{
		[Key] // primary key
		public Guid CountryID { get; set; }
		public string? CountryName { get; set; }

		public virtual ICollection<Person>? Persons { get; set; }
	}
}