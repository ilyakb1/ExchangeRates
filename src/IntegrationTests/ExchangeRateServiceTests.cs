using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateService.IntegrationTests
{
	[TestFixture]
	public class ExchangeRateServiceTests
	{
		[Test]
		public async Task CallExchangeRateServiceGetRates()
		{
			using (var client = new HttpClient())
			{
				var response = await client.GetAsync("v1/exchange-rate/convert/base/usd/target/aud");
				response.EnsureSuccessStatusCode();
				var text = response.Content.ReadAsStringAsync();
				Assert.AreEqual("", text);
			}
		}
	}
}
