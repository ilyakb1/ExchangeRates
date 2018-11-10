using System;
using System.Collections.Generic;

namespace ExchangeRateDownloader
{
	public class DataFeed
	{
		public bool Success { get; set; }
		public int Timestamp { get; set; }
		public string Base { get; set; }
		public Dictionary<string, decimal> Rates { get; set; }

		public DateTime GetDataFeedDateTime()
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var timeSpan = TimeSpan.FromSeconds(Timestamp);
			return epoch.Add(timeSpan).ToLocalTime();
		}
	}
}
