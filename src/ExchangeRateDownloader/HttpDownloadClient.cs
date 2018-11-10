using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateDownloader
{
	public class HttpDownloadClient : IDownloadClient
	{
		readonly HttpClient client;

		public HttpDownloadClient(HttpClient client)
		{
			this.client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<string> DownloadAsync(string url)
		{
			return await client.GetStringAsync(url);
		}
	}
}
