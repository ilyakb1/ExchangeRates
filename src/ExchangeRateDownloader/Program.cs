using ExchangeRateService.Infrastructure.Data;
using ExchangeRateService.Services;
using System;
using System.IO;
using System.Net.Http;

namespace ExchangeRateDownloader
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var configuration = new DataFeedManagerConfiguration()
				{
					ServiceUrl = @"http://data.fixer.io/api/latest?access_key=c2510f9a89d59e5f17e511157b0bcb41",
					StorageFolderPath = Path.Combine(Path.GetTempPath(), "ExchangeRates")
				};

				var mapper = new DataFeedMapper();
				var repository = new DataFeedRepository(configuration, mapper);
				var service = new DataFeedService(repository);

				using (var httpClient = new HttpClient())
				{
					var downloadClient = new HttpDownloadClient(httpClient);
					var manager = new DataFeedManager(configuration, downloadClient, mapper, service);
					manager.DownloadAsync().Wait();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}
	}
}
