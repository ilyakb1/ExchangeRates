using ExchangeRateService.Services.Interfaces;
using System;
using System.Threading.Tasks;
using ExchangeRateService.Domain.Interfaces;

namespace ExchangeRateDownloader
{
	public class DataFeedManager
	{
		readonly DataFeedManagerConfiguration configuration;
		readonly IDownloadClient client;
		readonly IDataFeedMapper mapper;
		readonly IDataFeedService service;

		public DataFeedManager(
			DataFeedManagerConfiguration configuration,
			IDownloadClient client,
			IDataFeedMapper mapper,
			IDataFeedService service)
		{
			this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.client = client ?? throw new ArgumentNullException(nameof(client));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			this.service = service ?? throw new ArgumentNullException(nameof(service));
		}

		public async Task DownloadAsync()
		{
			var text = await client.DownloadAsync(configuration.ServiceUrl);
			var dataFeed = mapper.Map(text);
			await service.SaveAsync(dataFeed);
		}
	}
}
