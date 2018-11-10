using System;

namespace ExchangeRateService.Domain.Entities
{
	public class ExchangeRatePair
	{
		public ExchangeRatePair(string baseCurrency, string targetCurrency, string exchangeRate, DateTime timestampUtc)
		{
			BaseCurrency = baseCurrency ?? throw new ArgumentNullException(nameof(baseCurrency));
			TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));
			ExchangeRate = exchangeRate ?? throw new ArgumentNullException(nameof(exchangeRate));
			TimestampUtc = timestampUtc;
		}

		public string BaseCurrency { get; }
		public string TargetCurrency { get; }
		public string ExchangeRate { get; }
		public DateTime TimestampUtc { get; }
	}
}
