using CsvHelper;
using CsvHelper.Configuration;
using DbContext;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.Globalization;

namespace Services
{
	public class PersonsService : IPersonsService
	{
		private readonly IPersonsRepository _personsRepository;
		public PersonsService(IPersonsRepository personsRepository)
		{
			_personsRepository = personsRepository;
		}

		public async Task<PersonResponse> AddPerson(PersonAddRequest? request)
		{
			// check if PersonAddRequest not null
			if(request == null)
			{
				throw new ArgumentNullException(nameof(request));
			}

			// model validations
			ValidationHelper.ModelValidation(request);

			// convert PersonRequest into Person
			Person person = request.ToPerson();
			// generate personID
			person.PersonID = Guid.NewGuid();
			// add person object to the person list
			await _personsRepository.AddPerson(person);

			// stored procedure
			//_countriesRepository.sp_InsertPerson(person);
			//_countriesRepository.SaveChanges();

			return person.ToPersonResponse();
		}

		public async Task<List<PersonResponse>> GetAllPersons()
		{
			// _countriesService.Persons.ToList() => sql select query
			// self-defined method cannot work with dbcontext method, cannot translate to sql
			//return _countriesService.Persons.ToList().Select(p => ConvertPersonToPersonResponse(p)).ToList();

			// use stored procedure
			//return _countriesService.sp_GetAllPersons().Select(p => ConvertPersonToPersonResponse(p)).ToList();

			// table relation using EF
			//var persons = await _personsService.Persons.Include("Country").ToListAsync();
			
			return (await _personsRepository.GetAllPersons())
				.Select(temp => temp.ToPersonResponse()).ToList();
		}

		public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
		{
			if(personID == null)
			{
				return null;
			}

			Person? person = await _personsRepository.GetPersonByPersonID(personID.Value);

			if(person == null)
			{
				return null;
			}
			else
			{
				return person.ToPersonResponse();
			}
		}

		public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
		{
			List<Person> filteredPersons = searchBy switch
			{
				nameof(PersonResponse.Name) =>
					await _personsRepository.GetFilteredPersons(
						temp => temp.Name.Contains(searchString)),

				nameof(PersonResponse.Email) =>
					await _personsRepository.GetFilteredPersons(
						temp => temp.Email.Contains(searchString)),

				nameof(PersonResponse.DateOfBirth) =>
					await _personsRepository.GetFilteredPersons(
						temp => temp.DateOfBirth.Value.ToString("dd MMMM yyyy")
									.Contains(searchString)),

				nameof(PersonResponse.Gender) =>
					await _personsRepository.GetFilteredPersons(
						temp => temp.Gender.Contains(searchString)),

				nameof(PersonResponse.CountryID) =>
					await _personsRepository.GetFilteredPersons(
						temp => temp.Country.CountryName.Contains(searchString)),

				nameof(PersonResponse.Address) =>
					await _personsRepository.GetFilteredPersons(
						temp => temp.Address.Contains(searchString)),

				_ => await _personsRepository.GetAllPersons()
			};

			return filteredPersons.Select(temp => temp.ToPersonResponse()).ToList();
		}

		public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> personResponses, string sortBy, SortOrderOptions sortOrder)
		{
			if (string.IsNullOrEmpty(sortBy))
			{
				return personResponses;
			}
			List<PersonResponse> sortedPersonResponses = (sortBy, sortOrder)
			switch
			{
				(nameof(PersonResponse.Name), SortOrderOptions.ASC) => 
				personResponses.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Name), SortOrderOptions.DESC) => 
				personResponses.OrderByDescending(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), SortOrderOptions.ASC) =>
				personResponses.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), SortOrderOptions.DESC) =>
				personResponses.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) =>
				personResponses.OrderBy(p => p.DateOfBirth.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) =>
				personResponses.OrderByDescending(p => p.DateOfBirth.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
				
				(nameof(PersonResponse.Age), SortOrderOptions.ASC) =>
				personResponses.OrderBy(p => p.Age).ToList(),

				(nameof(PersonResponse.Age), SortOrderOptions.DESC) =>
				personResponses.OrderByDescending(p => p.Age).ToList(),
				
				(nameof(PersonResponse.Gender), SortOrderOptions.ASC) =>
				personResponses.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Gender), SortOrderOptions.DESC) =>
				personResponses.OrderByDescending(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Country), SortOrderOptions.ASC) =>
				personResponses.OrderBy(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Country), SortOrderOptions.DESC) =>
				personResponses.OrderByDescending(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Address), SortOrderOptions.ASC) =>
				personResponses.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Address), SortOrderOptions.DESC) =>
				personResponses.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.ASC) =>
				personResponses.OrderBy(p => p.ReceiveNewsLetter).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.DESC) =>
				personResponses.OrderByDescending(p => p.ReceiveNewsLetter).ToList(),

				// _ => default case
				_ => personResponses
			};
			return sortedPersonResponses;
		}

		public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
		{
			if(personUpdateRequest == null)
			{
				throw new ArgumentNullException(nameof(personUpdateRequest));
			}

			// model validation
			ValidationHelper.ModelValidation(personUpdateRequest);

			// get matching person object to update
			Person? matchingPerson = await _personsRepository.GetPersonByPersonID(personUpdateRequest.PersonID);

			// no matching person
            if (matchingPerson == null)
            {
				throw new ArgumentException("Invalid person id");
            }
			// update person details
			matchingPerson.Name = personUpdateRequest.Name;
			matchingPerson.Email = personUpdateRequest.Email;
			matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
			matchingPerson.Gender = personUpdateRequest.Gender.ToString();
			matchingPerson.CountryID = personUpdateRequest.CountryID;
			matchingPerson.Address = personUpdateRequest.Address;
			matchingPerson.ReceiveNewsLetter = personUpdateRequest.ReceiveNewsLetter;

			await _personsRepository.UpdatePerson(matchingPerson);

			return matchingPerson.ToPersonResponse();
		}

		public async Task<bool> DeletePerson(Guid? personID)
		{
			if(personID  == null)
			{
				throw new ArgumentNullException(nameof(personID));
			}
			Person? matchingPerson = await _personsRepository.GetPersonByPersonID(personID.Value);

			if(matchingPerson == null)
			{
				return false;
			}
			await _personsRepository.DeletePersonByPersonID(personID.Value);
			return true;
		}

		public async Task<MemoryStream> GetPersonsCSV()
		{
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream);

			CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
			CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);


			// write headers, eg: PersonID, Name, Email...
			//csvWriter.WriteHeader<PersonResponse>();
			// write specific field, eg: Name...
			csvWriter.WriteField(nameof(PersonResponse.Name));
			csvWriter.WriteField(nameof(PersonResponse.Email));
			csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
			csvWriter.WriteField(nameof(PersonResponse.Age));
			csvWriter.WriteField(nameof(PersonResponse.Gender));
			csvWriter.WriteField(nameof(PersonResponse.Country));
			csvWriter.WriteField(nameof(PersonResponse.Address));
			csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetter));

			// next row
			await csvWriter.NextRecordAsync();

			List<PersonResponse> personResponses = await GetAllPersons();
			// write all records data, eg: 1, "Example Name", "example@gmail.com"...
			//await csvWriter.WriteRecordsAsync(personResponses);

			// loop each person response to write its field data
			foreach (PersonResponse personResponse in personResponses)
            {
				csvWriter.WriteField(personResponse.Name);
				csvWriter.WriteField(personResponse.Email);
				if (personResponse.DateOfBirth.HasValue)
				{
					csvWriter.WriteField(personResponse.DateOfBirth.Value.ToString("yyyy-MM-dd"));
				}
				else
				{
					csvWriter.WriteField("");
				}
				csvWriter.WriteField(personResponse.Age);
				csvWriter.WriteField(personResponse.Gender);
				csvWriter.WriteField(personResponse.Country);
				csvWriter.WriteField(personResponse.Address);
				csvWriter.WriteField(personResponse.ReceiveNewsLetter);

				await csvWriter.NextRecordAsync();
				// add the data writen to the memory stream
				await csvWriter.FlushAsync();
			}

            // after writting records, has to relocate the cursor to the beginning
            memoryStream.Position = 0;
			return memoryStream;
		}

		public async Task<MemoryStream> GetPersonsExcel()
		{
			MemoryStream memoryStream = new MemoryStream();

			// to create excel workbook and worksheet
			using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
			{
				ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
				worksheet.Cells["A1"].Value = "Name";
				worksheet.Cells["B1"].Value = "Email";
				worksheet.Cells["C1"].Value = "Date of Birth";
				worksheet.Cells["D1"].Value = "Age";
				worksheet.Cells["E1"].Value = "Gender";
				worksheet.Cells["F1"].Value = "Country";
				worksheet.Cells["G1"].Value = "Address";
				worksheet.Cells["H1"].Value = "Receive News Letter";

				using (ExcelRange headerCells = worksheet.Cells["A1:H1"])
				{
					headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
					headerCells.Style.Font.Bold = true;
				}

				int row = 2;
				List<PersonResponse> personResponses = await GetAllPersons();

                foreach (PersonResponse personResponse in personResponses)
                {
					worksheet.Cells[row, 1].Value = personResponse.Name;
					worksheet.Cells[row, 2].Value = personResponse.Email;
					if(personResponse.DateOfBirth != null)
					{
						worksheet.Cells[row, 3].Value = personResponse.DateOfBirth.Value.ToString("yyyy-MM-dd");
					}
					worksheet.Cells[row, 4].Value = personResponse.Age;
					worksheet.Cells[row, 5].Value = personResponse.Gender;
					worksheet.Cells[row, 6].Value = personResponse.Country;
					worksheet.Cells[row, 7].Value = personResponse.Address;
					worksheet.Cells[row, 8].Value = personResponse.ReceiveNewsLetter;

					row += 1;
                }

				// automatically adjust column width based on the content of the cells
				worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

				// save the written content into the memory stream
				await excelPackage.SaveAsync();
            }
			memoryStream.Position = 0;
			return memoryStream;
		}
	}
}
