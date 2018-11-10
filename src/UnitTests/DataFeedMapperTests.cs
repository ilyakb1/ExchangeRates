using ExchangeRateDownloader;
using NUnit.Framework;
using System;
using System.Linq;

namespace ExchangeRateService.UnitTests
{
	[TestFixture]
	public class DataFeedMapperTests
	{
		[Test]
		public void MapCorrectDataFeed()
		{
			var text = TestHelper.GetEmbeddedResourceAsString("TestFiles.DataFeedSmall.json");
			var mapper = new DataFeedMapper();
			var dataFeed = mapper.Map(text);
			Assert.IsTrue(dataFeed.Success, "Status");
			Assert.AreEqual("EUR", dataFeed.Base);
			Assert.AreEqual(new DateTime(2018, 11, 10, 12, 19, 06), dataFeed.GetDataFeedDateTime());

			Assert.AreEqual(3, dataFeed.Rates.Count);
			Assert.AreEqual("ETB", dataFeed.Rates.Keys.First());
			Assert.AreEqual(31.813406m, dataFeed.Rates[dataFeed.Rates.Keys.First()]);
		}
	}
}
