using ExchangeRateService.Infrastructure.Data;
using System;

namespace ExchangeRateService
{
	public class ExchangeRateServiceConfiguration : IDataFeedRepositoryConfiguration
	{
		public string StorageFolderPath { get; set; }

		public TimeSpan CacheDuration { get; set; }
	}
}
