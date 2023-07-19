using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace CRUDExample.Controllers
{
	[Route("[controller]")]
	public class CountriesController : Controller
	{
		private readonly ICountriesService _countriesService;
		public CountriesController(ICountriesService countriesService)
		{
			_countriesService = countriesService;
		}

		[HttpGet]
		[Route("[action]")]
		public IActionResult UploadFromExcel()
		{
			return View();
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
		{
			// to ensure file is selected and not empty
			if(excelFile == null || excelFile.Length == 0)
			{
				ViewBag.ErrorMessage = "Please select an excel file";
				return View();
			}
			// check file extension
			else if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
			{
				ViewBag.ErrorMessage = "Unsupported file type";
				return View();
			}
			else
			{
				try
				{
					int countriesInserted = await _countriesService.UploadCountriesFromExcelFile(excelFile);
					ViewBag.Message = $"{countriesInserted} countries uploaded successful";
				}
				catch(Exception e)
				{
					ViewBag.ErrorMessage = e.Message;
				}
				return View();
			}
		}
	}
}
