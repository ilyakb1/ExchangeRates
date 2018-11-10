using ExchangeRateService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateService.Services.Interfaces
{
	public interface IExchangeRateService
	{
		Task SaveAsync(ExchangeRatePair[] pairs);

		Task<IEnumerable<ExchangeRatePair>> LoadAsync();
	}
}
