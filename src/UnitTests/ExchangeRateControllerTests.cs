using ExchangeRateService.Controllers;
using ExchangeRateService.Domain.Entities;
using ExchangeRateService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ExchangeRateService.UnitTests
{
	[TestFixture()]
	public class ExchangeRateControllerTests
	{
		[Test]
		public async Task ConvertSuccess()
		{
			var service = new Mock<IDataFeedService>(MockBehavior.Strict);

			var dataFeed = new DataFeed()
			{
				Base = "EUR",
				Success = true,
				Timestamp = 1541819346,
				Rates = new Dictionary<string, decimal>()
				{
					{"ETB", 31.813406m},
					{"EUR", 1m},
					{"GBP", 0.873592m}
				}
			};

			service.Setup(s => s.LoadAsync()).ReturnsAsync(dataFeed);
			var controller = new ExchangeRateController(service.Object);
			var result = await controller.Convert("GBP", "EUR");

			Assert.IsInstanceOf<ActionResult<ExchangeRatePair>>(result);

			Assert.IsNotNull(result.Value, "ExchangeRatePair");
			Assert.AreEqual("GBP", result.Value.BaseCurrency);
			Assert.AreEqual("EUR", result.Value.TargetCurrency);
			Assert.AreEqual(0.87359m, result.Value.ExchangeRate);
			Assert.AreEqual("2018-11-10T02:09:06.00Z", result.Value.Timestamp);

		}

		[Test]
		public async Task ConvertErrorCurrencyIsNot3SymbolsInLength()
		{
			var service = new Mock<IDataFeedService>(MockBehavior.Strict);

			var controller = new ExchangeRateController(service.Object);
			var result = await controller.Convert("GB", "EU");

			Assert.IsInstanceOf<ActionResult<ExchangeRatePair>>(result);
			Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

			Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)result.Result).StatusCode);

			Assert.IsNull(result.Value);
		}

		[Test]
		public async Task ConvertErrorBaseAndTargetCurrencyIsTheSame()
		{
			var service = new Mock<IDataFeedService>(MockBehavior.Strict);

			var controller = new ExchangeRateController(service.Object);
			var result = await controller.Convert("GBP", "GBP");

			Assert.IsInstanceOf<ActionResult<ExchangeRatePair>>(result);
			Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

			Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)result.Result).StatusCode);
			Assert.AreEqual("BaseCurrency should be different than TargetCurrency.", ((BadRequestObjectResult)result.Result).Value);

			Assert.IsNull(result.Value);
		}

		[Test]
		public async Task ConvertErrorDataFeedNotLoaded()
		{
			var service = new Mock<IDataFeedService>(MockBehavior.Strict);
			service.Setup(s => s.LoadAsync()).ReturnsAsync((DataFeed)null);

			var controller = new ExchangeRateController(service.Object);
			var result = await controller.Convert("GBP", "EUR");

			Assert.IsInstanceOf<ActionResult<ExchangeRatePair>>(result);
			Assert.IsInstanceOf<ObjectResult>(result.Result);

			Assert.AreEqual((int)HttpStatusCode.PreconditionFailed, ((ObjectResult)result.Result).StatusCode);
			Assert.AreEqual("DataFeed is empty", ((ObjectResult)result.Result).Value);

			Assert.IsNull(result.Value);
		}
	}
}
