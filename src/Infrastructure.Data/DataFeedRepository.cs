using ExchangeRateService.Domain.Entities;
using ExchangeRateService.Domain.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateService.Infrastructure.Data
{
	public class DataFeedRepository : IDataFeedRepository
	{
		const string FilePrefix = "ExchangeRate_";
		const string FileExtension = ".json";
		const string TempFileExtension = ".tmp";
		readonly IDataFeedRepositoryConfiguration configuration;
		readonly IDataFeedMapper mapper;

		public DataFeedRepository(IDataFeedRepositoryConfiguration configuration, IDataFeedMapper mapper)
		{
			this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task SaveAsync(DataFeed dataFeed)
		{
			var text = mapper.Map(dataFeed);
			string fileName = $"{FilePrefix}{dataFeed.GetDataFeedDateTime():yyyyMMddHHmmss}";
			var tmpFilePath = Path.Combine(configuration.StorageFolderPath, $"{fileName}{TempFileExtension}");

			if (!Directory.Exists(configuration.StorageFolderPath))
			{
				Directory.CreateDirectory(configuration.StorageFolderPath);
			}

			var filePath = Path.Combine(configuration.StorageFolderPath, $"{fileName}{FileExtension}");

			if (File.Exists(filePath))
			{
				return;
			}

			using (var writer = new StreamWriter(tmpFilePath))
			{
				await writer.WriteAsync(text);
			}

			File.Move(tmpFilePath, filePath);
		}

		public async Task<DataFeed> LoadAsync()
		{
			var filePaths = Directory.GetFiles(configuration.StorageFolderPath).ToArray();

			var latestFileName = filePaths.Select(Path.GetFileName)
							.Where(f => f.EndsWith(FileExtension) && f.StartsWith(FilePrefix))
							.OrderByDescending(f => f)
							.FirstOrDefault();

			if (latestFileName == null)
			{
				return null;
			}

			var filePath = filePaths.First(f => f.EndsWith(latestFileName));

			var text = await File.ReadAllTextAsync(filePath);
			return mapper.Map(text);
		}
	}
}
