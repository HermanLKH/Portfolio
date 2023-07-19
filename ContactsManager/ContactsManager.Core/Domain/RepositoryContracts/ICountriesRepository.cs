
using DbContext;

namespace RepositoryContracts
{
	/// <summary>
	/// Represent data access logic for managing Country entity
	/// </summary>
	public interface ICountriesRepository
	{
		/// <summary>
		/// Adds a new country object
		/// </summary>
		/// <param name="country">Country object to add</param>
		/// <returns>Returns the country object after adding it</returns>
		Task<Country> AddCountry(Country country);
		/// <summary>
		/// Returns all countries
		/// </summary>
		/// <returns>Returns a list of Country objects</returns>
		Task<List<Country>> GetAllCountries();
		/// <summary>
		/// Returns a country based on given country id
		/// </summary>
		/// <param name="countryID">country id to search</param>
		/// <returns>Returns a matching country object or null</returns>
		Task<Country?> GetCountryByCountryId(Guid countryID);
		/// <summary>
		/// Returns a country based on given country name
		/// </summary>
		/// <param name="countryName">country name to search</param>
		/// <returns>Returns a matching country object or null</returns>
		Task<Country?> GetCountryByCountryName(string countryName);
	}
}