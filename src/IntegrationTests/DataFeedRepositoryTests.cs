using ExchangeRateService.Domain.Entities;
using ExchangeRateService.Infrastructure.Data;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateService.IntegrationTests
{
	[TestFixture()]
	public class DataFeedRepositoryTests
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
		public async Task SaveDataFeed()
		{
			var mapper = new DataFeedMapper();
			var configuration = new Mock<IDataFeedRepositoryConfiguration>();
			configuration.Setup(c => c.StorageFolderPath).Returns(storageFolderPath);
			var repository = new DataFeedRepository(configuration.Object, mapper);

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

			await repository.SaveAsync(dataFeed);

			var fileName = Directory.GetFiles(storageFolderPath).FirstOrDefault(f => f.EndsWith("ExchangeRate_20181110140906.json"));
			Assert.IsNotNull(fileName);
			Assert.IsTrue(new FileInfo(fileName).Length > 0, "File size is zero");
		}

		[Test]
		public async Task ReadLatestDataFeed()
		{
			var mapper = new DataFeedMapper();
			var configuration = new Mock<IDataFeedRepositoryConfiguration>();
			configuration.Setup(c => c.StorageFolderPath).Returns(storageFolderPath);
			var repository = new DataFeedRepository(configuration.Object, mapper);

			var dataFeed1 = new DataFeed()
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

			await repository.SaveAsync(dataFeed1);

			var dataFeed2 = new DataFeed()
			{
				Base = "EUR",
				Success = true,
				Timestamp = 1541819446,
				Rates = new Dictionary<string, decimal>()
				{
					{"ETB", 31.813406m},
					{"EUR", 1m},
					{"GBP", 0.873592m}
				}
			};

			await repository.SaveAsync(dataFeed2);

			var dataFeed = await repository.LoadAsync();
			Assert.AreEqual(dataFeed2.Timestamp, dataFeed.Timestamp, "Should load latest DataFeed");
		}
	}
}
