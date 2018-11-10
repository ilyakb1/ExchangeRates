using ExchangeRateDownloader;
using ExchangeRateService.Domain.Entities;
using ExchangeRateService.Domain.Interfaces;
using ExchangeRateService.Infrastructure.Data;
using ExchangeRateService.Services;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.IO;
using System.Threading.Tasks;

namespace ExchangeRateService.UnitTests
{
	[TestFixture()]
	public class DataFeedManagerTests
	{
		[Test]
		public async Task DownloadNewDataFeed()
		{
			var text = TestHelper.GetEmbeddedResourceAsString("TestFiles.DataFeedSmall.json");

			var configuration = new DataFeedManagerConfiguration()
			{
				ServiceUrl = @"http://data.fixer.io/api/latest?access_key=c2510f9a89d59e5f17e511157b0bcb41",
				StorageFolderPath = Path.Combine(Path.GetTempPath(), "ExchangeRates")
			};

			var mapper = new DataFeedMapper();
			var repository = new Mock<IDataFeedRepository>(MockBehavior.Strict);

			repository.Setup(r => r.SaveAsync(It.IsAny<DataFeed>())).Callback((DataFeed dataFeed) =>
			{
				Assert.AreEqual(1541812746, dataFeed.Timestamp);
			}).Returns(Task.Delay(1));

			var service = new DataFeedService(repository.Object);

			var downloadClient = new Mock<IDownloadClient>(MockBehavior.Strict);

			downloadClient.Setup(c => c.DownloadAsync(configuration.ServiceUrl)).ReturnsAsync(text);

			var manager = new DataFeedManager(configuration, downloadClient.Object, mapper, service);
			await manager.DownloadAsync();
		}

	}
}
