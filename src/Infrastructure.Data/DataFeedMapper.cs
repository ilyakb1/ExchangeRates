using ExchangeRateService.Domain.Entities;
using ExchangeRateService.Domain.Interfaces;
using ExchangeRateService.Services.Interfaces;
using Newtonsoft.Json;


namespace ExchangeRateService.Infrastructure.Data
{
	public class DataFeedMapper : IDataFeedMapper
	{
		public DataFeed Map(string text)
		{
			return JsonConvert.DeserializeObject<DataFeed>(text);
		}

		public string Map(DataFeed dataFeed)
		{
			return JsonConvert.SerializeObject(dataFeed);
		}
	}
}
