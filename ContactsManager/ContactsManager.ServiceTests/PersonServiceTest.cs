using AutoFixture;
using DbContext;
//using EntityFrameworkCoreMock;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System.Linq.Expressions;
using Xunit.Abstractions;

namespace CRUDTests
{
	public class PersonServiceTest
	{
		// private field
		private readonly IPersonsService _personService;

		private readonly Mock<IPersonsRepository> _personsRepositoryMock;
		private readonly IPersonsRepository _personsRepository;

		private readonly IFixture _fixture;

		private readonly ITestOutputHelper _testOutputHelper;

		// constructor
		public PersonServiceTest(ITestOutputHelper testOutputHelper)
		{
			// to create dummy objects
			_fixture = new Fixture();

			// mock repository
			_personsRepositoryMock = new Mock<IPersonsRepository>();
			_personsRepository = _personsRepositoryMock.Object;

			// create service with mocked repository
			_personService = new PersonsService(_personsRepository);

			// inject output helper
			_testOutputHelper = testOutputHelper;
		}

		#region AddPerson
		// PersonAddRequest: null
		// throw ArgumentNullException
		[Fact]
		public async Task AddPerson_NullPerson()
		{
			// arrange
			PersonAddRequest? personAddRequest = null;

			// no need to mock the repository method because it is not called

			// act
			Func<Task> action = (async () =>
			{
				await _personService.AddPerson(personAddRequest);
			});

			// act & assert
			//await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			//{
			//	await _personService.AddPerson(personAddRequest);
			//});

			// assert
			await action.Should().ThrowAsync<ArgumentNullException>();
		}

		// PersonAddRequest.Name: null
		// throw ArgumentNullException
		[Fact]
		public async Task AddPerson_NameIsNull_ToBeArgumentException()
		{
			// arrange
			PersonAddRequest? personAddRequest = _fixture
				.Build<PersonAddRequest>()
				.With(temp => temp.Name, null as string)
				.Create();

			// when PersonRepository.AddPerson is called, it has to return the same Person object
			_personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
				.ReturnsAsync(personAddRequest.ToPerson());

			// act
			Func<Task> action = (async () =>
			{
				await _personService.AddPerson(personAddRequest);
			});

			// assert
			await action.Should().ThrowAsync<ArgumentException>();

			// act & assert
			//await Assert.ThrowsAsync<ArgumentException>(async () =>
			//{
			//	await _personService.AddPerson(personAddRequest);
			//});
		}

		// Valid person details
		// Insert into person list
		// Return PersonResponse object with PersonID
		[Fact]
		public async Task AddPerson_ProperPersonDetails_ToBeSuccessful()
		{
			// arrange
			// autofixture with customization
			PersonAddRequest? personAddRequest = _fixture
				.Build<PersonAddRequest>()
				.With(temp => temp.Email, "someone@example.com")
				.Create();

			_personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
				.ReturnsAsync(personAddRequest.ToPerson());

			// act
			PersonResponse personAdded = await _personService.AddPerson(personAddRequest);

			// assert
			personAdded.PersonID.Should().NotBe(Guid.Empty);
			//Assert.True(personAdded.PersonID != Guid.Empty);
		}
		#endregion

		#region GetPersonByID
		/// <summary>
		/// personID: null
		/// return: null
		/// </summary>
		[Fact]
		public async Task GetPersonByID_NullPersonID_ToBeNull()
		{
			// arrange
			Guid personID = Guid.Empty;

			// act
			PersonResponse? personResponse = await _personService.GetPersonByPersonID(personID);

			// assert
			personResponse.Should().Be(null);
			//Assert.Null(personResponse);
		}

		/// <summary>
		///  valid personID
		///  return a PersonResponse object with matching person details
		/// </summary>
		[Fact]
		public async Task GetPersonByID_WithPersonID_ToBeSuccessful()
		{
			// arrange
			Person person = _fixture
				.Build<Person>()
				.With(temp => temp.Email, "someone@example.com")
				.With(temp => temp.Country, null as Country)
				.Create();
			PersonResponse personResponseExpected = person.ToPersonResponse();

			_personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
				.ReturnsAsync(person);

			// act
			PersonResponse? personGetResponse = await _personService.GetPersonByPersonID(person.PersonID);

			// assert
			personGetResponse.Should().Be(personResponseExpected);
			//Assert.Equal(personAddResponse, personGetResponse);
		}
		#endregion

		#region GetAllPersons
		/// <summary>
		/// return empty list by default
		/// </summary>
		[Fact]
		public async Task GetAllPersons_ToBeEmptyList()
		{
			_personsRepositoryMock.Setup(temp => temp.GetAllPersons())
				.ReturnsAsync(new List<Person>());

			// act
			List<PersonResponse> personResponses = await _personService.GetAllPersons();

			// assert
			personResponses.Should().BeEmpty();
			//Assert.Empty(personResponses);
		}

		/// <summary>
		/// Add a few persons
		/// Returns the same persons added
		/// </summary>
		[Fact]
		public async Task GetAllPersons_AddFewPersons_ToBeSuccessful()
		{
			// arrange
			List<Person> persons = new List<Person>()
			{
				_fixture.Build<Person>()
					.With(temp => temp.Country, null as Country)
					.With(temp => temp.Email, "someone_1@example.com")
					.Create(),
				_fixture.Build<Person>()
					.With(temp => temp.Country, null as Country)
					.With(temp => temp.Email, "someone_2@example.com")
					.Create(),
				_fixture.Build<Person>()
					.With(temp => temp.Country, null as Country)
					.With(temp => temp.Email, "someone_3@example.com")
					.Create(),
			};

			List<PersonResponse> personResponsesExpected = persons.Select(temp => temp.ToPersonResponse()).ToList();

			// print expected
			_testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse in personResponsesExpected)
            {
				_testOutputHelper.WriteLine(personResponse.ToString());
            }

			_personsRepositoryMock.Setup(temp => temp.GetAllPersons())
				.ReturnsAsync(persons);

            // act
            List<PersonResponse>? personGetAllResponses = await _personService.GetAllPersons();

			// print actual
			_testOutputHelper.WriteLine("Actual: ");
			foreach (PersonResponse personResponse in personGetAllResponses)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}

			// assert
			personGetAllResponses.Should().BeEquivalentTo(personResponsesExpected);
			//Assert.Equal(personAddResponses, personGetAllResponses);
		}
		#endregion

		#region GetFilteredPersons
		/// <summary>
		/// search string: empty
		/// return: empty list
		/// </summary>
		[Fact]
		public async Task GetFilteredPersons_EmptySearchString_ToBeSuccessful()
		{
			// arrange
			List<Person> persons = new List<Person>()
			{
				_fixture.Build<Person>()
					.With(temp => temp.Country, null as Country)
					.With(temp => temp.Email, "someone_1@example.com")
					.Create(),
				_fixture.Build<Person>()
					.With(temp => temp.Country, null as Country)
					.With(temp => temp.Email, "someone_2@example.com")
					.Create(),
				_fixture.Build<Person>()
					.With(temp => temp.Country, null as Country)
					.With(temp => temp.Email, "someone_3@example.com")
					.Create(),
			};

			List<PersonResponse> personResponsesExpected = persons.Select(temp => temp.ToPersonResponse()).ToList();

			// print expected
			_testOutputHelper.WriteLine("Expected: ");
			foreach (PersonResponse personResponse in personResponsesExpected)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}

			_personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
				.ReturnsAsync(persons);

			// act
			List<PersonResponse>? getFilteredPersonResponses = await _personService.GetFilteredPersons
				(nameof(Person.Name), "");

			// print actual
			_testOutputHelper.WriteLine("Actual: ");
			foreach (PersonResponse personResponse in getFilteredPersonResponses)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}

			// assert
			getFilteredPersonResponses.Should().BeEquivalentTo(personResponsesExpected);
			//Assert.Equal(personAddResponses, sortedPersonResponses);
		}
		/// <summary>
		/// search based on person name and some search string
		/// return: a list of matching Person objects
		/// </summary>
		[Fact]
		public async Task GetFilteredPersons_SearchByNames_ToBeSuccessful()
		{
			// arrange
			List<Person> persons = new List<Person>()
			{
				_fixture.Build<Person>()
					.With(temp => temp.Country, null as Country)
					.With(temp => temp.Email, "someone_1@example.com")
					.Create(),
				_fixture.Build<Person>()
					.With(temp => temp.Country, null as Country)
					.With(temp => temp.Email, "someone_2@example.com")
					.Create(),
				_fixture.Build<Person>()
					.With(temp => temp.Country, null as Country)
					.With(temp => temp.Email, "someone_3@example.com")
					.Create(),
			};

			List<PersonResponse> personResponsesExpected = persons.Select(temp => temp.ToPersonResponse()).ToList();

			// print expected
			_testOutputHelper.WriteLine("Expected: ");
			foreach (PersonResponse personResponse in personResponsesExpected)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}

			_personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
				.ReturnsAsync(persons);

			// act
			List<PersonResponse>? getFilteredPersonResponses = await _personService.GetFilteredPersons
				(nameof(Person.Name), "dA");

			// print actual
			_testOutputHelper.WriteLine("Actual: ");
			foreach (PersonResponse personResponse in getFilteredPersonResponses)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}

			// assert
			getFilteredPersonResponses.Should().BeEquivalentTo(personResponsesExpected);
			//Assert.Contains(personAddResponses[1], sortedPersonResponses);
			//Assert.Contains(personAddResponses[2], sortedPersonResponses);
		}
		#endregion

		#region GetSortedPersons
		[Fact]
		public async Task GetSortedPersons_ToBeSuccessful()
		{
			// arrange
			List<Person> persons = new List<Person>()
			{
				_fixture.Build<Person>()
					.With(temp => temp.Country, null as Country)
					.With(temp => temp.Email, "someone_1@example.com")
					.Create(),
				_fixture.Build<Person>()
					.With(temp => temp.Country, null as Country)
					.With(temp => temp.Email, "someone_2@example.com")
					.Create(),
				_fixture.Build<Person>()
					.With(temp => temp.Country, null as Country)
					.With(temp => temp.Email, "someone_3@example.com")
					.Create(),
			};

			List<PersonResponse> getPersonResponses = persons.Select(temp => temp.ToPersonResponse()).ToList();

			// act
			List<PersonResponse>? sortedPersonResponses = await _personService.GetSortedPersons(
				getPersonResponses, nameof(Person.Name), SortOrderOptions.ASC);

			// print actual
			_testOutputHelper.WriteLine("Actual: ");
			foreach (PersonResponse personResponse in sortedPersonResponses)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}

			// assert
			sortedPersonResponses.Should().BeInAscendingOrder(temp => temp.Name);
			//Assert.Equal(personAddResponses[2], sortedPersonResponses[0]);
			//Assert.Equal(personAddResponses[1], sortedPersonResponses[1]);
			//Assert.Equal(personAddResponses[0], sortedPersonResponses[2]);
		}
		#endregion

		#region UpdatePerson
		/// <summary>
		/// PersonUpdateRequest: null
		/// throw argument null exception
		/// </summary>
		[Fact]
		public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
		{
			// arrange
			PersonUpdateRequest? personUpdateRequest = null;

			// act
			Func<Task> action = async () =>
			{
				await _personService.UpdatePerson(personUpdateRequest);
			};

			// assert
			await action.Should().ThrowAsync<ArgumentNullException>();
		}
		/// <summary>
		/// Invalid person id
		/// Throw argument exception
		/// </summary>
		[Fact]
		public async Task UpdatePerson_InvalidPersonID_ToBeArgumentException()
		{
			// arrange
			PersonUpdateRequest? personUpdateRequest = _fixture
				.Build<PersonUpdateRequest>()
				.With(temp => temp.PersonID, Guid.NewGuid())
				.Create();

			_personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
				.ReturnsAsync(null as Person);

			// act
			Func<Task> action = async () =>
			{
				await _personService.UpdatePerson(personUpdateRequest);
			};

			// assert
			await action.Should().ThrowAsync<ArgumentException>();
		}
		/// <summary>
		/// person name: null
		/// Throw argument exception
		/// </summary>
		[Fact]
		public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
		{
			// arrange
			Person person = _fixture.Build<Person>()
				.With(temp => temp.Gender, "Male")
				.With(temp => temp.Name, null as string)
				.With(temp => temp.Country, null as Country)
				.Create();

			PersonResponse personResponse = person.ToPersonResponse();
			PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

			// act
			Func<Task> action = async () =>
			{
				await _personService.UpdatePerson(personUpdateRequest);
			};

			// assert
			await action.Should().ThrowAsync<ArgumentException>();
		}
		/// <summary>
		/// valid updated person details
		/// Returns an updated PersonResponse object
		/// </summary>
		[Fact]
		public async Task UpdatePerson_FullDetails_ToBeSuccessful()
		{
			// arrange
			Person person = _fixture.Build<Person>()
				.With(temp => temp.Gender, "Male")
				.With(temp => temp.Email, "someone@example.com")
				.With(temp => temp.Country, null as Country)
				.Create();

			PersonResponse personResponseExpected = person.ToPersonResponse();
			PersonUpdateRequest personUpdateRequest = personResponseExpected.ToPersonUpdateRequest();

			_personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
				.ReturnsAsync(person);
			_personsRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>()))
				.ReturnsAsync(person);

			// assert
			PersonResponse updatedPersonResponse = await _personService.UpdatePerson(personUpdateRequest);
			PersonResponse? personGetResponse = await _personService.GetPersonByPersonID(updatedPersonResponse.PersonID);

			// act
			personGetResponse.Should().Be(updatedPersonResponse);
			//Assert.Equal(updatedPersonResponse, personGetResponse);
		}
		#endregion

		#region DeletePerson
		/// <summary>
		/// If invalid PersonID is supplied, return false
		/// </summary>
		[Fact]
		public async Task DeletePerson_InvalidPersonID()
		{
			// act
			bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

			_personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
				.ReturnsAsync(null as Person);

			// assert
			isDeleted.Should().BeFalse();
			//Assert.False(isDeleted);
		}
		/// <summary>
		/// If valid PersonID is supplied, return true
		/// </summary>
		[Fact]
		public async Task DeletePerson_ValidPersonID()
		{
			// arrange
			Person person = _fixture
				.Build<Person>()
				.With(temp => temp.Email, "someone@example.com")
				.With(temp => temp.Gender, "Male")
				.With(temp => temp.Country, null as Country)
				.Create();

			_personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
				.ReturnsAsync(person);
			_personsRepositoryMock.Setup(temp => temp.DeletePersonByPersonID(It.IsAny<Guid>()))
				.ReturnsAsync(true);

			// act
			bool isDeleted = await _personService.DeletePerson(person.PersonID);

			// assert
			isDeleted.Should().BeTrue();
			//Assert.True(isDeleted);
		}
		#endregion
	}
}
