using ExchangeRateService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateService.Domain.Interfaces
{
	public interface IExchangeRateRepository
	{
		Task SaveAsync(ExchangeRatePair[] pairs);

		Task<IEnumerable<ExchangeRatePair>> LoadAsync();
	}
}
