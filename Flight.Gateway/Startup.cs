using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Flight.Gateway
{
    public class Startup
    {
        private const string AircraftHangarSchema = "aircraftHangar";
        private const string AirportsSchema = "airports";
        private const string FlightsSchema = "flights";
        private const string AirlinesSchema = "airlines";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient(AircraftHangarSchema,
                client => client.BaseAddress = new Uri("http://localhost:5051/graphql"));
            services.AddHttpClient(AirportsSchema,
                client => client.BaseAddress = new Uri("http://localhost:5052/graphql"));
            services.AddHttpClient(FlightsSchema,
                client => client.BaseAddress = new Uri("http://localhost:5053/graphql"));
            services.AddHttpClient(AirlinesSchema,
                client => client.BaseAddress = new Uri("http://localhost:5054/graphql"));

            services.AddGraphQLServer()
                .AddRemoteSchema(AircraftHangarSchema, true)
                .AddRemoteSchema(AirportsSchema, true)
                .AddRemoteSchema(FlightsSchema, false)
                .AddRemoteSchema(AirlinesSchema, true)
                //.AddQueryType(descriptor => descriptor.Name("Query"))
                .AddTypeExtensionsFromFile("./Stitching.graphql");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapGraphQL(); });
        }
    }
}