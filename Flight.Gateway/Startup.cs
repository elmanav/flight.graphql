using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Flight.Gateway
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<Startup>>();
            
            var servicesConfig = _configuration.GetSection("Services").GetChildren()
                .Select(section => new { ServiceName = section.Key, ServiceUrl = section.Value }).ToList();
            foreach (var serviceConfig in servicesConfig)
                services.AddHttpClient(serviceConfig.ServiceName,
                    client => client.BaseAddress = new Uri(serviceConfig.ServiceUrl));

            var builder = services.AddGraphQLServer()
                .AddQueryType(descriptor => descriptor.Name("Query"))
                .AddTypeExtensionsFromFile("./Stitching.graphql");
            foreach (var serviceConfig in servicesConfig)
            {
                builder.AddRemoteSchema(serviceConfig.ServiceName, true);
                logger.LogInformation("Adding remote schema: {ServiceName}", serviceConfig.ServiceName);
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapGraphQL(); });
        }
    }
}