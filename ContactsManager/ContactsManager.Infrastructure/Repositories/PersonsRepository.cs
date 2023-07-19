using DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using System.Linq.Expressions;

namespace Repositories
{
	public class PersonsRepository : IPersonsRepository
	{
		private readonly ApplicationDbContext _db;
		private readonly ILogger<PersonsRepository> _logger;
		public PersonsRepository(ApplicationDbContext db, ILogger<PersonsRepository> logger)
		{
			_db = db;
			_logger = logger;
		}

		public async Task<Person> AddPerson(Person person)
		{
			await _db.Persons.AddAsync(person);
			await _db.SaveChangesAsync();
			return person;
		}

		public async Task<bool> DeletePersonByPersonID(Guid personID)
		{
			_db.Persons.RemoveRange(_db.Persons.Where(temp => temp.PersonID == personID));
			int rowsDeleted = await _db.SaveChangesAsync();
			return rowsDeleted > 0;
		}

		public async Task<List<Person>> GetAllPersons()
		{
			return await _db.Persons.Include("Country").ToListAsync();
		}

		public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
		{
			_logger.LogInformation("GetFilteredPersons of PersonsRepository");

			return await _db.Persons.Include("Country")
				.Where(predicate)
				.ToListAsync();
		}

		public async Task<Person?> GetPersonByPersonID(Guid personID)
		{
			return await _db.Persons.Include("Country")
				.FirstOrDefaultAsync(temp => temp.PersonID == personID);
		}

		public async Task<Person> UpdatePerson(Person person)
		{
			Person matchingPerson = (await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == person.PersonID))!;
			
			matchingPerson.Name = person.Name;
			matchingPerson.Email = person.Email;
			matchingPerson.DateOfBirth = person.DateOfBirth;
			matchingPerson.Gender = person.Gender;
			matchingPerson.CountryID = person.CountryID;
			matchingPerson.Address = person.Address;
			matchingPerson.ReceiveNewsLetter = person.ReceiveNewsLetter;

			await _db.SaveChangesAsync();
			return matchingPerson;
		}
	}
}
