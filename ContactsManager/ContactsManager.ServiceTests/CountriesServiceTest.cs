using DbContext;
//using EntityFrameworkCoreMock;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Moq;
using AutoFixture;
using RepositoryContracts;
using FluentAssertions;

namespace CRUDTests
{
	public class CountriesServiceTest
	{
		private readonly ICountriesService _countriesService;

		private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
		private readonly ICountriesRepository _countriesRepository;

		private readonly IFixture _fixture;

		public CountriesServiceTest()
		{
			_fixture = new Fixture();

			_countriesRepositoryMock = new Mock<ICountriesRepository>();
			_countriesRepository = _countriesRepositoryMock.Object;

			_countriesService = new CountriesService(_countriesRepository);
		}
		
		// mock db context
		//public CountriesServiceTest()
		//{
		//	List<Country> countriesInitialData = new List<Country>() { };

		//	create mock dbcontext
		//	DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
		//		new DbContextOptionsBuilder<ApplicationDbContext>().Options);

		//	pass the implementation of mock db context
		//   ApplicationDbContext dbContext = dbContextMock.Object;

		//	mock the dbset
		//	dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);

		//	pass the mock dbcontext to the service so the services only interact with mock layer
		//   _countriesService = new CountriesService(dbContext);
		//}

		#region AddCountry
		// country add request : null
		// throw:  argument null exception
		[Fact]
		public async Task AddCountry_NullCountry_ToBeArgumentNullException()
		{
			CountryAddRequest? request = null;

			Func<Task> action = async () =>
			{
				await _countriesService.AddCountry(request);
			};

			await action.Should().ThrowAsync<ArgumentNullException>();
		}

		// country name : null
		// throw:  argument exception
		[Fact]
		public async Task AddCountry_CountryNameIsNull_ToBeArgumentException()
		{
			CountryAddRequest? request = new CountryAddRequest()
			{
				CountryName = null
			};

			Func<Task> action = async () =>
			{
				await _countriesService.AddCountry(request);
			};

			await action.Should().ThrowAsync<ArgumentException>();
		}

		// country name: duplicate
		// throw:  argument exception
		[Fact]
		public async Task AddCountry_DuplicateCountryName_ToBeArgumentException()
		{
			CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
			Country country = countryAddRequest.ToCountry();

			_countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
				.ReturnsAsync(country);

			Func<Task> action = async () =>
			{
				await _countriesService.AddCountry(countryAddRequest);
			};

			await action.Should().ThrowAsync<ArgumentException>();
		}

		// country name: duplicate
		// throw:  argument exception
		[Fact]
		public async Task AddCountry_ProperCountryDetails_ToBeSuccessful()
		{
			CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
			Country country = countryAddRequest.ToCountry();

			_countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
				.ReturnsAsync(null as Country);
			_countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>()))
				.ReturnsAsync(country);

			CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);

			countryResponse.CountryID.Should().NotBe(Guid.Empty);
		}
		#endregion

		#region GetAllCountries
		// list of countries should be empty by default (before adding any country)
		[Fact]
		public async Task GetAllCountries_ToBeEmptyList()
		{
			_countriesRepositoryMock.Setup(temp => temp.GetAllCountries())
				.ReturnsAsync(new List<Country>());

			List<CountryResponse> countryResponses = await _countriesService.GetAllCountries();

			countryResponses.Should().BeEmpty();
		}

		// list of countries should be empty by default (before adding any country)
		[Fact]
		public async Task GetAllCountries_ToBeSuccessful()
		{
			List<Country> countries = new List<Country>()
			{ 
				_fixture.Build<Country>()
					.With(temp => temp.Persons, null as List<Person>)
					.Create(),
				_fixture.Build<Country>()
					.With(temp => temp.Persons, null as List<Person>)
					.Create(),
				_fixture.Build<Country>()
					.With(temp => temp.Persons, null as List<Person>)
					.Create(),
			};

			List<CountryResponse> countryResponsesExpected = countries.Select(temp => temp.ToCountryResponse()).ToList();

			_countriesRepositoryMock.Setup(temp => temp.GetAllCountries())
				.ReturnsAsync(countries);

			List<CountryResponse> countryResponsesActual = await _countriesService.GetAllCountries();

			countryResponsesActual.Should().BeEquivalentTo(countryResponsesExpected);
		}
		#endregion

		#region GetCountryByCountryId
		// countryId : null
		// return null
		[Fact]
		public async Task GetCountryById_NullCountryId_ToBeNull()
		{
			Guid? countryId = null;

			_countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryId(It.IsAny<Guid>()))
				.ReturnsAsync(null as Country);

			CountryResponse? countryResponse = await _countriesService.GetCountryByCountryId(countryId);

			countryResponse.Should().BeNull();
		}

		// countryId: valid
		// return countryResponse with matching country details
		[Fact]
		public async Task GetCountryById_ValidCountryId_ToBeSuccessful()
		{
			Country country = _fixture.Build<Country>()
				.With(temp => temp.Persons, null as List<Person>)
				.Create();
			CountryResponse countryResponseExpected = country.ToCountryResponse();
			Guid? countryId = country.CountryID;

			_countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryId(It.IsAny<Guid>()))
				.ReturnsAsync(country);

			CountryResponse? countryGetResponse = await _countriesService.GetCountryByCountryId(countryId);

			countryGetResponse.Should().Be(countryResponseExpected);
		}
		#endregion
	}
}
