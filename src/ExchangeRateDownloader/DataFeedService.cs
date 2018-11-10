using System;
using System.Threading.Tasks;

namespace ExchangeRateDownloader
{
	public class DataFeedService
	{
		readonly DataFeedServiceConfiguration configuration;
		readonly IDownloadClient client;

		public DataFeedService(DataFeedServiceConfiguration configuration, IDownloadClient client)
		{
			this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task DownloadAsync()
		{
			var text = await client.DownloadAsync(configuration.ServiceUrl);
		}
	}
}
