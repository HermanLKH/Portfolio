using AutoFixture;
using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;

namespace CRUDTests
{
	// integration testing/ blackbox testing
	public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
	{
		// http client to simulate a browser request
		private readonly HttpClient _client;

		// inject the factory created and use that to create a http client
		public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
		{
			_client = factory.CreateClient();
		}

		#region Index
		[Fact]
		public async Task Index_ToReturnView()
		{
			// act
			HttpResponseMessage response = await _client.GetAsync("Persons/Index");

			// assert
			response.Should().BeSuccessful();

			string responseBody = await response.Content.ReadAsStringAsync();
			HtmlDocument html = new HtmlDocument();
			// generate html dom based on response body
			html.LoadHtml(responseBody);

			var document = html.DocumentNode;
			document.QuerySelectorAll("table.persons").Should().NotBeNull();
		}
		#endregion
	}
}
