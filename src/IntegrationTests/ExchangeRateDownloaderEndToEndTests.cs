using ExchangeRateDownloader;
using ExchangeRateService.Infrastructure.Data;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.IO;
using System.Threading.Tasks;

namespace ExchangeRateService.IntegrationTests
{
	[TestFixture()]
	public class ExchangeRateDownloaderEndToEndTests
	{
		string storageFolderPath;

		[SetUp]
		public void Init()
		{
			var tempFolderPath = Path.GetTempPath();
			storageFolderPath = Path.Combine(tempFolderPath, "ExchangeRateTests");
		}


		[TearDown]
		public void Dispose()
		{
			if (Directory.Exists(storageFolderPath))
			{
				Directory.Delete(storageFolderPath, true);
			}
		}

		[Test]
		public async Task DownloadNewDataFeed()
		{
			await new Startup().RunAsync();
			var mapper = new DataFeedMapper();
			var configuration = new Mock<IDataFeedRepositoryConfiguration>();
			configuration.Setup(c => c.StorageFolderPath).Returns(Path.Combine(Path.GetTempPath(), "ExchangeRates"));
			var repository = new DataFeedRepository(configuration.Object, mapper);
			var dataFeed = await repository.LoadAsync();
			Assert.IsNotNull(dataFeed, "Should load DataFeed");
		}
	}
}
