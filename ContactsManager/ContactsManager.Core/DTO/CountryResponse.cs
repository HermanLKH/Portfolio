using System;
using System.Collections.Generic;
using DbContext;

namespace ServiceContracts.DTO
{
	/// <summary>
	/// DTO class is used as return type for most of CountriesService methods
	/// </summary>
	public class CountryResponse
	{
		public Guid CountryID { get; set; }
		public string? CountryName { get; set; }

		/// <summary>
		/// It compares the current object to another CountryResponse object by using CountryID
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>true if Country Id are same, else false</returns>
		public override bool Equals(object? obj)
		{
			if(obj == null) 
			{
				return false;
			}
			else if (obj is CountryResponse countryResponse)
			{
				return CountryID == countryResponse.CountryID;
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return CountryID.GetHashCode();
		}
	}
	
	public static class CountryExtensions
	{
		/// <summary>
		/// Converts from Country object to CountryResponse object
		/// </summary>
		/// <param name="country"></param>
		/// <returns>converted CountryResponse object</returns>
		public static CountryResponse ToCountryResponse(this Country country)
		{
			return new CountryResponse() 
			{
				CountryID = country.CountryID, 
				CountryName = country.CountryName,
			};
		}
	}
}
