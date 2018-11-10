using Newtonsoft.Json;

namespace ExchangeRateDownloader
{
	public class DataFeedMapper
	{
		public DataFeed Map(string text)
		{
			return JsonConvert.DeserializeObject<DataFeed>(text);
		}
	}
}
