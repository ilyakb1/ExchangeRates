using ExchangeRateService.Domain.Entities;
using ExchangeRateService.Services;
using ExchangeRateService.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateService
{
	public class DataFeedServiceWithCache : IDataFeedService
	{
		readonly DataFeedService dataFeedService;
		readonly IMemoryCache memoryCache;
		readonly ExchangeRateServiceConfiguration configuration;
		const string CacheKey = "DataFeed";
		static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

		public DataFeedServiceWithCache(
			DataFeedService dataFeedService,
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
			return await memoryCache.GetOrCreateAsync<DataFeed>(CacheKey, async cacheEntry =>
			{
				await semaphoreSlim.WaitAsync();
				try
				{
					cacheEntry.AbsoluteExpirationRelativeToNow = configuration.CacheDuration;
					return await dataFeedService.LoadAsync();
				}
				finally
				{
					semaphoreSlim.Release();
				}
			});
		}
	}
}
