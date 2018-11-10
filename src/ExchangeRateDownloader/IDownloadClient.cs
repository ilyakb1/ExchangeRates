using System.Threading.Tasks;

namespace ExchangeRateDownloader
{
	public interface IDownloadClient
	{
		Task<string> DownloadAsync(string url);
	}
}
