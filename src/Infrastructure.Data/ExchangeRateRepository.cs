using ExchangeRateService.Domain.Entities;
using ExchangeRateService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateService.Infrastructure.Data
{
	public class ExchangeRateRepository : IExchangeRateRepository
	{
		readonly string storageFolderPath;

		public ExchangeRateRepository(string storageFolderPath)
		{
			if (string.IsNullOrWhiteSpace(storageFolderPath)) throw new ArgumentNullException(nameof(storageFolderPath));
			this.storageFolderPath = storageFolderPath;
		}

		public async Task SaveAsync(ExchangeRatePair[] pairs)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<ExchangeRatePair>> LoadAsync()
		{
			throw new NotImplementedException();
		}
	}
}
