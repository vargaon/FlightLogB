namespace FlightLogNet
{
    using AutoMapper;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Startup(IConfiguration configuration)
    {
        private const string AllowedOrigins = "AllowedOrigins";

        public IConfiguration Configuration { get; } = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InjectConfiguration.Initialization(services);
            services.AddControllers();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            // services.AddAutoMapper(System.Reflection.Assembly.GetCallingAssembly());
            services.AddCors(options => {
                options.AddDefaultPolicy(
                    policy => {
                        string[] origins = this.Configuration.GetSection(AllowedOrigins).Get<string[]>();
                        if (origins is null)
                        {
                            policy.AllowAnyOrigin();
                        }
                        else
                        {
                            policy.WithOrigins(origins);
                        }
                    });
            });
            services.AddOpenApiDocument(document => {
                document.Title = "FlighLog";
            }); // registers a OpenAPI v3.0 document with the name "v1"
        }

        // ReSharper disable once UnusedMember.Global - used by Framework
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();

            app.UseOpenApi(); // Serves the registered OpenAPI/Swagger documents by default on `/swagger/{documentName}/swagger.json`
            app.UseSwaggerUi(); // Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`

            app.UseDefaultFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                TestDatabaseGenerator.RenewDatabase(this.Configuration);
            }
            else
            {
                using var dbContext = new Repositories.LocalDatabaseContext(this.Configuration);
                bool newlyCreated = dbContext.Database.EnsureCreated();
                if (newlyCreated)
                {
                    TestDatabaseGenerator.InitializeDatabase(this.Configuration);
                }
            }

            app.UseRouting();
            app.UseCors();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
