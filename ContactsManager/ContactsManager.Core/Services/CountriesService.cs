using DbContext;
using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
	public class CountriesService : ICountriesService
	{
		// inject persons db context
		private readonly ICountriesRepository _countriesRepository;
		public CountriesService(ICountriesRepository countriesRepository)
		{
			_countriesRepository = countriesRepository;
        }

		public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
		{
			if(countryAddRequest == null)
			{
				throw new ArgumentNullException(nameof(countryAddRequest));
			}
			if(countryAddRequest.CountryName == null)
			{
				throw new ArgumentException(nameof(countryAddRequest.CountryName));
			}
			if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
			{
				throw new ArgumentException("Given country name already exists");
			}
			// convert request DTO to model object
			Country country = countryAddRequest.ToCountry();

			// use model object to perform database operation
			country.CountryID = Guid.NewGuid();
			await _countriesRepository.AddCountry(country);

			// convert model object to response DTO
			return country.ToCountryResponse();
		}

		public async Task<List<CountryResponse>> GetAllCountries()
		{
			return (await _countriesRepository.GetAllCountries())
				.Select(c => c.ToCountryResponse())
				.ToList();
		}

		public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
		{
			if(countryId != null)
			{
				Country? country = await _countriesRepository.GetCountryByCountryId(countryId.Value);

				if (country != null)
				{
					return country.ToCountryResponse();
				}
			}
			return null;
		}

		public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
		{
			MemoryStream memoryStream = new MemoryStream();
			// adds the data from the file into the memory stream
			await formFile.CopyToAsync(memoryStream);
			int countriesInserted = 0;

			using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
			{
				ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Countries"];

				int rowCount = worksheet.Dimension.Rows;

				for(int row = 2; row <= rowCount; row++)
				{
					string? countryName = Convert.ToString(worksheet.Cells[row, 1].Value);

					if (!string.IsNullOrEmpty(countryName))
					{
						if (_countriesRepository.GetCountryByCountryName(countryName) == null)
						{
							Country country = new Country() { CountryName = countryName };
							await _countriesRepository.AddCountry(country);
							countriesInserted += 1;
						}
						else
						{
							throw new ArgumentException("Country name cannot be duplicated");
						}
					}
					else
					{
						throw new ArgumentNullException("Country name cannot be null or empty");
					}
				}
			}
			return countriesInserted;
		}
	}
}