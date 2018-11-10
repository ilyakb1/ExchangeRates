using ExchangeRateService.Domain.Entities;

namespace ExchangeRateService.Domain.Interfaces
{
	public interface IDataFeedMapper
	{
		DataFeed Map(string text);

		string Map(DataFeed dataFeed);
	}
}