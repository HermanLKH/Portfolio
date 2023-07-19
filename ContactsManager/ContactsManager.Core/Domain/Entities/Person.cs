using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbContext
{
	/// <summary>
	/// Person domain class
	/// </summary>
	public class Person
	{
		[Key]
		public Guid PersonID { get; set; }

		[StringLength(40)]
		public string? Name { get; set; }

		[StringLength(40)]
		public string? Email { get; set; }
		public DateTime? DateOfBirth { get; set; }

		[StringLength(10)]
		public string? Gender { get; set; }

		// unique identifier
		public Guid? CountryID { get; set; }

		[StringLength(200)]
		public string? Address { get; set; }

		// bit
		public bool ReceiveNewsLetter { get; set; }

		//[Column("TaxIdentificationNumber", TypeName = "varchar(8)")]
		public string? TIN { get; set; }

		//[ForeignKey([foreign key property name in current model])]
		//public [model the foreign key is linked to]?[navigation property name] { get; set; }
		[ForeignKey("CountryID")]
		public Country? Country { get; set; }
	}
}
