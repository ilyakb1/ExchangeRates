using ExchangeRateService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ExchangeRateService.Controllers
{
	//var response = await client.GetAsync("v1/exchange-rate/convert/base/usd/target/aud");

	[Route("v1/exchangeRate")]
	[ApiController]
	public class ExchangeRateController : ControllerBase
	{
		readonly IDataFeedService dataFeedService;

		public ExchangeRateController(IDataFeedService dataFeedService)
		{
			this.dataFeedService = dataFeedService ?? throw new ArgumentNullException(nameof(dataFeedService));
		}

		[HttpGet]
		[Route("convert/baseCurrency/{baseCurrency}/targetCurrency/{targetCurrency}")]
		public async Task<ActionResult<ExchangeRatePair>> Convert(string baseCurrency, string targetCurrency)
		{
			var result = ValidateCurrencyPair(baseCurrency, targetCurrency);
			if (result != string.Empty)
			{
				return BadRequest(result);
			}

			var baseCurrencyNormalized = baseCurrency.ToUpper();
			var targetCurrencyNormalized = targetCurrency.ToUpper();

			var dataFeed = await dataFeedService.LoadAsync();

			if (dataFeed == null)
			{
				return StatusCode((int)HttpStatusCode.PreconditionFailed, "DataFeed is empty");
			}

			if (!dataFeed.Rates.ContainsKey(baseCurrencyNormalized))
			{
				return NotFound($"{baseCurrencyNormalized} currency is not supported");
			}

			if (!dataFeed.Rates.ContainsKey(targetCurrencyNormalized))
			{
				return NotFound($"{targetCurrencyNormalized} currency is not supported");
			}

			decimal exchangeRate = Math.Round(dataFeed.Rates[baseCurrencyNormalized] / dataFeed.Rates[targetCurrencyNormalized], 5);

			var pair = new ExchangeRatePair()
			{
				BaseCurrency = baseCurrencyNormalized,
				TargetCurrency = targetCurrencyNormalized,
				ExchangeRate = exchangeRate,
				Timestamp = dataFeed.GetDataFeedDateTime().ToString("yyyy-MM-ddThh:mm:ss.00Z")
			};
			return pair;
		}

		string ValidateCurrencyPair(string baseCurrency, string targetCurrency)
		{
			var output = new StringWriter();

			if (baseCurrency.Length != 3)
			{
				output.WriteLine("BaseCurrency should be 3 symbols length.");
			}

			if (targetCurrency.Length != 3)
			{
				output.WriteLine("TargetCurrency should be 3 symbols length.");
			}

			if (baseCurrency == targetCurrency)
			{
				output.Write("BaseCurrency should be different than TargetCurrency.");
			}

			return output.ToString();
		}
	}
}