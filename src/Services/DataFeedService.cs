using ExchangeRateService.Domain.Entities;
using ExchangeRateService.Domain.Interfaces;
using ExchangeRateService.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ExchangeRateService.Services
{
	public class DataFeedService : IDataFeedService
	{
		readonly IDataFeedRepository repository;

		public DataFeedService(IDataFeedRepository repository)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task SaveAsync(DataFeed dataFeed)
		{
			await repository.SaveAsync(dataFeed);
		}

		public async Task<DataFeed> LoadAsync()
		{
			return await repository.LoadAsync();
		}
	}
}
