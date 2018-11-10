using ExchangeRateService.Infrastructure.Data;

namespace ExchangeRateDownloader
{
	public class DataFeedManagerConfiguration : IDataFeedRepositoryConfiguration
	{
		public string ServiceUrl { get; set; }

		public string StorageFolderPath { get; set; }
	}
}
