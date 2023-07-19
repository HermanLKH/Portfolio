using DbContext;
using ServiceContracts.Enums;
using System;
using System.Net;
using System.Reflection;
using System.Xml.Linq;

namespace ServiceContracts.DTO
{
	/// <summary>
	/// Act as DTO which is returned by most methods of Person services
	/// </summary>
	public class PersonResponse
	{
		public Guid PersonID { get; set; }
		public string? Name { get; set; }
		public string? Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Gender { get; set; }
		public Guid? CountryID { get; set; }
		public string? Country { get; set; }
		public string? Address { get; set; }
		public bool ReceiveNewsLetter { get; set; }
		public double? Age { get; set; }

		/// <summary>
		/// It compares the current object to another PersonResponse object by using PersonID
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>true or false</returns>
		/// <summary>
		public override bool Equals(object? obj)
		{
			if (obj != null)
			{
				if (obj is PersonResponse personResponse)
				{
					if (PersonID == personResponse.PersonID)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override int GetHashCode()
		{
			return PersonID.GetHashCode();
		}

		public override string ToString()
		{
			return $"Person ID: {PersonID}, Name: {Name}, Email: {Email}, " +
				$"Date of Birth: {DateOfBirth?.ToString("dd MMM yyyy")}, Gender: {Gender}, " +
				$"Country ID: {CountryID}, Country: {Country}, Address: {Address}, " +
				$"Receive News Letter: {ReceiveNewsLetter}, Age: {Age}";
		}

		// convert PersonResponse to other type of PersonRequest
		// because all person obj/ info is received using PersonRequest
		public PersonUpdateRequest ToPersonUpdateRequest() => new PersonUpdateRequest()
		{
			PersonID = PersonID,
			Name = Name,
			Email = Email,
			DateOfBirth = DateOfBirth,
			Gender = (!string.IsNullOrEmpty(Gender)) ? (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true) : null,
			CountryID = CountryID,
			Address = Address,
			ReceiveNewsLetter = ReceiveNewsLetter,
		};
	}
	public static class PersonExtensions
	{
		/// <summary>
		/// an extension method to convert Person object into PersonResponse
		/// </summary>
		/// <param name="person"></param>
		/// <returns>returns the converted PersonResponse object</returns>
		public static PersonResponse ToPersonResponse(this Person person)
		{
			return new PersonResponse()
			{
				PersonID = person.PersonID,
				Name = person.Name,
				Email = person.Email,
				DateOfBirth = person.DateOfBirth,
				Gender = person.Gender,
				CountryID = person.CountryID,
				Country = person.Country?.CountryName,
				Address = person.Address,
				ReceiveNewsLetter = person.ReceiveNewsLetter,
				Age = (person.DateOfBirth != null) 
						? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25)
						: null
			};
		}
	}
}
