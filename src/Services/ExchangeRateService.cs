using ExchangeRateService.Domain.Entities;
using ExchangeRateService.Domain.Interfaces;
using ExchangeRateService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateService.Services
{
	public class ExchangeRateService : IExchangeRateService
	{
		readonly IExchangeRateRepository repository;

		public ExchangeRateService(IExchangeRateRepository repository)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task SaveAsync(ExchangeRatePair[] pairs)
		{
			await repository.SaveAsync(pairs);
		}

		public async Task<IEnumerable<ExchangeRatePair>> LoadAsync()
		{
			return await repository.LoadAsync();
		}
	}
}
