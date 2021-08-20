using System.Reflection;
using Flight.AircraftHangar.Filters;
using HotChocolate.Execution;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Flight.AircraftHangar
{
	public class Startup
	{
		#region Public

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

			app.UseRouting();
			app.UseEndpoints(endpoints => { endpoints.MapGraphQL(); });
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMassTransit(cfg =>
			{
				cfg.AddConsumers(Assembly.GetExecutingAssembly());
				cfg.SetKebabCaseEndpointNameFormatter();
				cfg.UsingRabbitMq((context, configurator) => { configurator.ConfigureEndpoints(context); });
			});

			services.AddMassTransitHostedService();

			var builder = services.AddGraphQLServer()
				.AddQueryType<Query>()
				.AddFiltering<CustomFilteringConvention>();
			services.AddScoped(_ => builder.BuildRequestExecutorAsync().GetAwaiter().GetResult());

			services.AddDbContext<AircraftDbContext>(optionsBuilder =>
			{
				optionsBuilder.UseSqlite(@"Data Source=flights.db");
			});
		}

		#endregion
	}
}