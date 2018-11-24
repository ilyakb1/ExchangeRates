using ExchangeRateService.Domain.Entities;
using ExchangeRateService.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace ExchangeRateService
{
	public class DataFeedServiceWithCache : IDataFeedService
	{
		readonly IDataFeedService dataFeedService;
		readonly IMemoryCache memoryCache;
		readonly ExchangeRateServiceConfiguration configuration;
		const string CacheKey = "DataFeed";

		public DataFeedServiceWithCache(
			IDataFeedService dataFeedService,
			IMemoryCache memoryCache,
			ExchangeRateServiceConfiguration configuration)
		{
			this.dataFeedService = dataFeedService ?? throw new ArgumentNullException(nameof(dataFeedService));
			this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
			this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public async Task SaveAsync(DataFeed dataFeed)
		{
			await dataFeedService.SaveAsync(dataFeed);
		}

		public async Task<DataFeed> LoadAsync()
		{
			return await memoryCache.GetOrCreateAsync<DataFeed>(CacheKey, cacheEntry =>
			{
				cacheEntry.AbsoluteExpirationRelativeToNow = configuration.CacheDuration;
				return dataFeedService.LoadAsync();
			});
		}
	}
}
