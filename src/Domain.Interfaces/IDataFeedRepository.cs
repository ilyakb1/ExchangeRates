using ExchangeRateService.Domain.Entities;
using System.Threading.Tasks;

namespace ExchangeRateService.Domain.Interfaces
{
	public interface IDataFeedRepository
	{
		Task SaveAsync(DataFeed dataFeed);

		Task<DataFeed> LoadAsync();
	}
}
