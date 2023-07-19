using DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDExample.Controllers
{
	// route token -> take the controller/ action method name as route
    // [Route("[controller]")]
    // set common prefix route for all the routes of every action method in this controller
    [Route("persons")]
	public class PersonsController : Controller
	{
		// private fields
		private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;

        // constructor
        public PersonsController(IPersonsService personService, ICountriesService countriesService)
        {
            _personService = personService;
			_countriesService = countriesService;
        }

		[Route("/")]
        [Route("[action]")]
        public async Task<IActionResult> Index(string searchBy, string? searchString, 
			string sortBy = nameof(PersonResponse.Name), 
			SortOrderOptions sortOrder = SortOrderOptions.ASC)
		{
			//logger
			//_logger.LogInformation("Index action method of PersonsController");
			//_logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortBy: {sortBy}, " +
			//	$"sortOrder: {sortOrder}");

			// populate select options of searchBy
			ViewBag.SearchFields = new Dictionary<string, string>()
			{
				{ nameof(PersonResponse.Name), "Person Name" },
				{ nameof(PersonResponse.Email), "Email" },
				{ nameof(PersonResponse.DateOfBirth), "Date of Birth" },
				{ nameof(PersonResponse.Gender), "Gender" },
				{ nameof(PersonResponse.CountryID), "Country ID" },
				{ nameof(PersonResponse.Address), "Address" }
			};
			// fetch filtered/ all persons
			List<PersonResponse> persons = await _personService.GetFilteredPersons(searchBy, searchString);

			// persist the search fields
			ViewBag.CurrentSearchBy = searchBy;
			ViewBag.CurrentSearchString = searchString;

			// sort persons
			persons = await _personService.GetSortedPersons(persons, sortBy, sortOrder);

			// persist the search fields
			ViewBag.CurrentSortBy = sortBy;
			ViewBag.CurrentSortOrder = sortOrder;

			return View(persons); // Views/Persons/Index.cshtml
		}

		// create person view
		[Route("create")]
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			List<CountryResponse> countries = await _countriesService.GetAllCountries();
			ViewBag.Countries = countries.Select(c =>
				new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() }
			);

			return View(); 
		}

        // create person
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
			if(!ModelState.IsValid)
			{
				ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

				List<CountryResponse> countries = await _countriesService.GetAllCountries();
				ViewBag.Countries = countries.Select(c =>
					new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() }
				);

				return View(personAddRequest);
            }

			PersonResponse personResponse = await _personService.AddPerson(personAddRequest);

            return RedirectToAction("Index", "Persons");
        }

		// edit person
		[Route("[action]/{personID}")]
		[HttpGet]
		public async Task<IActionResult> Edit(Guid personID)
		{
			PersonResponse? personResponse = await _personService.GetPersonByPersonID(personID);

			// if person is null(person id invalid), redirect to homepage
			if(personResponse == null)
			{
				return RedirectToAction("Index", "Persons");
			}

			// if valid person is fetched
			PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

			List<CountryResponse> countries = await _countriesService.GetAllCountries();
			ViewBag.Countries = countries.Select(c =>
				new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() }
			);

			return View(personUpdateRequest);
		}

		[HttpPost]
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
		{
			// to determine if person id is valid
			PersonResponse? getPersonResponse = await _personService.GetPersonByPersonID(personUpdateRequest.PersonID);

			if(getPersonResponse == null)
			{
				return RedirectToAction("Index", "Persons");
			}

			if(ModelState.IsValid)
			{
				PersonResponse updatePersonResponse = await _personService.UpdatePerson(personUpdateRequest);
				
				return RedirectToAction("Index", "Persons");
			}
			else
			{
				// server side validation
				ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

				List<CountryResponse> countries = await _countriesService.GetAllCountries();
				ViewBag.Countries = countries.Select(c =>
					new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() }
				);

				return View(getPersonResponse.ToPersonUpdateRequest());
			}
		}

        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid personID)
		{
			PersonResponse? personResponse = await _personService.GetPersonByPersonID(personID);

			// if person is null(person id invalid), redirect to homepage
			if (personResponse == null)
			{
				return RedirectToAction("Index", "Persons");
			}
			return View(personResponse);
		}

		[HttpPost]
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
		{
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personUpdateRequest.PersonID);

            // if person is null(person id invalid), redirect to homepage
            if (personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }

            bool isDelete = await _personService.DeletePerson(personUpdateRequest.PersonID);

			if(isDelete)
			{
				return RedirectToAction("Index", "Persons");
			}
			return View();
		}

		[HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> PersonsPDF()
		{
			List<PersonResponse> personResponses = await _personService.GetAllPersons();

			return new ViewAsPdf("PersonsPDF", personResponses, ViewData)
			{
				PageMargins = new Rotativa.AspNetCore.Options.Margins()
				{
					Top = 20, Right = 20, Bottom = 20, Left = 20
				},
				PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
			};
		}

		[HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> PersonsCSV()
		{
			MemoryStream memoryStream = await _personService.GetPersonsCSV();
			return File(memoryStream, "application/octet-stream", "persons.csv");
		}

		[HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> PersonsExcel()
		{
			MemoryStream memoryStream = await _personService.GetPersonsExcel();
			return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
				"persons.xlsx");
		}
	}
}
