using ServiceContracts.DTO;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
	/// <summary>
	/// Represents business logic for manipulating Country entity
	/// </summary>
	public interface ICountriesService
	{
		/// <summary>
		/// Adds a Country object to the list of countries
		/// </summary>
		/// <param name="countryAddRequest"></param>
		/// <returns>Returns the Country object after adding it (include newly generated country id)
		/// </returns>
		Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
		/// <summary>
		/// Returns all countries from the list
		/// </summary>
		/// <returns>All countries from the list as List of CountryResponse</returns>
		Task<List<CountryResponse>> GetAllCountries();
		/// <summary>
		/// Returns a country object based on given country id
		/// </summary>
		/// <param name="countryId">to search</param>
		/// <returns>Matching country as CountryResponse object</returns>
		Task<CountryResponse?> GetCountryByCountryId(Guid? countryId);
		/// <summary>
		/// Uploads countries from excel file into database
		/// </summary>
		/// <param name="formFile">Excel file with list of countries</param>
		/// <returns>Returns number of countries added</returns>
		// IFormFile => file submitted from the browser
		Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
	}
}