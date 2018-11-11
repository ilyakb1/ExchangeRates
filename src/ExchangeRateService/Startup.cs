using ExchangeRateService.Domain.Interfaces;
using ExchangeRateService.Infrastructure.Data;
using ExchangeRateService.Services;
using ExchangeRateService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace ExchangeRateService
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "Exchange Rate Api", Version = "v1" });
			});

			var configuration = new ExchangeRateServiceConfiguration()
			{
				StorageFolderPath = Path.Combine(Path.GetTempPath(), "ExchangeRates"),
				CacheDuration = new TimeSpan(0, 0, 5, 0)
			};

			services.AddSingleton<IDataFeedRepositoryConfiguration>(configuration);
			services.AddSingleton(configuration);
			services.AddTransient<IDataFeedRepository, DataFeedRepository>();
			services.AddTransient<IDataFeedMapper, DataFeedMapper>();
			services.AddTransient<IDataFeedService, DataFeedServiceWithCache>();
			services.AddTransient<DataFeedService, DataFeedService>();

			services.AddMemoryCache();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Rate Api");
			});

			app.UseMvc();
		}
	}
}
