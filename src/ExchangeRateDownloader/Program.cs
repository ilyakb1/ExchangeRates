using System;

namespace ExchangeRateDownloader
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				new Startup().RunAsync().Wait();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}
	}
}
