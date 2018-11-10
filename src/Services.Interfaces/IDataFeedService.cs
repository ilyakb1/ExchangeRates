using ExchangeRateService.Domain.Entities;
using System.Threading.Tasks;

namespace ExchangeRateService.Services.Interfaces
{
	public interface IDataFeedService
	{
		Task SaveAsync(DataFeed dataFeed);

		Task<DataFeed> LoadAsync();
	}
}
