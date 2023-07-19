using DbContext;
using System.Linq.Expressions;

namespace RepositoryContracts
{
	/// <summary>
	/// Represent data access logic for managing Person entity
	/// </summary>
	public interface IPersonsRepository
	{
		/// <summary>
		/// Adds a person
		/// </summary>
		/// <param name="person">Person object to add</param>
		/// <returns>Returns the Person object added</returns>
		Task<Person> AddPerson(Person person);
		/// <summary>
		/// Returns all persons
		/// </summary>
		/// <returns>Returns a list of Person objects</returns>
		Task<List<Person>> GetAllPersons();
		/// <summary>
		/// Returns a person based on person id
		/// </summary>
		/// <param name="personID">person id to search</param>
		/// <returns>Returns a matching Person object</returns>
		Task<Person?> GetPersonByPersonID(Guid personID);
		/// <summary>
		/// Returns all persons based on given condition
		/// </summary>
		/// <param name="predicate">LINQ expression to filter</param>
		/// <returns>Returns a list of matching Person objects</returns>
		Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);
		/// <summary>
		/// Updates a person based on details given
		/// </summary>
		/// <param name="person">Person object to update</param>
		/// <returns>Returns the updated Person object</returns>
		Task<Person> UpdatePerson(Person person);
		/// <summary>
		/// Deletes a person
		/// </summary>
		/// <param name="personID">person id to delete</param>
		/// <returns>Returns true if deletion is successful otherwise false</returns>
		Task<bool> DeletePersonByPersonID(Guid personID);
	}
}
