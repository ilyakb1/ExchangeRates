using ExchangeRateDownloader;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateService.IntegrationTests
{
	[TestFixture]
	public class ExchangeRateServiceTests
	{
		[Test]
		[Ignore("End to end test. Run service host before execute test")]
		public async Task CallExchangeRateServiceGetRates()
		{
			new Startup().RunAsync().Wait();


			using (var client = new HttpClient())
			{
				var response = await client.GetAsync("https://localhost:5001/v1/exchangerate/convert/basecurrency/usd/targetcurrency/aud");
				response.EnsureSuccessStatusCode();
				var text = response.Content.ReadAsStringAsync();
				Assert.IsNotNull(text);
			}
		}
	}
}
