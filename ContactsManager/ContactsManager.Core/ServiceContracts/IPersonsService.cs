using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
	/// <summary>
	/// Represent business logic for manipulating Person entity
	/// </summary>
	public interface IPersonsService
	{
		/// <summary>
		/// Add a new person into a list of persons
		/// </summary>
		/// <param name="request"></param>
		/// <returns>Returns the same person details with newly generated PersonID</returns>
		Task<PersonResponse> AddPerson(PersonAddRequest? request);
		/// <summary>
		/// Returns all persons
		/// </summary>
		/// <returns>Returns a list of PersonResponse objects</returns>
		Task<List<PersonResponse>> GetAllPersons();
		/// <summary>
		/// Returns a person based on given person id
		/// </summary>
		/// <param name="personID">to search</param>
		/// <returns>Returns a matching PersonResponse object</returns>
		Task<PersonResponse?> GetPersonByPersonID(Guid? personID);
		/// <summary>
		/// Returns all persons which match the given filter search field and search string
		/// </summary>
		/// <param name="searchBy">property name to search</param>
		/// <param name="searchString">value to search</param>
		/// <returns>Returns a list of matching PersonResponse objects</returns>
		Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);
		/// <summary>
		/// Sort persons based on property name given
		/// </summary>
		/// <param name="personResponses">Represent list of persons sorted</param>
		/// <param name="sortBy">Property name used to sort</param>
		/// <param name="sortOrder">ASC or DESC</param>
		/// <returns>Returns a list of sorted PersonResponse objects</returns>
		Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> personResponses, string sortBy, SortOrderOptions sortOrder);
		/// <summary>
		/// Update a specified person details based on given details
		/// </summary>
		/// <param name="personUpdateRequest">person details to update</param>
		/// <returns>Returns the updated PersonResponse</returns>
		Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
		/// <summary>
		/// Delete a person based on given person id
		/// </summary>
		/// <param name="personID">person id to delete</param>
		/// <returns>Returns true if deletion is successful, otherwise false</returns>
		Task<bool> DeletePerson(Guid? personID);
		/// <summary>
		/// Returns persons as CSV
		/// </summary>
		/// <returns>Returns the memory stream with CSV data of persons</returns>
		Task<MemoryStream> GetPersonsCSV();
		/// <summary>
		/// Returns persons as Excel file
		/// </summary>
		/// <returns>Returns the memory stream with Excel data of persons</returns>
		Task<MemoryStream> GetPersonsExcel();
	}
}
