namespace ExchangeRateService
{
	public class ExchangeRatePair
	{
		public string BaseCurrency { get; set; }
		public string TargetCurrency { get; set; }
		public decimal ExchangeRate { get; set; }
		public string Timestamp { get; set; }
	}
}
